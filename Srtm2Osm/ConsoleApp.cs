using System;
using System.Collections.Generic;
using System.Text;
using Brejc.Common.Console;

namespace Srtm2Osm
{
    public class ConsoleApp : Brejc.Common.Console.ConsoleApplicationBase
    {
        public ConsoleApp (string[] args) : base (args) {}

        public override IList<IConsoleApplicationCommand> ParseArguments ()
        {
            if (Args.Length == 0)
                return null;

            List<IConsoleApplicationCommand> cmdList = new List<IConsoleApplicationCommand> ();
            IConsoleApplicationCommand cmd = new Srtm2OsmCommand ();

            cmd.ParseArgs (Args, 0);
            cmdList.Add (cmd);

            return cmdList;
        }

        public override void ShowBanner ()
        {
            System.Diagnostics.FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo
                (System.Reflection.Assembly.GetExecutingAssembly ().Location);
            Console.Out.WriteLine ("Srtm2Osm v{0} by Igor Brejc and others", version.FileVersion);
            Console.Out.WriteLine ();
            Console.Out.WriteLine ("Uses SRTM data to generate elevation contour lines for use in OpenStreetMap");
            Console.Out.WriteLine ();
        }

        public override void ShowHelp ()
        {
            Console.Out.WriteLine ();
            Console.Out.WriteLine ("USAGE:");
            Console.Out.WriteLine ("Srtm2Osm <bounds> <options>");
            Console.Out.WriteLine ();
            Console.Out.WriteLine ("BOUNDS (choose one):");
            Console.Out.WriteLine ("-bounds1 <minLat> <minLng> <maxLat> <maxLng>: specifies the area to cover");
            Console.Out.WriteLine ("-bounds2 <lat> <lng> <boxsize (km)>: specifies the area to cover");
            Console.Out.WriteLine ("-bounds3 <slippymap link>: specifies the area to cover using the URL from a map");
            Console.Out.WriteLine ("All bound parameters can be specified more than once.");
            Console.Out.WriteLine ();
            Console.Out.WriteLine ("OPTIONS:");
            Console.Out.WriteLine ("-o <path>: specifies an output OSM file (default: 'srtm.osm')");
            Console.Out.WriteLine ("-merge <path>: specifies an OSM file to merge with the output");
            Console.Out.WriteLine ("-d <path>: specifies a SRTM cache directory (default: 'Srtm')");
            Console.Out.WriteLine ("-i: forces the regeneration of SRTM index file (default: no)");
            Console.Out.WriteLine ("-feet: uses feet units for elevation instead of meters");
            Console.Out.WriteLine ("-step <elevation>: elevation step between contours (default: 20 units)");
            Console.Out.WriteLine ("-cat <major> <medium>: adds contour category tag to OSM ways");
            Console.Out.WriteLine ("       example: -cat 400 100 will mark:");
            Console.Out.WriteLine ("           contours 400, 800, 1200 etc as contour_ext=elevation_major");
            Console.Out.WriteLine ("           contours 100, 200, 300, 500 etc as contour_ext=elevation_medium");
            Console.Out.WriteLine ("           and all others as contour_ext=elevation_minor");
            Console.Out.WriteLine ("-large: runs in 'large area mode' in which each contour is written to OSM file ");
            Console.Out.WriteLine ("       immediately upon discovery. This prevents 'out-of-memory' errors.");
            Console.Out.WriteLine ("-corrxy <corrLng> <corrLat>: correction values to shift contours");
            Console.Out.WriteLine ("-source <url>: base URL used for download");
            Console.Out.WriteLine ("       (default 'https://terrain.ardupilot.org/data/SRTM/')");
            Console.Out.WriteLine ("-maxwaynodes <count>: specifies the maximum number of nodes in a single way");
            Console.Out.WriteLine ("-firstnodeid <id>: specifies the first ID of a node (default: 2^63-1)");
            Console.Out.WriteLine ("-firstwayid <id>: specifies the first ID of a way (default: 2^63-1)");
            Console.Out.WriteLine ("-incrementid: runs in 'ID incrementation mode' in which the OSM element ID is");
            Console.Out.WriteLine ("       incremented instead of decremented. Needs the firstnodeid and firstwayid");
            Console.Out.WriteLine ("       parameters set.");
            Console.Out.WriteLine ("-splitbounds <lat> <lng>: splits the given bound(s) in smaller parts. The");
            Console.Out.WriteLine ("       values specify the size of the area which is covered in a single");
            Console.Out.WriteLine ("       calculation run. This prevents out-of-memory errors.");
            Console.Out.WriteLine ();
        }

        public override void ExceptionHandler (Exception ex)
        {
#if DEBUG
            Console.Error.WriteLine(ex.ToString());
#endif

            Console.Error.WriteLine ();

            ArgumentException aex = ex as ArgumentException;
            if (aex != null)
            {
                Console.Error.WriteLine ("ERROR: {0}", ex.Message);
                ShowHelp ();
                Environment.Exit (1);
            }

            System.Net.WebException wex = ex as System.Net.WebException;
            if (wex != null)
            {
                Console.Error.WriteLine ("An error occurred while accessing the network:");
                Console.Error.WriteLine ("{0} ({1})", wex.Message, wex.Status);
                if (wex.Response != null)
                    Console.Error.WriteLine ("URI: {0}", wex.Response.ResponseUri);

                Environment.Exit (2);
            }

            Console.Error.WriteLine ("ERROR: {0}", ex);
            Environment.Exit (2);
        }
    }
}
