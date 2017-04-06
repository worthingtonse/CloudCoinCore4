using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Foundation
{
    class CloudCoin
    {
        //  instance variables
        public int nn;// Network Numbers
        public int sn; // Serial Number
        public String[] ans = new String[25];// Authenticity Numbers
        public String[] pans = new String[25];// Proposed Authenticty Numbers
        public enum Status {  fail, pass, error, undetected };
        public Status[] pastStatus = new Status[25];
        public String ed;// Expiration Date expressed as a hex string like 9-2016
        public String edHex;// ed in hex form like like 97e2 Sep 2016. 
        public int hp;// HitPoints (1-25, One point for each server not failed)
        public Dictionary<string, string> aoid = new Dictionary<string, string>();// Account or Owner ID
        public String fileName;
        public String json;
        public byte[] jpeg;
        public const int YEARSTILEXPIRE = 2;
        public enum Folder { Suspect, Counterfeit, Fracked, Bank, Trash };
        public Folder folder;
        public String[] gradeStatus = new String[3];// What passed, what failed, what was undetected

        public CloudCoin(int nn, int sn, String[] ans, String ed, Dictionary<string, string> aoid, String extension)
        {
            //  initialise instance variables
            this.nn = nn;
            this.sn = sn;
            this.ans = ans;
            this.ed = ed;
            this.hp = 25;
            this.aoid = aoid;
            this.fileName = this.getDenomination() + ".CloudCoin." + this.nn + "." + this.sn + ".";
            this.json = "";
            this.jpeg = null;
            for (int i = 0; i < 25; i++)//For every AN
            {
                this.pans[i] = this.generatePan();
                this.pastStatus[i] = Status.undetected;
                if ( aoid.ContainsKey("fracked") )
                {//If there is a fracuted key value
                   // Console.WriteLine("CloudCoin fracked char  " + i  + " is " + aoid["fracked"][i] );
                    switch (aoid["fracked"][i])
                    {
                        case 'p': this.pastStatus[i] = Status.pass; break;
                        case 'f': this.pastStatus[i] = Status.fail; break;
                        case 'u': this.pastStatus[i] = Status.undetected; break;
                        case 'e': this.pastStatus[i] = Status.error; break;
                        default: this.pastStatus[i] = Status.undetected; break;
                    }//end switch on past status
                }//end if has fracked aoid key
               //Console.WriteLine("CloudCoin past status is  " + pastStatus[i]);
            } // end for each pan
              //  String value = aoid["fracked"];
              //  Console.WriteLine("Fracked value in the coin constructor in CLoudCOin  is " + value);
            
        }//end constructor

        public CloudCoin()
        {

        }//end constructor


        public string getPastStatus(int raida_id)
        {
            string returnString = "";
            switch (pastStatus[raida_id])
            {
                case Status.error: returnString = "error"; break;
                case Status.fail: returnString = "fail"; break;
                case Status.pass: returnString = "pass"; break;
                case Status.undetected: returnString = "undetected"; break;
            }//end switch
            return returnString;
        }//end getPastStatus

        public bool setPastStatus(string status, int raida_id)
        {
            bool setGood = false;
            switch (status)
            {
                case "error": pastStatus[raida_id] = Status.error; setGood = true; break;
                 case "fail": pastStatus[raida_id] = Status.fail; setGood = true; break;
                 case "pass": pastStatus[raida_id] = Status.pass; setGood = true; break;
           case "undetected": pastStatus[raida_id] = Status.undetected; setGood = true; break;
            }//end switch
            return setGood;
        }//end set past status

        public string getFolder()
        {
            string returnString = "";
            switch ( folder)
            {
                case Folder.Bank: returnString = "Bank"; break;
                case Folder.Counterfeit: returnString = "Counterfeit"; break;
                case Folder.Fracked: returnString = "Fracked"; break;
                case Folder.Suspect: returnString = "Suspect"; break;
                case Folder.Trash: returnString = "Trash"; break;
            }//end switch
            return returnString;
        }//end getPastStatus

        public bool setFolder(string folderName)
        {
            bool setGood = false;
            switch (folderName.ToLower())
            {
                case "bank": folder = Folder.Bank;  break;
                case "counterfeit": folder = Folder.Counterfeit;  break;
                case "fracked": folder = Folder.Fracked;  break;
                case "suspect": folder = Folder.Suspect;  break;
                case "trash": folder = Folder.Trash;  break;
            }//end switch
            return setGood;
        }//end set past status

        public int getDenomination()
        {
            int nom = 0;
            if ((this.sn < 1))
            {
                nom = 0;
            }
            else if ((this.sn < 2097153))
            {
                nom = 1;
            }
            else if ((this.sn < 4194305))
            {
                nom = 5;
            }
            else if ((this.sn < 6291457))
            {
                nom = 25;
            }
            else if ((this.sn < 14680065))
            {
                nom = 100;
            }
            else if ((this.sn < 16777217))
            {
                nom = 250;
            }
            else
            {
                nom = '0';
            }

            return nom;
        }//end get denomination

        public void calculateHP()
        {
            this.hp = 25;
            for (int i = 0; (i < 25); i++)
            {
                if (this.pastStatus[i]==(int)Status.fail)
                {
                    this.hp--;
                }

            }

        }

        // End calculate hp
        public String gradeCoin()
        {
            int passed = 0;
            int failed = 0;
            int other = 0;
            String passedDesc = "";
            String failedDesc = "";
            String otherDesc = "";
            String internalAoid = "";
            for (int i = 0; (i < 25); i++)
            {
                if (this.pastStatus[i] == Status.pass)
                {
                    passed++;
                    internalAoid += "p";
                    // p means pass
                }
                else if (this.pastStatus[i] == (int)Status.fail)
                {
                    failed++;
                    internalAoid += "f";
                    // f means fail
                }
                else
                {
                    other++;
                    internalAoid += "u";
                    // u means undetected
                }

                // end if pass, fail or unknown
            }

            // for each status
            if (aoid.ContainsKey("fracked"))
            {
                this.aoid.Remove("fracked");
                this.aoid.Add("fracked", internalAoid);
            }
           
            // Calculate passed
            if ((passed == 25))
            {
                passedDesc = "100% Passed!";
            }
            else if ((passed > 17))
            {
                passedDesc = "Super Majority";
            }
            else if ((passed > 13))
            {
                passedDesc = "Majority";
            }
            else if ((passed == 0))
            {
                passedDesc = "None";
            }
            else if ((passed < 5))
            {
                passedDesc = "Super Minority";
            }
            else
            {
                passedDesc = "Minority";
            }

            // Calculate failed
            if ((failed == 25))
            {
                failedDesc = "100% Failed!";
            }
            else if ((failed > 17))
            {
                failedDesc = "Super Majority";
            }
            else if ((failed > 13))
            {
                failedDesc = "Majority";
            }
            else if ((failed == 0))
            {
                failedDesc = "None";
            }
            else if ((failed < 5))
            {
                failedDesc = "Super Minority";
            }
            else
            {
                failedDesc = "Minority";
            }

            // Calcualte Other RAIDA Servers did not help. 
            switch (other)
            {
                case 0:
                    otherDesc = "RAIDA 100% good";
                    break;
                case 1:
                case 2:
                    otherDesc = "Four or less RAIDA errors";
                    break;
                case 3:
                case 4:
                    otherDesc = "Four or less RAIDA errors";
                    break;
                case 5:
                case 6:
                    otherDesc = "Six or less RAIDA errors";
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    otherDesc = "Between 7 and 12 RAIDA errors";
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    otherDesc = "RAIDA total failure";
                    break;
                default:
                    otherDesc = "FAILED TO EVALUATE RAIDA HEALTH";
                    break;
            }
            // end RAIDA other errors and unknowns
            return "\n " + passedDesc + " said Passed. " + "\n " + failedDesc + " said Failed. \n RAIDA Status: " + otherDesc;
        }

        // end grade coin
        public void calcExpirationDate()
        {
            DateTime date = DateTime.Today;
            date.AddYears(YEARSTILEXPIRE);
            this.ed = (date.Month + "-" + date.Year);
            this.edHex = date.Month.ToString("X1");
            this.edHex += date.Year.ToString("X3");
        }

        // end calc exp date
        public String generatePan()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                Guid pan = new Guid(bytes);
                String rawpan = pan.ToString("N");
                String fullPan = "";
                switch (rawpan.Length)//Make sure the pan is 32 characters long. The odds of this happening are slim but it will happen.
                {
                    case 27: fullPan = ("00000" + rawpan); break;
                    case 28: fullPan = ("0000" + rawpan); break;
                    case 29: fullPan = ("000" + rawpan); break;
                    case 30: fullPan = ("00" + rawpan); break;
                    case 31: fullPan = ("0" + rawpan); break;
                    case 32: fullPan = rawpan; break;
                    case 33: fullPan = rawpan.Substring(0, rawpan.Length - 1); break;//trim one off end
                    case 34: fullPan = rawpan.Substring(0, rawpan.Length - 2); break;//trim one off end
                }

                return fullPan;
            }
        }

        public String[] grade()
        {
            int passed = 0;
            int failed = 0;
            int other = 0;
            String passedDesc = "";
            String failedDesc = "";
            String otherDesc = "";
            String fracked = "";
            for (int i = 0; (i < 25); i++)
            {
                if (this.pastStatus[i] == Status.pass)
                {
                    passed++;
                    fracked += 'p';
                }
                else if (this.pastStatus[i] == Status.fail)
                {
                    failed++;
                    fracked += 'f';
                }
                else
                {
                    other++;
                    fracked += 'u';
                }// end if pass, fail or unknown
            }
            if( aoid.ContainsKey("fracked"))
            { aoid.Remove("fracked");
            }
            aoid.Add("fracked", fracked);

            // for each status
            // Calculate passed
            if ((passed == 25))
            {
                passedDesc = "100% Passed!";
            }
            else if ((passed > 17))
            {
                passedDesc = "Super Majority";
            }
            else if ((passed > 13))
            {
                passedDesc = "Majority";
            }
            else if ((passed == 0))
            {
                passedDesc = "None";
            }
            else if ((passed < 5))
            {
                passedDesc = "Super Minority";
            }
            else
            {
                passedDesc = "Minority";
            }

            // Calculate failed
            if ((failed == 25))
            {
                failedDesc = "100% Failed!";
            }
            else if ((failed > 17))
            {
                failedDesc = "Super Majority";
            }
            else if ((failed > 13))
            {
                failedDesc = "Majority";
            }
            else if ((failed == 0))
            {
                failedDesc = "None";
            }
            else if ((failed < 5))
            {
                failedDesc = "Super Minority";
            }
            else
            {
                failedDesc = "Minority";
            }

            // Calcualte Other RAIDA Servers did not help. 
            switch (other)
            {
                case 0:
                    otherDesc = "100% of RAIDA responded";
                    break;
                case 1:
                case 2:
                    otherDesc = "Four or less RAIDA errors";
                    break;
                case 3:
                case 4:
                    otherDesc = "Four or less RAIDA errors";
                    break;
                case 5:
                case 6:
                    otherDesc = "Six or less RAIDA errors";
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    otherDesc = "Between 7 and 12 RAIDA errors";
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    otherDesc = "RAIDA total failure";
                    break;
                default:
                    otherDesc = "FAILED TO EVALUATE RAIDA HEALTH";
                    break;
            }
            // end RAIDA other errors and unknowns
            // Coin will go to bank, counterfeit or fracked
            if ( other > 12 )
            {
                // not enough RAIDA to have a quorum
                folder = Folder.Suspect;
            }
            else if ( failed > passed )
            {
                // failed out numbers passed with a quorum: Counterfeit
                folder = Folder.Counterfeit;
            }
            else if ( failed > 0 )
            {
                // The quorum majority said the coin passed but some disagreed: fracked. 
                folder = Folder.Fracked;
            }
            else
            {
                // No fails, all passes: bank
                folder = Folder.Bank;
            }

            this.gradeStatus[0] = passedDesc;
            this.gradeStatus[1] = failedDesc;
            this.gradeStatus[2] = otherDesc;
            return this.gradeStatus;
        }

        // end gradeStatus


        public void setAnsToPans()
        {
            for (int i = 0; (i < 25); i++)
            {
                this.pans[i] = this.ans[i];
            }// end for 25 ans
        }

        // end setAnsToPans
        public void setAnsToPansIfPassed()
        {
            // now set all ans that passed to the new pans
            for (int i = 0; (i < 25); i++)
            {
                if (pastStatus[i] == Status.pass)//1 means pass
                {
                    ans[i] = pans[i];
                }
                else if (pastStatus[i] == Status.undetected && !RAIDA_Status.failsEcho[i] )//Timed out but there server echoed. So it probably changed the PAN just too slow of a response
                {
                    ans[i] = pans[i];
                }
                else
                {
                    // Just keep the ans and do not change. Hopefully they are not fracked. 
                }

            }// for each guid in coin
        }// end set ans to pans if passed


        public void consoleReport()
        {
            // Used only for console apps
            //  System.out.println("Finished detecting coin index " + j);
            // PRINT OUT ALL COIN'S RAIDA STATUS AND SET AN TO NEW PAN


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.Out.WriteLine( StringHolder.cloudcoin_report + string.Format("{0,8}", this.sn) + StringHolder.cloudcoin_denomination + string.Format("{0,3}", this.getDenomination()) + " ║");
            Console.Out.WriteLine("╠══════════╦══════════╦══════════╦══════════╦══════════╣");
            Console.Out.Write("║    "); a(pastStatus[0]);  Console.Out.Write("     ║    "); a(pastStatus[1]);   Console.Out.Write("     ║    "); a(pastStatus[2]);  Console.Out.Write("     ║    "); a(pastStatus[3]);  Console.Out.Write("     ║    "); a(pastStatus[4]);  Console.Out.WriteLine("     ║");
            Console.Out.WriteLine("╠══════════╬══════════╬══════════╬══════════╬══════════╣");
            Console.Out.Write("║    "); a(pastStatus[5]); Console.Out.Write("     ║    "); a(pastStatus[6]);    Console.Out.Write("     ║    "); a(pastStatus[7]);  Console.Out.Write("     ║    "); a(pastStatus[8]);  Console.Out.Write("     ║    "); a(pastStatus[9]);  Console.Out.WriteLine("     ║");
            Console.Out.WriteLine("╠══════════╬══════════╬══════════╬══════════╬══════════╣");
            Console.Out.Write("║    "); a(pastStatus[10]); Console.Out.Write("     ║    "); a(pastStatus[11]);  Console.Out.Write("     ║    "); a(pastStatus[12]); Console.Out.Write("     ║    "); a(pastStatus[13]); Console.Out.Write("     ║    "); a(pastStatus[14]); Console.Out.WriteLine("     ║");
            Console.Out.WriteLine("╠══════════╬══════════╬══════════╬══════════╬══════════╣");
            Console.Out.Write("║    "); a(pastStatus[15]); Console.Out.Write("     ║    "); a(pastStatus[16]);  Console.Out.Write("     ║    "); a(pastStatus[17]); Console.Out.Write("     ║    "); a(pastStatus[18]); Console.Out.Write("     ║    "); a(pastStatus[19]); Console.Out.WriteLine("     ║");
            Console.Out.WriteLine("╠══════════╬══════════╬══════════╬══════════╬══════════╣");
            Console.Out.Write("║    "); a(pastStatus[20]); Console.Out.Write("     ║    "); a(pastStatus[21]);  Console.Out.Write("     ║    "); a(pastStatus[22]); Console.Out.Write("     ║    "); a(pastStatus[23]); Console.Out.Write("     ║    "); a(pastStatus[24]); Console.Out.WriteLine("     ║");
            Console.Out.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

                // check if failed
              //  string fmt = "00";
               // string fi = i.ToString(fmt); // Pad numbers with two digits
            //    Console.Out.WriteLine("RAIDA" + i + " " + pastStatus[i] + " | ");
               // Console.Out.WriteLine("AN " + i + ans[i]);
               // Console.Out.WriteLine("PAN " + i + pans[i]);
          //  }

            // End for each cloud coin GUID statu
          //  Console.Out.WriteLine("ed " + ed);
          //  Console.Out.WriteLine("edHex " + edHex);
          //  Console.Out.WriteLine("edhp " + hp);
           // Console.Out.WriteLine("fileName " + fileName);
           // Console.Out.WriteLine("YEARSTILEXPIRE " + YEARSTILEXPIRE);
         //   Console.Out.WriteLine("extension " + extension);


        }//Console Report

        public void a( Status status) {
            if ( status == Status.pass ) {
                Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Green; Console.Out.Write("♥"); Console.ForegroundColor = ConsoleColor.White;
            } else if (status == Status.fail)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Out.Write("█"); Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("U"); Console.ForegroundColor = ConsoleColor.White;
            }
        }//end a Report helper
    }
}
