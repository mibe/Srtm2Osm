using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Brejc.Geometry;

namespace Brejc.DemLibrary
{
    //The DEM is provided as 16-bit signed integer data in a simple binary raster. There are no header 
    //or trailer bytes embedded in the file. The data are stored in row major order (all the data for row 
    //1, followed by all the data for row 2, etc.).
    //All elevations are in meters referenced to the WGS84/EGM96 geoid as documented at 
    //http://www.NGA.mil/GandG/wgsegm/.
    //Byte order is Motorola ("big-endian") standard with the most significant byte first. Since they are 
    //signed integers elevations can range from -32767 to 32767 meters, encompassing the range of 
    //elevation to be found on the Earth.
    //These data also contain occassional voids from a number of causes such as shadowing, phase 
    //unwrapping anomalies, or other radar-specific causes. Voids are flagged with the value -32768.    

    public class Srtm3Storage : IDemLoader
    {
        public IActivityLogger ActivityLogger { get { return activityLogger; } set { activityLogger = value; } }

        public SrtmIndex SrtmIndex
        {
            get { return index; }
            set { index = value; }
        }

        public string Srtm3CachePath
        {
            get { return srtm3CachePath; }
            set { srtm3CachePath = value; }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Srtm3Storage"/> class using a specified cache directory
        /// where the downloaded SRTM files are stored.
        /// </summary>
        /// <param name="srtm3CachePath">The SRTM cache path.</param>
        public Srtm3Storage (string srtm3CachePath)
        {
            Srtm3CachePath = srtm3CachePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Srtm3Storage"/> class using a specified cache directory
        /// where the downloaded SRTM files are stored and a SRTM index file.
        /// </summary>
        /// <param name="srtm3CachePath">The SRTM cache path.</param>
        /// <param name="index">The SRTM index file.</param>
        public Srtm3Storage (string srtm3CachePath, SrtmIndex index) : this (srtm3CachePath)
        {
            this.index = index;
        }

        public IDigitalElevationModel LoadDemForArea (Bounds2 bounds)
        {
            // make sure the cache directory exists
            Directory.CreateDirectory (srtm3CachePath);

            // first create a list of geographicals cells which constitute the specified area
            IDictionary<int, Srtm3Cell> cellsToUse = new Dictionary<int, Srtm3Cell> ();

            for (int lat = CalculateCellDegrees (bounds.MinY); lat <= CalculateCellDegrees (bounds.MaxY); lat++)
            {
                for (int lng = CalculateCellDegrees (bounds.MinX); lng <= CalculateCellDegrees (bounds.MaxX); lng++)
                {
                    Srtm3Cell cell = new Srtm3Cell ((Int16)lng, (Int16)lat);
                    cellsToUse.Add (Srtm3Cell.CalculateCellKey (cell), cell);
                }
            }

            // then fetch a list of already downloaded cells
            IDictionary<int, Srtm3Cell> cachedCells = FetchCachedCellsList ();

            try
            {
                WebClient webClient = new WebClient ();

                foreach (Srtm3Cell cell in cellsToUse.Values)
                {
                    // if it is not cached...
                    if (!cachedCells.ContainsKey (Srtm3Cell.CalculateCellKey (cell)))
                    {
                        // find the right subdirectory
                        SrtmContinentalRegion continentalRegion = index.GetValueForCell (cell.CellLon, cell.CellLat);

                        if (continentalRegion == SrtmContinentalRegion.None)
                        {
                            // the cell does not exist in the index, it is probably an ocean cell
                            // add an empty cell to the cache and go to the next cell
                            cachedCells.Add (Srtm3Cell.CalculateCellKey (cell), cell);
                            continue;
                        }

                        string filename = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}.zip", cell.CellFileName);
                        Uri uri = new Uri (srtmSource, continentalRegion + "/" + filename);

                        string localFilename = Path.Combine (srtm3CachePath, filename);

                        this.activityLogger.Log (ActivityLogLevel.Verbose, "Downloading SRTM cell " + cell.CellFileName);

                        webClient.DownloadFile (uri, localFilename);

                        // unzip it and delete the zip file
                        System.IO.Compression.ZipFile.ExtractToDirectory(localFilename, srtm3CachePath);
                        File.Delete (localFilename);
                    }

                    // now load it
                    cell.LoadFromCache (srtm3CachePath);

                    if (cell.BogusData)
                        activityLogger.Log (ActivityLogLevel.Warning, "Possible bogus data in cell.");
                }
            }
            finally
            {
            }
 
            // create elevation data
            int west, south, east, north;
            west = Srtm3Storage.CalculateCellPosition (bounds.MinX);
            south = Srtm3Storage.CalculateCellPosition (bounds.MinY);
            east = Srtm3Storage.CalculateCellPosition (bounds.MaxX);
            north = Srtm3Storage.CalculateCellPosition (bounds.MaxY);

            int width = east - west + 1;
            int height = north - south + 1;

            int loadCounter = 1;

            RasterDigitalElevationModelFactory factory = new RasterDigitalElevationModelFactory (1200, 1200, west, south, width, height);
            factory.ActivityLogger = activityLogger;
            RasterDigitalElevationModelBase dem = factory.CreateSupportedModel();

            if (dem == null)
                throw new PlatformNotSupportedException("No suitable location for the DEM found.");

            // and fill the DEM with each cell points
            foreach (Srtm3Cell cell in cellsToUse.Values)
            {
                this.activityLogger.LogFormat (ActivityLogLevel.Normal,
                    "Loading cell {0} of {1} into DEM", loadCounter++, cellsToUse.Values.Count);

                dem.CopyElevationPointsFrom (cell);
            }

            return dem;
        }

        public IDictionary<int, Srtm3Cell> FetchCachedCellsList ()
        {
            Dictionary<int, Srtm3Cell> cachedCells = new Dictionary<int, Srtm3Cell> ();

            DirectoryInfo cacheDir = new DirectoryInfo (Srtm3CachePath);

            FileInfo[] files = null;
            try
            {
                files = cacheDir.GetFiles ("*.hgt");

                foreach (FileInfo file in files)
                {
                    Srtm3Cell cell = Srtm3Cell.CreateSrtm3Cell (file.Name, false);
                    cachedCells.Add (Srtm3Cell.CalculateCellKey (cell), cell);
                }
            }
            catch (DirectoryNotFoundException)
            {
                // no cache found, skip 
            }

            return cachedCells;
        }

        public void ClearStorage ()
        {
            DirectoryInfo dir = new DirectoryInfo (Srtm3CachePath);
            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles ("*"))
                    file.Delete ();
            }
        }

        static public int CalculateCellPosition (double angle)
        {
            return (int)(angle * 1200 + 1.5);
        }

        /// <summary>
        /// For a given latitude or longitude angle calculates integer degrees value of the cell to which the angle belongs.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>Integer degrees value of the cell to which the angle belongs</returns>
        static public int CalculateCellDegrees (double angle)
        {
            if (angle >= 0)
                return Angular.GetDegrees (angle);

            return Angular.GetDegreesFloor (angle);
        }

        private SrtmIndex index;

        private string srtm3CachePath;

        private static Uri srtmSource = new Uri ("http://firmware.ardupilot.org/SRTM/");

        private IActivityLogger activityLogger = new ConsoleActivityLogger ();
    }
}
