using System;
using System.Configuration;
using System.IO;

namespace PUELicencia_ImprimeCard
{
    class CommandLineOptions
    {
        public bool checkSupplies;
        public bool magstripe;
        public bool jobCompletion;
        public bool portraitBack;
        public bool portraitFront;
        public bool rotateBack;
        public bool rotateFront;
        public bool showXml;
        public bool twoPages;
        public short numCopies = 1;
        public string printerName;
        public string topcoatBlockingBack;
        public string topcoatBlockingFront;

        public enum DisablePrinting { All, Off, Front, Back }
        public DisablePrinting disablePrinting = DisablePrinting.Off;
        public LaminationActions.Actions L1Action = LaminationActions.Actions.doesNotApply;
        public LaminationActions.Actions L2Action = LaminationActions.Actions.doesNotApply;

        private static void Usage()
        {
            string nameImpresora = ConfigurationManager.AppSettings["NameDataCard"].ToString();
            string thisExeName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            Console.WriteLine(thisExeName + " demonstrates print functionality of the printer and driver.");
            Console.WriteLine();
            Console.WriteLine("Uses hardcoded data for printing, magnetic stripe, topcoat region and");
            Console.WriteLine("print blocking region.");
            Console.WriteLine();
            Console.WriteLine("This sample changes the driver printing preference settings.");
            Console.WriteLine();
            Console.WriteLine(thisExeName + " -n <printername> [-r front | -r back | -r both] [-2]");
            Console.WriteLine("  [-o frontPort | -o backPort | -o bothPort] [ -s <number of copies>]");
            Console.WriteLine("  [-m ] [-d] [-x]");
            Console.WriteLine("  [-t all | -t chip | -t magJIS | -t mag2 | -t mag3 | -t custom]");
            Console.WriteLine("  [-u all | -u chip | -u magJIS | -u mag2 | -u mag3 | -u custom]");
            Console.WriteLine();
            Console.WriteLine("options:");
            //Console.WriteLine("  -n <printername>. Required. Try -n \"XPS Card Printer (Copy 1)\".");
            Console.WriteLine("  -n <printername>. Required. Try -n \"" + nameImpresora + "\".");
            Console.WriteLine("  -r <front | back | both >. Rotates the card image by 180 degrees for");
            Console.WriteLine("     front, back, or both sides.");
            Console.WriteLine("  -2 Prints a 2-sided (duplex) card. Default is front-side printing.");
            Console.WriteLine("  -o <frontPort | backPort | bothPort>. Sets portrait orientation");
            Console.WriteLine("     for a card side. Default is landscape orientation for both card sides.");
            Console.WriteLine("  -s <number of copies>. Default is 1.");
            Console.WriteLine("  -m Writes 3-track magnetic stripe data to backside of card using escapes.");
            Console.WriteLine("     Default is no encoding.");
            Console.WriteLine("  -d < All | Off | Front | Back > Disable Printing. Default is Off.");
            Console.WriteLine("  -x Display the print ticket data. Default is no display.");
            Console.WriteLine("  -t <all | chip | magJIS | mag2 | mag3 | custom> Top coat and print blocking");
            Console.WriteLine("     region for front of card. Use '-t all' to topcoat the entire card side");
            Console.WriteLine("     with no print blocking. Default is the current driver setting.");
            Console.WriteLine("  -u <all | chip | magJIS | mag2 | mag3 | custom> Top coat and print blocking");
            Console.WriteLine("     region for for back of card. Use '-u all' to topcoat the entire card side");
            Console.WriteLine("     with no print blocking. Default is the current driver setting.");
            Console.WriteLine("  -i Check supplies before printing. Default is to not check.");
            Console.WriteLine("  -c Poll for job completion.");
            Console.WriteLine();
            //Console.WriteLine(thisExeName + " -n \"XPS Card Printer (Copy 1)\"");
            Console.WriteLine(thisExeName + " -n \"" + nameImpresora + "\"");
            Console.WriteLine("  Prints a one-sided landscape card.");
            Console.WriteLine();
            //Console.WriteLine(thisExeName + " -n \"XPS Card Printer (Copy 1)\" -r both -2 -o frontport");
            Console.WriteLine(thisExeName + " -n \"" + nameImpresora + "\" -r both -2 -o frontport");
            Console.WriteLine("  Prints a two-sided card with both sides of card image rotated 180 degrees and");
            Console.WriteLine("  with front side as portrait orientation and back side as landscape");
            Console.WriteLine("  orientation.");
            Console.WriteLine();
            //Console.WriteLine(thisExeName + " -n \"XPS Card Printer (Copy 1)\" -2 -t all -u mag3");
            Console.WriteLine(thisExeName + " -n \"" + nameImpresora + "\" -2 -t all -u mag3");
            Console.WriteLine("  Prints a two-sided card with topcoat applied over all of side one and topcoat");
            Console.WriteLine("  and printing blocked over the 3-track magnetic stripe area on the back of the");
            Console.WriteLine("  card.");
            Environment.Exit(-1);
        }

        public void Validate()
        {

            if (string.IsNullOrEmpty(printerName))
            {
                Console.WriteLine("printer name is required");
                Environment.Exit(-1);
            }

            if (numCopies <= 0)
            {
                Console.WriteLine("invalid number of copies: {0}", numCopies);
                Environment.Exit(-1);
            }

            switch (topcoatBlockingFront.ToLower())
            {
                case "": break;
                case "all": break;
                case "chip": break;
                case "magjis": break;
                case "mag2": break;
                case "mag3": break;
                case "custom": break;
                default:
                    Console.WriteLine("invalid front topcoat / blocking option: {0}", topcoatBlockingFront);
                    Environment.Exit(-1);
                    break;
            }

            switch (topcoatBlockingBack.ToLower())
            {
                case "": break;
                case "all": break;
                case "chip": break;
                case "magjis": break;
                case "mag2": break;
                case "mag3": break;
                case "custom": break;
                default:
                    Console.WriteLine("invalid back topcoat / blocking option: {0}", topcoatBlockingBack);
                    Environment.Exit(-1);
                    break;
            }
        }

        static public CommandLineOptions CreateFromArguments(string[] args)
        {

            if (args.Length == 0) Usage();

            CommandLineOptions commandLineOptions = new CommandLineOptions();
            CommandLine.Utility.Arguments arguments = new CommandLine.Utility.Arguments(args);

            if (!string.IsNullOrEmpty(arguments["h"])) Usage();

            commandLineOptions.printerName = arguments["n"];
            commandLineOptions.showXml = !string.IsNullOrEmpty(arguments["x"]);
            commandLineOptions.twoPages = !string.IsNullOrEmpty(arguments["2"]);
            commandLineOptions.magstripe = !string.IsNullOrEmpty(arguments["m"]);
            commandLineOptions.checkSupplies = !string.IsNullOrEmpty(arguments["i"]);
            commandLineOptions.jobCompletion = !string.IsNullOrEmpty(arguments["c"]);

            if (!string.IsNullOrEmpty(arguments["d"]))
            {
                switch (arguments["d"].ToLower())
                {
                    case "all":
                        commandLineOptions.disablePrinting = DisablePrinting.All; break;
                    case "off":
                        commandLineOptions.disablePrinting = DisablePrinting.Off; break;
                    case "front":
                        commandLineOptions.disablePrinting = DisablePrinting.Front; break;
                    case "back":
                        commandLineOptions.disablePrinting = DisablePrinting.Back; break;
                }
            }

            commandLineOptions.topcoatBlockingFront =
               string.IsNullOrEmpty(arguments["t"]) ? string.Empty : arguments["t"].ToLower();
            commandLineOptions.topcoatBlockingBack =
               string.IsNullOrEmpty(arguments["u"]) ? string.Empty : arguments["u"].ToLower();

            if (!string.IsNullOrEmpty(arguments["s"]))
            {
                commandLineOptions.numCopies = short.Parse(arguments["s"]);
            }

            if (!string.IsNullOrEmpty(arguments["r"]))
            {
                switch (arguments["r"].ToLower())
                {
                    case "front":
                        commandLineOptions.rotateFront = true; break;
                    case "back":
                        commandLineOptions.rotateBack = true; break;
                    case "both":
                        commandLineOptions.rotateFront = true;
                        commandLineOptions.rotateBack = true;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(arguments["o"]))
            {
                switch (arguments["o"].ToLower())
                {
                    case "frontport":
                        commandLineOptions.portraitFront = false; break;
                    case "backport":
                        commandLineOptions.portraitBack = true; break;
                    case "bothport":
                        commandLineOptions.portraitFront = true;
                        commandLineOptions.portraitBack = true;
                        break;
                }
            }

            //SE AGREGA PARA LAMINACION
            bool laminationActionSpecified = false;

            string laminationActionArg = arguments["1"];
            if (!string.IsNullOrEmpty(laminationActionArg))
            {
                laminationActionSpecified = true;
                commandLineOptions.L1Action = LaminationActions.GetLaminationAction(laminationActionArg);
            }

            laminationActionArg = arguments["3"];
            if (!string.IsNullOrEmpty(laminationActionArg))
            {
                laminationActionSpecified = true;
                commandLineOptions.L2Action = LaminationActions.GetLaminationAction(laminationActionArg);
            }


            return commandLineOptions;
        }
    }
}
