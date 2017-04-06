using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    public static class StringHolder
    {
        public static string program_command_1 = "echo raida";
        public static string program_command_2 = "show coins";
        public static string program_command_3 = "import";
        public static string program_command_4 = "export";
        public static string program_command_5 = "fix fracked";
        public static string program_command_6 = "show folders";
        public static string program_command_7 = "export for sales";
        public static string program_command_8 = "quit";
        public static string program_run_1 = "Commands Available:";
        public static string program_run_2 = "Command failed. Try again.";

        public static string program_start_1 = "║                  CloudCoin Foundation 2 v.4.04.17                ║";
        public static string program_start_2 = "║          Used to Authenticate, Store and Payout CloudCoins       ║";
        public static string program_start_3 = "║      This Software is provided as is with all faults, defects    ║";
        public static string program_start_4 = "║          and errors, and without warranty of any kind.           ║";
        public static string program_start_5 = "║                Free from the CloudCoin Consortium.               ║";
        public static string program_start_6 = "Checking RAIDA...";
        public static string program_echo_1 = "RAIDA Health: "; 
        public static string program_echo_2 = " out of 25";
        public static string program_echo_3 = "Not enough RAIDA servers can be contacted to import new coins.";
        public static string program_echo_4 = "Is your device connected to the Internet?";
        public static string program_echo_5 = "Is a router blocking your connection?";
        public static string program_echo_6 = "The RAIDA is ready for counterfeit detection.";
        public static string raida0 = "Australia";
        public static string raida1 = "Macedonia";
        public static string raida2 = "Philippines";
        public static string raida3 = "Serbia";
        public static string raida4 = "Bulgaria";
        public static string raida5 = "Russia";
        public static string raida6 = "Switzerland";
        public static string raida7 = "United Kingdom";
        public static string raida8 = "Punjab";
        public static string raida9 = "India";
        public static string raida10 = "Texas";
        public static string raida11 = "USA";
        public static string raida12 = "Romania";
        public static string raida13 = "Taiwan";
        public static string raida14 = "Moscow";
        public static string raida15 = "St. Petersburge";
        public static string raida16 = "Columbia";
        public static string raida17 = "Singapore";
        public static string raida18 = "Germany";
        public static string raida19 = "Canada";
        public static string raida20 = "Venezuela";
        public static string raida21 = "Hyperbad";
        public static string raida22 = "USA";
        public static string raida23 = "Ukraine";
        public static string raida24 = "Luxenburg";
        public static string program_showcoins_total = "║  Total Coins in Bank:    ";
        public static string program_showcoins_perfect = "║ Perfect: ║ ";
        public static string program_showcoins_fracked = "║ Fracked: ║ ";
        public static string program_showFolders_1 = "Your Root folder is:";
        public static string program_showFolders_2 = "Your Import folder is:";
        public static string program_showFolders_3 = "Your Imported folder is:";
        public static string program_showFolders_4 = "Your Suspect folder is:";
        public static string program_showFolders_5 = "Your Trash folder is:";
        public static string program_showFolders_6 = "Your Bank folder is:";
        public static string program_showFolders_7 = "Your Fracked folder is:";
        public static string program_showFolders_8 = "Your Templates folder is:";
        public static string program_showFolders_9 = "Your Directory folder is:";
        public static string program_showFolders_10 = "Your Counterfeits folder is:";
        public static string program_showFolders_11 = "Your Export folder is:";
        public static string program_import_1 = "Finishing importing coins from last time...";
        public static string program_import_2 = "Now looking in import folder for new coins...";
        public static string program_import_3 = "Loading all CloudCoins in your Import folder: ";
        public static string program_import_4 = "No coins in import folder.";

        public static string program_detect_1 = "Detecting Authentication of Suspect Coins";
        public static string program_detect_2 = "Total imported to bank: ";
        public static string program_detect_3 = "Total imported to fracked: ";
        public static string program_detect_4 = "Total Counterfeit: ";
        public static string program_detect_5 = "Total Kept in suspect folder: ";

        public static string program_dump_1 = "Export for sales will export stack files one note in them.";
        public static string program_dump_2 = "Each file will recieve a random tag.";
        public static string program_dump_3 = "This function helps you make CloudCoins for automated sales points.";
        public static string program_dump_4 = "Continue to dump? 1.Yes or 2. No?";
        //Note this code is used in export and export for sales
        public static string program_dump_5 = "Your Bank Inventory:";
        public static string program_dump_6 = "How many 1s do you want to export?";
        public static string program_dump_7 = "How many 5s do you want to export?";
        public static string program_dump_8 = "How many 25s do you want to export?";
        public static string program_dump_9 = "How many 100s do you want to export?";
        public static string program_dump_10 = "How many 250s do you want to export?";
        public static string program_dump_11 = "Export complete. Check your export folder";

        public static string program_export_1 = "Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?";
        public static string program_export_2 = "Tag your jpegs with 'random' to give them a random number.";
        public static string program_export_3 = "What tag will you add to the file name?";

        public static string program_fix_1 = "Fixing fracked coins can take many minutes.";
        public static string program_fix_2 = "If your coins are not completely fixed, fix fracked again.";
        public static string program_fix_3 = "Attempting to fix all fracked coins.";
        public static string program_fix_4 = "Fix Time: ";
        public static string program_fix_5 = "If your coins are not completely fixed, you may fix fracked again.";


        public static string cloudcoin_report = "║  Authenticity Report SN #";
        public static string cloudcoin_denomination = ", Denomination: ";

        public static string detector_3 = "You tried to import a coin that has already been imported.";
        public static string detector_4 = "Suspect CloudCoin was moved to Trash folder.";
        public static string detector_5 = "Now scanning coin ";
        public static string detector_6 = " for counterfeit. SN ";
        public static string detector_7 = "Not enough RAIDA were contacted to determine if the coin is authentic.";
        public static string detector_8 = "Try again later.";

        public static string frackfixer_1 = "RAIDA Fails Echo or Fix. Try again when RAIDA online.";
        public static string frackfixer_2 = " unfracked successfully.";
        public static string frackfixer_3 = "RAIDA failed to accept tickets on corner ";
        public static string frackfixer_4 = "Trusted servers failed to provide tickets for corner ";
        public static string frackfixer_5 = "One or more of the trusted triad will not echo and detect.So not trying.";
        public static string frackfixer_6 = "You have no fracked coins.";
        public static string frackfixer_unfracking = "UnFracking coin ";
        public static string frackfixer_8 = "CloudCoin was moved to Bank.";
        public static string frackfixer_9 = "CloudCoin was moved to Trash.";
        public static string frackfixer_10 = "CloudCoin was moved back to Fraked folder.";
        public static string frackfixer_11 = "Attempting to fix RAIDA ";
        public static string frackfixer_12 = " Using corner ";
        public static string frackfixer_13 = "Time spent fixing RAIDA in milliseconds: ";

        public static string importer_1 = " File moved to trash: ";
        public static string importer_2 = " File not found: ";
        public static string importer_3 = " IO Exception: ";
        public static string importer_importstack1 = "The following file does not appear to be valid JSON. It will be moved to the Trash Folder: ";
        public static string importer_importstack2 = "Paste the text into http://jsonlint.com/ to check for validity.";

        public static string importer_seemsValidJSON_1 = "The stack file did not have a matching number of { }. There were";
        public static string importer_seemsValidJSON_2 = "The stack file did not have a matching number of []. There were ";
        public static string importer_seemsValidJSON_3 = "The stack file did not have a matching number of double quotations";
        public static string importer_seemsValidJSON_4 = "The stack file did not have a the right number of full colons :";

        public static string keyboardreader_1 = " IO Exception: ";
        public static string keyboardreader_2 = "Please enter one of the following: ";
        public static string keyboardreader_3 = "Input is not an integer. ";
        public static string keyboardreader_4 = "Please enter an integer between ";
        public static string keyboardreader_5 = "Input is not an integer. Please enter an integer between ";
        public static string keyboardreader_6 = "Please enter one of the following: ";
        public static string keyboardreader_7 = "Fatal error. Exiting program.";



    }
}
