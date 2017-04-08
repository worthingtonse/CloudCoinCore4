using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
//using Newtonsoft.Json;
using System.Xml;

namespace Foundation
{
    class Program
    {
        /* INSTANCE VARIABLES */
        public static KeyboardReader reader = new KeyboardReader();
        //  public static String rootFolder = System.getProperty("user.dir") + File.separator +"bank" + File.separator ;
        public static String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static String importFolder = rootFolder + "Import" + Path.DirectorySeparatorChar;
        public static String importedFolder = rootFolder + "Imported" + Path.DirectorySeparatorChar;
        public static String trashFolder = rootFolder + "Trash" + Path.DirectorySeparatorChar;
        public static String suspectFolder = rootFolder + "Suspect" + Path.DirectorySeparatorChar;
        public static String frackedFolder = rootFolder + "Fracked" + Path.DirectorySeparatorChar;
        public static String bankFolder = rootFolder + "Bank" + Path.DirectorySeparatorChar;
        public static String templateFolder = rootFolder + "Templates" + Path.DirectorySeparatorChar;
        public static String counterfeitFolder = rootFolder + "Counterfeit" + Path.DirectorySeparatorChar;
        public static String directoryFolder = rootFolder + "Directory" + Path.DirectorySeparatorChar;
        public static String exportFolder = rootFolder + "Export" + Path.DirectorySeparatorChar;
        public static String languageFolder = rootFolder + "Language" + Path.DirectorySeparatorChar;
        public static String prompt = "> ";
        public static String[] commandsAvailable = new String[]{ "echo raida", "show coins", "import", "export", "fix fracked", "show folders", "export for sales", "quit" };
        public static string[] countries = new String[]{ "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Texas", "USA", "Romania", "Taiwan", "Moscow", "St. Petersburge", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };
      
        //{ "echo raida", "show coins", "import", "export", "fix fracked", "show folders", "export for sales", "quit" };
        public static int timeout = 10000; // Milliseconds to wait until the request is ended. 
        public static FileUtils fileUtils = new FileUtils(rootFolder, importFolder, importedFolder, trashFolder, suspectFolder, frackedFolder, bankFolder, templateFolder, counterfeitFolder, directoryFolder, exportFolder);
        public static Random myRandom = new Random();
  

        /* MAIN METHOD */
        public static void Main(String[] args)
        {
            // doc.Load("strings.xml");
            /*
            using (StreamReader file = File.OpenText( languageFolder + @"strings.json"))
                {
                   JsonSerializer serializer = new JsonSerializer();
                   StringHolderTemp st = (StringHolderTemp)serializer.Deserialize(file, typeof(StringHolderTemp));
                   StringHolder.loadNewlanguage(st);
                   commandsAvailable = new String[] { StringHolder.program_command_1, StringHolder.program_command_2, StringHolder.program_command_3, StringHolder.program_command_4, StringHolder.program_command_5, StringHolder.program_command_6, StringHolder.program_command_7, StringHolder.program_command_8 };
                   countries = new string[]{ StringHolder.raida0, StringHolder.raida1, StringHolder.raida2, StringHolder.raida3,StringHolder.raida4,StringHolder.raida5,StringHolder.raida6,StringHolder.raida7,StringHolder.raida8,StringHolder.raida9, StringHolder.raida10,StringHolder.raida11,StringHolder.raida12,StringHolder.raida13,
StringHolder.raida14, StringHolder.raida15, StringHolder.raida16, StringHolder.raida17, StringHolder.raida18, StringHolder.raida19, StringHolder.raida20, StringHolder.raida21, StringHolder.raida22, StringHolder.raida23, StringHolder.raida24 };

    }
            */
            printWelcome();
            run(); // Makes all commands available and loops
        } // End main

        /* STATIC METHODS */
        public static void run()
        {
            bool restart = false;
            while (!restart)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("");
                //  Console.Out.WriteLine("========================================");
                Console.Out.WriteLine("");
                Console.Out.WriteLine( StringHolder.program_run_1);//"Commands Available:";
                Console.ForegroundColor = ConsoleColor.White;
                int commandCounter = 1;
                foreach (String command in commandsAvailable)
                {
                    Console.Out.WriteLine(commandCounter + (". " + command));
                    commandCounter++;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.Write(prompt);
                Console.ForegroundColor = ConsoleColor.White;
                int commandRecieved = reader.readInt(1,9);
                switch (commandRecieved)
                {
                    case 1:
                        echoRaida();
                        break;
                    case 2:
                        showCoins();
                        break;
                    case 3:
                        import();
                        break;
                    case 4:
                        export();
                        break;
                    case 5:
                        fix();
                        break;
                    case 6:
                        showFolders();
                        break;
                    case 7:
                        dump();
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Out.WriteLine(StringHolder.program_run_2);//"Command failed. Try again.";
                        break;
                }// end switch
            }// end while
        }// end run method


        public static void printWelcome()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Out.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.Out.WriteLine( StringHolder.program_start_1 );//"║                  CloudCoin Foundation 2 v.4.07.17                ║");
            Console.Out.WriteLine(StringHolder.program_start_2);//"║          Used to Authenticate, Store and Payout CloudCoins       ║");
            Console.Out.WriteLine(StringHolder.program_start_3);//"║      This Software is provided as is with all faults, defects    ║");
            Console.Out.WriteLine(StringHolder.program_start_4);//"║          and errors, and without warranty of any kind.           ║");
            Console.Out.WriteLine(StringHolder.program_start_5);//"║                Free from the CloudCoin Consortium.               ║");
            Console.Out.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine( StringHolder.program_start_6 );//"Checking RAIDA...");
            echoRaida();
        } // End print welcome


        public static bool echoRaida()
        {
            RAIDA raida1 = new RAIDA(5000);
            Response[] results = raida1.echoAll(5000);
            int totalReady = 0;

            //For every RAIDA check its results
            int longestCountryName = 15;

            Console.Out.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                int padding = longestCountryName - countries[i].Length;
                string strPad = "";
                for (int j = 0; j < padding; j++)
                {
                    strPad += " ";
                }//end for padding

                if ( !RAIDA_Status.failsEcho[i])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.Write(countries[i] + strPad);
                    totalReady++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.Write( countries[i] + strPad);
                }
                if (i == 4 || i == 9 || i == 14 || i == 19) { Console.WriteLine(); }
            }//end for
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine( StringHolder.program_echo_1 + totalReady + " out of 25");//"RAIDA Health: " + totalReady );
            Console.Out.WriteLine();
            //Check if enough are good 
            if (totalReady < 16)//
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.Write("");
                Console.Out.Write(StringHolder.program_echo_3);// "Not enough RAIDA servers can be contacted to import new coins.");
                Console.Out.Write(StringHolder.program_echo_4);// "Is your device connected to the Internet?");
                Console.Out.Write(StringHolder.program_echo_5);// "Is a router blocking your connection?");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("");
                Console.Out.WriteLine(StringHolder.program_echo_6);// "The RAIDA is ready for counterfeit detection.");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }//end if enough RAIDA
        }//End echo

        public static void showCoins()
        {
            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //Output  " 12.3"
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.Out.WriteLine( StringHolder.program_showcoins_total + string.Format("{0,8:N0}", (bankTotals[0] + frackedTotals[0])) + "                               ║");
            Console.Out.WriteLine("╠══════════╦══════════╦══════════╦══════════╦══════════╦══════════╣");
            Console.Out.WriteLine("║          ║    1s    ║    5s    ║    25s   ║   100s   ║   250s   ║");
            Console.Out.WriteLine("╠══════════╬══════════╬══════════╬══════════╬══════════╬══════════╣");
            Console.Out.WriteLine( StringHolder.program_showcoins_perfect + string.Format("{0,7}", bankTotals[1]) + "  ║ " + string.Format("{0,7}", bankTotals[2]) + "  ║ " + string.Format("{0,7}", bankTotals[3]) + "  ║ " + string.Format("{0,7}", bankTotals[4]) + "  ║ " + string.Format("{0,7}", bankTotals[5]) + "  ║");
            Console.Out.WriteLine("╠══════════╬══════════╬══════════╬══════════╬══════════╬══════════╣");
            Console.Out.WriteLine( StringHolder.program_showcoins_fracked + string.Format("{0,7}", frackedTotals[1]) + "  ║ " + string.Format("{0,7}", frackedTotals[2]) + "  ║ " + string.Format("{0,7}", frackedTotals[3]) + "  ║ " + string.Format("{0,7}", frackedTotals[4]) + "  ║ " + string.Format("{0,7}", frackedTotals[5]) + "  ║");
            Console.Out.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╩══════════╝");
            Console.ForegroundColor = ConsoleColor.White;

        }// end show

        public static void showFolders()
        {
            Console.Out.WriteLine( StringHolder.program_showFolders_1 + "\n " + rootFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_2 + "\n  " + importFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_3 + "\n  " + importedFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_4 + "\n  " + suspectFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_5 + "\n  " + trashFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_6 + "\n  " + bankFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_7 + "\n  " + frackedFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_8 + "\n  " + templateFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_9 + "\n  " + directoryFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_10 + "\n  " + counterfeitFolder);
            Console.Out.WriteLine(StringHolder.program_showFolders_11 + "\n  " + exportFolder);
        } // end show folders

        public static void import()
        {
            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine( StringHolder.program_import_1 );//Finishing importing coins from last time...
                Console.ForegroundColor = ConsoleColor.White;
                detect();
                Console.Out.WriteLine( StringHolder.program_import_2 );// "Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine(StringHolder.program_import_3);// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine( importFolder);
            Console.ForegroundColor = ConsoleColor.White;
            Importer importer = new Importer(fileUtils);
            if (!importer.importAll())//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine(StringHolder.program_import_4);// "No coins in import folder.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                detect();
            }//end if coins to import
        }   // end import

        /*
                public static void importChest()
                {
                    Console.Out.WriteLine("What is the path to your chest file?");
                    string path = reader.readString(false);
                    Console.Out.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.WriteLine("Loading all CloudCoins in your import folder: " + importFolder);
                    Console.ForegroundColor = ConsoleColor.White;
                    Importer importer = new Importer(fileUtils);
                    if (!importer.importAll())//Moves all CloudCoins from the Import folder into the Suspect folder. 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("No coins in import folder.");
                        Console.ForegroundColor = ConsoleColor.White;
                        //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
                        String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
                        if (suspectFileNames.Length > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Out.WriteLine("Finishing importing coins from last time...");
                            Console.ForegroundColor = ConsoleColor.White;
                            detect();
                        } //end if there are files in the suspect folder that need to be imported
                    }
                    else
                    {
                        detect();
                    }//end if coins to import
                }   // end import
        */

        public static void detect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine( StringHolder.program_detect_1 );// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout);
            int[] detectionResults = detector.detectAll();
            Console.Out.WriteLine(StringHolder.program_detect_2 + detectionResults[0]);//"Total imported to bank: "
            Console.Out.WriteLine(StringHolder.program_detect_3 + detectionResults[2]);//"Total imported to fracked: "
            // And the bank and the fractured for total
            Console.Out.WriteLine( StringHolder.program_detect_4 + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine( StringHolder.program_detect_5 + detectionResults[3]);//"Total Kept in suspect folder: " 
            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
        }//end detect

        public static void dump()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine(StringHolder.program_dump_1);// "Export for sales will export stack files one note in them.");
            Console.Out.WriteLine(StringHolder.program_dump_2);// "Each file will recieve a random tag.");
            Console.Out.WriteLine(StringHolder.program_dump_3);// "This function helps you make CloudCoins for automated sales points.");
            Console.Out.WriteLine(StringHolder.program_dump_4);// "Continue to dump? 1.Yes or 2. No?");

            int okToDump = reader.readInt(1,2);
            if (okToDump==1)
            {
                Dumper dumper = new Dumper(fileUtils);
                Console.Out.WriteLine("");
                Banker bank = new Banker(fileUtils);
                int[] bankTotals = bank.countCoins(bankFolder);
                int[] frackedTotals = bank.countCoins(frackedFolder);
                Console.Out.WriteLine(StringHolder.program_dump_5);// "Your Bank Inventory:");
                int grandTotal = (bankTotals[0] + frackedTotals[0]);
                showCoins();
                // state how many 1, 5, 25, 100 and 250
                int exp_1 = 0;
                int exp_5 = 0;
                int exp_25 = 0;
                int exp_100 = 0;
                int exp_250 = 0;

                // 1 jpg 2 stack
                if ((bankTotals[1] + frackedTotals[1]) > 0)
                {
                    Console.Out.WriteLine(StringHolder.program_dump_6);// "How many 1s do you want to dump?");
                    exp_1 = reader.readInt(0, (bankTotals[1] + frackedTotals[1]));
                }

                // if 1s not zero 
                if ((bankTotals[2] + frackedTotals[2]) > 0)
                {
                    Console.Out.WriteLine(StringHolder.program_dump_7);//"How many 5s do you want to dump?");
                    exp_5 = reader.readInt(0, (bankTotals[2] + frackedTotals[2]));
                }

                // if 1s not zero 
                if ((bankTotals[3] + frackedTotals[3] > 0))
                {
                    Console.Out.WriteLine(StringHolder.program_dump_8);//"How many 25s do you want to dump?");
                    exp_25 = reader.readInt(0, (bankTotals[3] + frackedTotals[3]));
                }

                // if 1s not zero 
                if ((bankTotals[4] + frackedTotals[4]) > 0)
                {
                    Console.Out.WriteLine(StringHolder.program_dump_9);//"How many 100s do you want to dump?");
                    exp_100 = reader.readInt(0, (bankTotals[4] + frackedTotals[4]));
                }

                // if 1s not zero 
                if ((bankTotals[5] + frackedTotals[5]) > 0)
                {
                    Console.Out.WriteLine(StringHolder.program_dump_10);//"How many 250s do you want to dump?");
                    exp_250 = reader.readInt(0, (bankTotals[5] + frackedTotals[5]));
                }

                dumper.dumpSome(exp_1, exp_5, exp_25, exp_100, exp_250);



                Console.Out.WriteLine(StringHolder.program_dump_11);//"Export complete. Check your export folder");
                // And the bank and the fractured for total
                showCoins();
            }

        }//end detect

        public static void export()
        {
            Console.Out.WriteLine("");
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            Console.Out.WriteLine(StringHolder.program_dump_5);// "Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0]);
            showCoins();
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = 0;
            int exp_5 = 0;
            int exp_25 = 0;
            int exp_100 = 0;
            int exp_250 = 0;
            Console.Out.WriteLine(StringHolder.program_export_1);// "Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?");
            int file_type = reader.readInt(1, 2);
            // 1 jpg 2 stack
            if ((bankTotals[1] + frackedTotals[1]) > 0)
            {
                Console.Out.WriteLine(StringHolder.program_dump_6);//"How many 1s do you want to export?");
                exp_1 = reader.readInt(0, (bankTotals[1] + frackedTotals[1]));
            }

            // if 1s not zero 
            if ((bankTotals[2] + frackedTotals[2]) > 0)
            {
                Console.Out.WriteLine(StringHolder.program_dump_7);//"How many 5s do you want to export?");
                exp_5 = reader.readInt(0, (bankTotals[2] + frackedTotals[2]));
            }

            // if 1s not zero 
            if ((bankTotals[3] + frackedTotals[3] > 0))
            {
                Console.Out.WriteLine(StringHolder.program_dump_8);//"How many 25s do you want to export?");
                exp_25 = reader.readInt(0, (bankTotals[3] + frackedTotals[3]));
            }

            // if 1s not zero 
            if ((bankTotals[4] + frackedTotals[4]) > 0)
            {
                Console.Out.WriteLine(StringHolder.program_dump_9);//"How many 100s do you want to export?");
                exp_100 = reader.readInt(0, (bankTotals[4] + frackedTotals[4]));
            }

            // if 1s not zero 
            if ((bankTotals[5] + frackedTotals[5]) > 0)
            {
                Console.Out.WriteLine(StringHolder.program_dump_10);//"How many 250s do you want to export?");
                exp_250 = reader.readInt(0, (bankTotals[5] + frackedTotals[5]));
            }

            // if 1s not zero 
            // move to export
            Exporter exporter = new Exporter(fileUtils);
            if (file_type == 1)
            {
                Console.Out.WriteLine(StringHolder.program_export_2);//"Tag your jpegs with 'random' to give them a random number.");
            }
            Console.Out.WriteLine(StringHolder.program_export_3 );//"What tag will you add to the file name?");
            String tag = reader.readString();
            //Console.Out.WriteLine(("Exporting to:" + exportFolder));
            if (file_type == 1)
            {
                exporter.writeJPEGFiles(exp_1, exp_5, exp_25, exp_100, exp_250, tag);
                // stringToFile( json, "test.txt");
            }
            else
            {
                exporter.writeJSONFile(exp_1, exp_5, exp_25, exp_100, exp_250, tag);
            }

            // end if type jpge or stack
            Console.Out.WriteLine(StringHolder.program_dump_11);//"Exporting CloudCoins Completed.");
        }// end export One

        public static void fix()
        {
            Console.Out.WriteLine( StringHolder.program_fix_1 );// "Fixing fracked coins can take many minutes.");
            Console.Out.WriteLine( StringHolder.program_fix_2 );//"If your coins are not completely fixed, fix fracked again.");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine( StringHolder.program_fix_3 );//"Attempting to fix all fracked coins.");
            Console.Out.WriteLine("");
            Frack_Fixer fixer = new Frack_Fixer(fileUtils, timeout);
            fixer.fixAll();
            stopwatch.Stop();
            Console.Out.WriteLine( StringHolder.program_fix_4 );//"Fix Time: " + stopwatch.Elapsed + " ms");
            showCoins();
            Console.Out.WriteLine( StringHolder.program_fix_5 );//"If your coins are not completely fixed, you may 'fix fracked' again.");
        }//end fix


    }//End class
}//end namespace
