using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace Brejc.DemLibrary
{
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SrtmContinentalRegion : byte
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
        Flat
    }

    public class SrtmIndex
    {
        // NOTE: names matching directory names on the srtm server http://firmware.ardupilot.org/SRTM/
        // other directory structure is not supported, but we support unstructured (flat) sources
        // with SrtmContinentalRegion.Flat.
        private static readonly List<SrtmContinentalRegion> StructuredRegions = new List<SrtmContinentalRegion> {
            SrtmContinentalRegion.Australia,
            SrtmContinentalRegion.Eurasia,
            SrtmContinentalRegion.Africa,
            SrtmContinentalRegion.Islands,
            SrtmContinentalRegion.North_America,
            SrtmContinentalRegion.South_America
        };

        public IActivityLogger ActivityLogger { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "latitude+90")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "longitude+180")]
        public SrtmContinentalRegion GetValueForCell (int longitude, int latitude)
        {
            var index = longitude + 180 + 360 * (latitude + 90);
            if (index >= 0 && index < data.Length)
            {
                return (SrtmContinentalRegion)data[index];
            }
            return SrtmContinentalRegion.None;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "latitude+90")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "longitude+180")]
        public void SetValueForCell (int longitude, int latitude, SrtmContinentalRegion continentalRegion)
        {
            var index = longitude + 180 + 360 * (latitude + 90);
            if (index >= 0 && index < data.Length)
            {
                data[longitude + 180 + 360 * (latitude + 90)] = (byte)continentalRegion;
            }
        }

        private static Uri srtmSource = new Uri("http://firmware.ardupilot.org/SRTM/");
        public static bool SrtmSourceFlat { get; set; }
        private static string srtmSourceExtension = ".hgt.zip";

        public static Uri SrtmSource
        {
            get { return srtmSource; }
            set
            {
                if (value != null)
                    srtmSource = value;
            }
        }

        public static string SrtmSourceExtension
        {
            get { return srtmSourceExtension; }
            set { srtmSourceExtension = value; }
        }


        /// <summary>
        /// Generates an index file by listing all available SRTM cells in the source (on the FTP site/in the local folder).
        /// </summary>
        public void Generate ()
        {
            ActivityLogger.Log(ActivityLogLevel.Verbose, "Downloading data for SRTM index generation");

            if (SrtmSourceFlat)
            {
                GenerateRegion(SrtmContinentalRegion.Flat);
            }
            else
            {
                foreach (var region in StructuredRegions)
                {
                    GenerateRegion(region);
                }
            }
        }

        private void GenerateRegion (SrtmContinentalRegion region)
        {
            var uri = (region == SrtmContinentalRegion.Flat) ? SrtmSource : new Uri(SrtmSource, region + "/");
            bool isLocal = SrtmSource.Scheme == "file";
            if (isLocal)
            {
                GenerateFromLocal(uri, region);
            }
            else
            {
                GenerateFromRemote(uri, region);
            }
        }

        private void GenerateFromLocal (Uri uri, SrtmContinentalRegion continentalRegion)
        {
            // Example: "N00E006.hgt.zip" but extension may be overwritten by SrtmSourceExtension.
            // we have to escape the dots for use in regex
            var extension = SrtmSourceExtension.Replace(".", @"\.");
            var localFileNameRegex = new Regex(@"([NnSs]\d\d[WwEe]\d\d\d" + extension + ")", RegexOptions.IgnoreCase);

            var localFiles = 0;
            var searchPattern = "*" + SrtmSourceExtension;
            var files = Directory.GetFiles (uri.AbsolutePath, searchPattern);
            foreach (string file in files)
            {
                Match match = localFileNameRegex.Match (file);
                if (!match.Success)
                    continue;
                
                CreateCell(Path.GetFileName (file), continentalRegion);
                localFiles++;
            }
            ActivityLogger.LogFormat(ActivityLogLevel.Verbose, "- added {0} files to index from local (region {1})", localFiles, continentalRegion);
        }

        private void GenerateFromRemote (Uri uri, SrtmContinentalRegion continentalRegion)
        {
            // Example: "N00E006.hgt.zip" but extension may be overwritten by SrtmSourceExtension.
            // we have to escape the dots for use in regex
            var extension = SrtmSourceExtension.Replace(".", @"\.");
            var remoteFileNameRegex = new Regex("href=\"([A-Za-z0-9]*" + extension + ")\"", RegexOptions.IgnoreCase);
            var remoteFiles = 0;

            // Get the directory listing from the server
            WebClient webClient = new WebClient ();
            string responseFromServer = webClient.DownloadString (uri);

            // Find files and process each match.
            var matches = remoteFileNameRegex.Matches (responseFromServer);
            foreach (Match match in matches)
            {
                string filename = match.Groups[1].Value.Trim();
                if (filename.Length == 0)
                    continue;

                CreateCell (filename, continentalRegion);
                remoteFiles++;
            }
            ActivityLogger.LogFormat(ActivityLogLevel.Verbose, "- added {0} files to index from remote (region {1})", remoteFiles, continentalRegion);
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
            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// Loads a SRTM index from a specified file.
        /// Check length to avoid the use of older incompatible files.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public bool Load (string filePath)
        {
            var bytes = File.ReadAllBytes (filePath);
            if (bytes.Length == data.Length)
            {
                data = bytes;
                return true;
            }
            return false;
        }

        /// data of the index file = 360x180x4 Bytes = 64'800 bytes
        /// (former implementation used 259'200 integer values)
        /// the array is accessed by <see cref="GetValueForCell"/> and <see cref="SetValueForCell"/> 
        private byte[] data = new byte[360*180];
    }
}
