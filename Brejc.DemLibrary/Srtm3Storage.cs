using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Brejc.Geometry;

namespace Brejc.DemLibrary
{
    // The DEM is provided as 16-bit signed integer data in a simple binary raster. There are no header 
    // or trailer bytes embedded in the file. The data are stored in row major order (all the data for row 
    // 1, followed by all the data for row 2, etc.).
    // All elevations are in meters referenced to the WGS84/EGM96 geoid as documented at 
    // http://www.NGA.mil/GandG/wgsegm/.
    // Byte order is Motorola ("big-endian") standard with the most significant byte first. Since they are 
    // signed integers elevations can range from -32767 to 32767 meters, encompassing the range of 
    // elevation to be found on the Earth.
    // These data also contain occassional voids from a number of causes such as shadowing, phase 
    // unwrapping anomalies, or other radar-specific causes. Voids are flagged with the value -32768.    

    public class Srtm3Storage : IDemLoader
    {
        public IActivityLogger ActivityLogger { get { return activityLogger; } set { activityLogger = value; } }

        public SrtmIndex Index
        {
            get { return index; }
            set { index = value; }
        }

        public string CachePath { get; set; }

        public Uri Source
        {
            get { return this.srtmSource; }
            set 
            {
                if (value != null)
                    this.srtmSource = value;
            }
        }

        public string SourceExtension { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Srtm3Storage"/> class using a specified cache directory
        /// where the downloaded SRTM files are stored.
        /// </summary>
        /// <param name="cachePath">The SRTM cache path.</param>
        public Srtm3Storage (string cachePath)
        {
            CachePath = cachePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Srtm3Storage"/> class using a specified cache directory
        /// where the downloaded SRTM files are stored and a SRTM index file.
        /// </summary>
        /// <param name="cachePath">The SRTM cache path.</param>
        /// <param name="index">The SRTM index file.</param>
        public Srtm3Storage (string cachePath, SrtmIndex index) : this (cachePath)
        {
            this.Index = index;
        }

        public IDigitalElevationModel LoadDemForArea (Bounds2 bounds)
        {
            // make sure the cache directory exists
            Directory.CreateDirectory (CachePath);

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
                        // find the right subdirectory (or use none for flat structure)

                        SrtmContinentalRegion continentalRegion = Index.GetValueForCell (cell.CellLon, cell.CellLat);

                        if (continentalRegion == SrtmContinentalRegion.None)
                        {
                            // the cell does not exist in the index, it is probably an ocean cell
                            // add an empty cell to the cache and go to the next cell
                            cachedCells.Add (Srtm3Cell.CalculateCellKey (cell), cell);
                            continue;
                        }

                        // either use flat or structured source
                        var filename = cell.CellFileName.Replace(".hgt", SourceExtension);
                        var sourcefilename = filename;
                        if (continentalRegion != SrtmContinentalRegion.Flat)
                        {
                            sourcefilename = string.Format (CultureInfo.InvariantCulture, "{0}/{1}", continentalRegion, filename);
                        }
                        string localfile = Path.Combine(CachePath, filename);

                        if (Source.Scheme == "file")
                        {
                            var sourceFile = string.Format (CultureInfo.InvariantCulture, "{0}/{1}", Source.AbsolutePath, sourcefilename);
                            ActivityLogger.LogFormat(ActivityLogLevel.Verbose, "Use SRTM cell {0} from file {1}", cell.CellFileName, sourceFile);

                            // unzip, but do not delete the source file
                            System.IO.Compression.ZipFile.ExtractToDirectory(sourceFile, CachePath);
                        }
                        else
                        {
                            ActivityLogger.LogFormat (ActivityLogLevel.Verbose, "Downloading SRTM cell {0}", cell.CellFileName);

                            Uri uri = new Uri(Source, sourcefilename);

                            webClient.DownloadFile (uri, localfile);

                            // unzip it and delete the zip file
                            System.IO.Compression.ZipFile.ExtractToDirectory (localfile, CachePath);
                            File.Delete(localfile);
                        }
                    }

                    // now load it
                    cell.LoadFromCache (CachePath);

                    if (cell.BogusData)
                        ActivityLogger.Log (ActivityLogLevel.Warning, "Possible bogus data in cell.");
                }
            }
            finally
            {
            }
 
            // create elevation data
            int west, south, east, north;
            west = CalculateCellPosition (bounds.MinX);
            south = CalculateCellPosition (bounds.MinY);
            east = CalculateCellPosition (bounds.MaxX);
            north = CalculateCellPosition (bounds.MaxY);

            int width = east - west + 1;
            int height = north - south + 1;

            int loadCounter = 1;

            RasterDigitalElevationModelFactory factory = new RasterDigitalElevationModelFactory (1200, 1200, west, south, width, height);
            factory.ActivityLogger = ActivityLogger;
            RasterDigitalElevationModelBase dem = factory.CreateSupportedModel();

            if (dem == null)
                throw new PlatformNotSupportedException("No suitable location for the DEM found.");

            // and fill the DEM with each cell points
            foreach (Srtm3Cell cell in cellsToUse.Values)
            {
                ActivityLogger.LogFormat (ActivityLogLevel.Normal,
                    "Loading cell {0} of {1} into DEM", loadCounter++, cellsToUse.Values.Count);

                dem.CopyElevationPointsFrom (cell);
            }

            return dem;
        }

        public IDictionary<int, Srtm3Cell> FetchCachedCellsList ()
        {
            Dictionary<int, Srtm3Cell> cachedCells = new Dictionary<int, Srtm3Cell> ();

            DirectoryInfo cacheDir = new DirectoryInfo (CachePath);

            try
            {
                var files = cacheDir.GetFiles ("*.hgt");

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
            DirectoryInfo dir = new DirectoryInfo (CachePath);
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

        private Uri srtmSource = new Uri ("http://firmware.ardupilot.org/SRTM/");

        private IActivityLogger activityLogger = new ConsoleActivityLogger ();
    }
}
