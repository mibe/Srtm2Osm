using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace Brejc.DemLibrary
{
    [Serializable]
    public class Srtm3Cell : RasterDigitalElevationModelBase
    {
        public Int16 CellLat
        {
            get { return (Int16) LatOffset; }
        }

        public Int16 CellLon
        {
            get { return (Int16) LonOffset; }
        }

        public bool IsLoaded { get { return data != null; } }

        public bool BogusData { get; private set; }

        public string CellFileName
        {
            get
            {
                return String.Format (System.Globalization.CultureInfo.InvariantCulture,
                    "{0}{1:00}{2}{3:000}.hgt",
                    CellLat >= 0 ? 'N' : 'S',
                    Math.Abs (CellLat),
                    CellLon >= 0 ? 'E' : 'W',
                    Math.Abs (CellLon));
            }
        }

        private static Regex regex = new Regex("^(?'latSign'[N|S])(?'latitude'[0-9]{2})(?'longSign'[W|E])(?'longitude'[0-9]{3})",
            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled);


        public Srtm3Cell (Int16 longitude, Int16 latitude)
            : base (1200, 1200, longitude, latitude, 1201, 1201)
        {
        }

        public static Srtm3Cell CreateSrtm3Cell (string fileName, bool load)
        {
            Match match = regex.Match (fileName);
            if (!match.Success)
                throw new ArgumentException ("Invalid filename", "fileName");

            char latSign = match.Groups["latSign"].Value[0];
            Int16 cellLat = Int16.Parse (match.Groups["latitude"].Value, System.Globalization.CultureInfo.InvariantCulture);
            char lonSign = match.Groups["longSign"].Value[0];
            Int16 cellLon = Int16.Parse (match.Groups["longitude"].Value, System.Globalization.CultureInfo.InvariantCulture);
            if ((latSign == 'S') || (latSign == 's'))
                cellLat = (Int16)(-cellLat);
            if ((lonSign == 'W') || (lonSign == 'w'))
                cellLon = (Int16)(-cellLon);

            Srtm3Cell cell = new Srtm3Cell (cellLon, cellLat);

            if (load)
                cell.LoadFromFile (new FileInfo (fileName));

            return cell;
        }

        [SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "lat+90")]
        [SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "lng+180")]
        public static int CalculateCellKey (int lng, int lat)
        {
            int key = ((lng + 180) << 16) | (lat + 90);
            return key;
        }

        [SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "lng+180")]
        public static int CalculateCellKey (Srtm3Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException ("cell");
            
            int key = (((int)cell.CellLon + 180) << 16) | ((int)cell.CellLat + 90);
            return key;
        }

        public void LoadFromFile (FileInfo file)
        {
            // SRTM3 cells are always 2884802 bytes long.
            if (file.Length != 2884802)
                BogusData = true;
            
            using (Stream stream = file.OpenRead())
            {
                BinaryReader reader = new BinaryReader (stream);

                data = reader.ReadBytes ((int)stream.Length);
            }
        }

        public void LoadFromCache (string cacheDir)
        {
            string filePath = Path.Combine (cacheDir, CellFileName);
            LoadFromFile (new FileInfo(filePath));
        }

        [SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "localLon*2")]
        [SuppressMessage ("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "1201-localLat")]
        public override double GetElevationForDataPoint (int localLon, int localLat)
        {
            // if the cell is empty, return "missing value"
            if (data == null)
                return Int16.MinValue;

            int bytesPos = ((1201 - localLat - 1) * 1201 * 2) + localLon * 2;

            return (Int16)((data[bytesPos]) << 8 | data[bytesPos + 1]);
        }

        public override void SetElevationForDataPoint (int localLon, int localLat, double elevation)
        {
            throw new NotImplementedException ("The method or operation is not implemented.");
        }

        public override object Clone ()
        {
            throw new NotImplementedException ("The method or operation is not implemented.");
        }

        private byte[] data;
    }
}
