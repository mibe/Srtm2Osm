using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics.CodeAnalysis;

namespace Brejc.DemLibrary
{
    // NOTE: names correspond to actual directory names on the srtm ftp server!
    public enum SrtmContinentalRegion
    {
        None,
        Australia,
        Eurasia,
        Africa,
        Islands,
        [SuppressMessage ("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        North_America,
        [SuppressMessage ("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        South_America,
        End,
    }

    [Serializable]
    public class SrtmIndex
    {
        public IActivityLogger ActivityLogger { get { return activityLogger; } set { activityLogger = value; } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "latitude+90")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "longitude+180")]
        public SrtmContinentalRegion GetValueForCell (int longitude, int latitude)
        {
            return (SrtmContinentalRegion)data[longitude + 180 + 360 * (latitude + 90)];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "latitude+90")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "longitude+180")]
        public void SetValueForCell (int longitude, int latitude, SrtmContinentalRegion continentalRegion)
        {
            data[longitude + 180 + 360 * (latitude + 90)] = (int)continentalRegion;
        }

        public static Uri SrtmSource
        {
            get { return srtmSource; }
            set
            {
                if (value != null)
                    srtmSource = value;
            }
        }

        private static Uri srtmSource = new Uri ("http://firmware.ardupilot.org/SRTM/");

        // Example file: "N00E006.hgt.zip"
        private static Regex remoteFileNameRegex = new Regex("href=\"([A-Za-z0-9]*\\.hgt\\.zip)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex localFileNameRegex = new Regex("([A-Za-z0-9]*\\.hgt\\.zip)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Generates an index file by listing all available SRTM cells on the FTP site.
        /// </summary>
        public void Generate ()
        {
            activityLogger.Log(ActivityLogLevel.Verbose, "Downloading data for SRTM index generation");

            bool isLocal = srtmSource.Scheme == "file";

            for (SrtmContinentalRegion continentalRegion = (SrtmContinentalRegion.None + 1); 
                 continentalRegion < SrtmContinentalRegion.End;
                 continentalRegion++)
            {
                string region = continentalRegion.ToString();
                Uri uri = new Uri (srtmSource, region + "/");

                if (isLocal)
                    GenerateFromLocal (uri, continentalRegion);
                else
                    GenerateFromRemote (uri, continentalRegion);
            }
        }

        private void GenerateFromLocal (Uri uri, SrtmContinentalRegion continentalRegion)
        {
            string[] files = Directory.GetFiles (uri.AbsolutePath, "*.zip");
            foreach (string file in files)
            {
                Match match = localFileNameRegex.Match (file);
                if (!match.Success)
                    continue;
                
                CreateCell(Path.GetFileName (file), continentalRegion);
            }
        }

        private void GenerateFromRemote (Uri uri, SrtmContinentalRegion continentalRegion)
        {
            // Get the directory listing from the server
            WebClient webClient = new WebClient ();
            string responseFromServer = webClient.DownloadString (uri);

            // Find files and process each match.
            MatchCollection matches = remoteFileNameRegex.Matches (responseFromServer);
            foreach (Match match in matches)
            {
                string filename = match.Groups[1].Value.Trim();
                if (filename.Length == 0)
                    continue;

                CreateCell (filename, continentalRegion);
            }
        }

        private void CreateCell(string filename, SrtmContinentalRegion continentalRegion)
        {
            Srtm3Cell srtm3Cell = Srtm3Cell.CreateSrtm3Cell(filename, false);
            SetValueForCell(srtm3Cell.CellLon, srtm3Cell.CellLat, continentalRegion);
        }

        /// <summary>
        /// Saves the SRTM index file to a specified location.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save (string filePath)
        {
            using (FileStream file = File.Open (filePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter ();
                formatter.Serialize (file, this);
            }
        }

        /// <summary>
        /// Loads a SRTM index from a specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        static public SrtmIndex Load (string filePath)
        {
            SrtmIndex index;

            using (FileStream file = File.Open (filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter ();
                index = formatter.Deserialize (file) as SrtmIndex;
            }

            return index;
        }

        private int[] data = new int[360*180];

        [NonSerialized()]
        private IActivityLogger activityLogger;
    }
}
