using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
//using Newtonsoft.Json;

namespace Foundation
{
    class FileUtils
    {
        /* INSTANCE VARIABLES */ 
        public String rootFolder;
        public String importFolder;
        public String importedFolder;
        public String trashFolder;
        public String suspectFolder;
        public String frackedFolder;
        public String bankFolder;
        public String templateFolder;
        public String counterfeitFolder;
        public String directoryFolder;
        public String exportFolder;

        /* CONSTRUCTOR */
        public FileUtils(String rootFolder, String importFolder, String importedFolder, String trashFolder, String suspectFolder, String frackedFolder, String bankFolder, String templateFolder, String counterfeitFolder, String directoryFolder, String exportFolder)
        {
            //  initialise instance variables
            this.rootFolder = rootFolder;
            this.importFolder = importFolder;
            this.importedFolder = importedFolder;
            this.trashFolder = trashFolder;
            this.suspectFolder = suspectFolder;
            this.frackedFolder = frackedFolder;
            this.bankFolder = bankFolder;
            this.templateFolder = templateFolder;
            this.counterfeitFolder = counterfeitFolder;
            this.directoryFolder = directoryFolder;
            this.exportFolder = exportFolder;
        }  // End constructor

        /* PUBLIC METHODS */
        /* This loads a JSON file (.stack) from the hard drive that contains only one CloudCoin and turns it into an object.  */
  
        /*  This has a bug that does not parse json unless it is in a specific order.    */
        public CloudCoin loadOneCloudCoinFromJsonFile(String loadFilePath)
                {
                    //Load file as JSON
                    String incomeJson = this.importJSON(loadFilePath);

                    //STRIP UNESSARY test
                    int secondCurlyBracket = ordinalIndexOf(incomeJson, "{", 2) - 1;
                    int firstCloseCurlyBracket = ordinalIndexOf(incomeJson, "}", 0);
                    // incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket);
                    incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket - secondCurlyBracket + 1);
                    // Console.Out.WriteLine(incomeJson);

                    String[] jsonArray = incomeJson.Split('"');

                    int nn = Convert.ToInt32(jsonArray[3]);
                    int sn = Convert.ToInt32(jsonArray[7]);
                    String[] ans = new String[25];
                    int count = 0;
                    for (int i = 11; i < 60; i = i + 2)
                    {
                        ans[count] = jsonArray[i];
                        count++;
                    }

                    String ed = jsonArray[63];
                    Dictionary<string, string> aoid_dictionary = new Dictionary<string, string>();
                    aoid_dictionary.Add("memo", "");
                    String frackedCodes ="";
                    // Console.Out.WriteLine(jsonArray.Length);
                    if (incomeJson.Contains("fracked"))//If there is an fracked note that explains the status "fracked=ppppppppfppppppppppppppp"
                    {
                        if (incomeJson.Contains("fracked\""))
                        {
                            frackedCodes = incomeJson.Substring(incomeJson.IndexOf("fracked") + 10, 25);//fixes bug where we used impropver json, can be deleted later
                        }
                        else {
                            frackedCodes = incomeJson.Substring(incomeJson.IndexOf("fracked") + 8, 25);
                        }

                       // Console.WriteLine(frackedCodes);
                        aoid_dictionary.Add("fracked", frackedCodes);
                    }

                    CloudCoin returnCC = new CloudCoin(nn, sn, ans, ed, aoid_dictionary, "suspect");

                    return returnCC;
                }//end load one CloudCoin from JSON
        

        /*
         This loads a JSON file (.stack) from the hard drive that contains only one CloudCoin and turns it into an object. 
           This uses Newton soft but causes a enrror System.IO.FileNotFoundException. Could not load file 'Newtonsoft.Json'  
        public CloudCoin loadOneCloudCoinFromJsonFile(String loadFilePath)
        {

            //Load file as JSON
            String incomeJson = this.importJSON(loadFilePath);
            //STRIP UNESSARY test
            int secondCurlyBracket = ordinalIndexOf(incomeJson, "{", 2) - 1;
            int firstCloseCurlyBracket = ordinalIndexOf(incomeJson, "}", 0) - secondCurlyBracket;
            // incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket);
            incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket + 1);
            // Console.Out.WriteLine(incomeJson);
            //Deserial JSON
            SimpleCoin cc = JsonConvert.DeserializeObject<SimpleCoin>(incomeJson);
            //Make Coin
            String[] strAoid = cc.aoid.ToArray();
            Dictionary<string, string> aoid_dictionary = new Dictionary<string, string>();
            for (int j = 0; j < strAoid.Length; j++)
            { //"fracked=ppppppppppppppppppppppppp"
                if (strAoid[j].Contains("="))
                {//see if the string contains an equals sign
                    String[] keyvalue = strAoid[j].Split('=');
                    aoid_dictionary.Add(keyvalue[0], keyvalue[1]);//index 0 is the key index 1 is the value.
                }
                else
                { //There is something there but not a key value pair. Treak it like a memo
                    aoid_dictionary.Add("memo", strAoid[j]);
                }//end if cointains an = 
            }//end for each aoid


            CloudCoin returnCC = new CloudCoin(cc.nn, cc.sn, cc.an.ToArray(), cc.ed, aoid_dictionary, "suspect");
            for (int i = 0; (i < 25); i++)
            {//All newly created loaded coins get new PANs. 
                returnCC.pans[i] = returnCC.generatePan();
                //returnCC.pastStatus[i] = "undetected";
            } // end for each pan
              //Return Coin

            returnCC.fileName = (returnCC.getDenomination() + (".CloudCoin." + (returnCC.nn + ("." + (returnCC.sn + ".")))));
            returnCC.json = "";
            returnCC.jpeg = null;

            return returnCC;
        }//end load one CloudCoin from JSON

            */


        public CloudCoin[] loadManyCloudCoinFromJsonFile(String loadFilePath, string incomeJson)
        {
            CloudCoin[] returnCoins;
            
           
            //Remove "{ "cloudcoin": ["
            int secondCurlyBracket = ordinalIndexOf(incomeJson, "{", 2) - 1;
            incomeJson = incomeJson.Substring(secondCurlyBracket);
            //remove the last instance of "]}"
            incomeJson = incomeJson.Remove(incomeJson.LastIndexOf("}"), 1);
            incomeJson = incomeJson.Remove(incomeJson.LastIndexOf("]"), 1);
            // Console.Out.WriteLine(incomeJson);

            String[] pieces = incomeJson.Split('}');//split json file into coins
            returnCoins = new CloudCoin[pieces.Length - 1];//last piece is not a coin

            for (int j = 0; j < pieces.Length - 1; j++)//for each cloudcoin segment in the chest or stack file. -1 allows for last runt segment 
            {
                String currentCoin = pieces[j];
                String[] jsonArray = currentCoin.Split('"');

                //  for (int i = 0; i < jsonArray.Length; i++ )
                //  {
                //      Console.Out.WriteLine( i  + " = " + jsonArray[i]);
                //  }

                int nn = Convert.ToInt32(jsonArray[3]);
                int sn = Convert.ToInt32(jsonArray[7]);
                String[] ans = new String[25];
                int count = 0;
                for (int i = 11; i < 60; i = i + 2)
                {
                    ans[count] = jsonArray[i];
                    count++;
                }

                String ed = jsonArray[63];
                String aoid = "";
                // Console.Out.WriteLine(jsonArray.Length);


                if (jsonArray.Length > 67)
                {//If there is an aoid
                    aoid = jsonArray[67];
                }


                Dictionary<string, string> aoid_dictionary = new Dictionary<string, string>();

                if (aoid.Contains("="))
                {//see if the string contains an equals sign
                    String[] keyvalue = aoid.Split('=');
                    aoid_dictionary.Add(keyvalue[0], keyvalue[1]);//index 0 is the key index 1 is the value.
                }
                else
                { //There is something there but not a key value pair. Treak it like a memo
                    aoid_dictionary.Add("memo", aoid);
                }//end if cointains an = 


                CloudCoin returnCC = new CloudCoin(nn, sn, ans, ed, aoid_dictionary, "suspect");
                for (int i = 0; (i < 25); i++)
                {//All newly created loaded coins get new PANs. 
                    returnCC.pans[i] = returnCC.generatePan();
                    returnCC.setPastStatus("undetected", i);
                } // end for each pan
                  //Return Coin

                returnCC.fileName = (returnCC.getDenomination() + (".CloudCoin." + (returnCC.nn + ("." + (returnCC.sn + ".")))));
                returnCC.json = "";
                returnCC.jpeg = null;
                returnCoins[j] = returnCC;
            }//end for each cloudcoin in the json file

            return returnCoins;
        }//end load one CloudCoin from JSON


        public CloudCoin loadOneCloudCoinFromJPEGFile(String loadFilePath)
        { 
            /* GET the first 455 bytes of he jpeg where the coin is located */
            String wholeString = "";
            byte[] jpegHeader = new byte[455];
            Console.Out.WriteLine("Load file path " + loadFilePath);
            FileStream fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read);
            try
            {
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(jpegHeader, sum, 455 - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            wholeString = bytesToHexString(jpegHeader);
            CloudCoin returnCC = this.parseJpeg(wholeString);
            Console.Out.WriteLine("From FileUtils returnCC.fileName " + returnCC.fileName);
            return returnCC;
        }//end load one CloudCoin from JSON

        public String importJSON(String jsonfile)
        {
            String jsonData = "";
            String line;

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.

                using (StreamReader sr = new StreamReader(jsonfile, Encoding.Default, false, 2000))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }//End if line is null
                        jsonData = (jsonData + line + "\n");
                    }//end while true
                }//end using
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file " + jsonfile + " could not be read:");
                Console.WriteLine(e.Message);
            }
            return jsonData;
        }//end importJSON

        // en d json test
        public String setJSON(CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "\t";
            String json = (tab + tab + "{ " + Environment.NewLine);// {
            json += tab + tab + quote + "nn" + quote + ":" + quote + cc.nn + quote + ", " + Environment.NewLine;// "nn":"1",
            json += tab + tab + quote + "sn" + quote + ":" + quote + cc.sn + quote + ", " + Environment.NewLine;// "sn":"367544",
            json += tab + tab + quote + "an" + quote + ": [" + quote;// "an": ["
            for (int i = 0; (i < 25); i++)
            {
                json += cc.ans[i];// 8551995a45457754aaaa44
                if (i == 4 || i == 9 || i == 14 || i == 19)
                {
                    json += quote + "," + Environment.NewLine + tab + tab + tab + quote; //", 
                }
                else if (i == 24)
                {
                    // json += "\""; last one do nothing
                }
                else
                { // end if is line break
                    json += quote + ", " + quote;
                }

                // end else
            }// end for 25 ans

            json += quote + "]," + Environment.NewLine;//"],
            // End of ans
            json += tab + tab + quote + "ed" + quote + ":" + quote + cc.ed + quote + "," + Environment.NewLine; // "ed":"9-2016",
            json += tab + tab + quote + "aoid" + quote + ": [";

            int count = 0;
            if (cc.aoid != null)
            {
                foreach (KeyValuePair<string, string> entry in cc.aoid)
                {
                    if ((count != 0))
                    {
                        json += ",";
                    }

                    json += quote + entry.Key + "=" + entry.Value + quote;
                    count++;
                }//end for each
            }//end if null

            json += Environment.NewLine + tab + tab + "]" + Environment.NewLine;
            json += "}";
            // Keep expiration date when saving (not a truley accurate but good enought )
            return json;
        }
        // end get JSON

        /* Writes a JPEG To the Export Folder */
        public bool writeJpeg(CloudCoin cc, string tag)
        {
           // Console.Out.WriteLine("Writing jpeg " + cc.sn);
  
            bool fileSavedSuccessfully = true;

            /* BUILD THE CLOUDCOIN STRING */
            String cloudCoinStr = "01C34A46494600010101006000601D05"; //THUMBNAIL HEADER BYTES
            for (int i = 0; (i < 25); i++)
            {
                cloudCoinStr = cloudCoinStr + cc.ans[i];
            } // end for each an

            cloudCoinStr += "204f42455920474f4420262044454645415420545952414e545320";// Hex for " OBEY GOD & DEFEAT TYRANTS "
            cloudCoinStr += "00"; // HC: Has comments. 00 = No
            cloudCoinStr += "00"; // LHC = 100%
            cc.calcExpirationDate();
            cloudCoinStr += cc.edHex; // 0x97E2;//Expiration date Sep. 2018
            cloudCoinStr += "01";//  cc.nn;//network number
            String hexSN = cc.sn.ToString("X6");
            String fullHexSN = "";
            switch (hexSN.Length)
            {
                case 1:fullHexSN = ("00000" + hexSN);   break;
                case 2:fullHexSN = ("0000" + hexSN);    break;
                case 3: fullHexSN = ("000" + hexSN);    break;
                case 4:fullHexSN = ("00" + hexSN);      break;
                case 5: fullHexSN = ("0" + hexSN);   break;
                case 6:fullHexSN = hexSN;     break;
            }
            cloudCoinStr = (cloudCoinStr + fullHexSN);   
            /* BYTES THAT WILL GO FROM 04 to 454 (Inclusive)*/
            byte[] ccArray = this.hexStringToByteArray(cloudCoinStr);
            

            /* READ JPEG TEMPLATE*/
            byte[] jpegBytes = null;
            switch (cc.getDenomination())
            {
                case   1: jpegBytes = readAllBytes(this.templateFolder + "jpeg1.jpg");   break;
                case   5: jpegBytes = readAllBytes(this.templateFolder + "jpeg5.jpg");   break;
                case  25: jpegBytes = readAllBytes(this.templateFolder + "jpeg25.jpg");  break;
                case 100: jpegBytes = readAllBytes(this.templateFolder + "jpeg100.jpg"); break;
                case 250: jpegBytes = readAllBytes(this.templateFolder + "jpeg250.jpg"); break;
            }// end switch


            /* WRITE THE SERIAL NUMBER ON THE JPEG */
 
            Bitmap bitmapimage;
            using (var ms = new MemoryStream(jpegBytes))
            {
                bitmapimage = new Bitmap(ms);
               // bitmapimage.Save(fileName2, ImageFormat.Jpeg);
            }
            Graphics graphics = Graphics.FromImage(bitmapimage);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            PointF drawPointAddress = new PointF(10.0F, 10.0F);
            graphics.DrawString(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", new Font("Arial", 20), Brushes.White, drawPointAddress);

            ImageConverter converter = new ImageConverter();
            byte[] snBytes = (byte[])converter.ConvertTo(bitmapimage, typeof(byte[]));

            List<byte> b1 = new List<byte>(snBytes);
            List<byte> b2 = new List<byte>(ccArray);
            b1.InsertRange( 4, b2);

            if (tag == "random") {
                Random r = new Random();
                int rInt = r.Next(100000, 1000000); //for ints
                tag = rInt.ToString();
            }

            string fileName = exportFolder + cc.fileName + tag + ".jpg";
            File.WriteAllBytes(fileName, b1.ToArray());
            Console.Out.WriteLine("Writing to " + fileName);
            return fileSavedSuccessfully;
        }//end write JPEG

        /* OPEN FILE AND READ ALL CONTENTS AS BYTE ARRAY */
        public byte[] readAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                int fileLength = Convert.ToInt32(fs.Length);
                fs.Read(buffer, 0, fileLength);
            }
            return buffer;
        }//end read all bytes

        public bool writeTo(String folder, CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "\t";
            String wholeJson = "{" + Environment.NewLine; //{
            bool alreadyExists = true;
            String json = this.setJSON(cc);
            if (!File.Exists(folder + cc.fileName + ".stack"))
            {
                wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
                wholeJson += json;
                wholeJson += Environment.NewLine + tab + "]" + Environment.NewLine +"}";
                File.WriteAllText(folder + cc.fileName + ".stack", wholeJson);
            }
            else
            {
                if (folder.Contains("Counterfeit") || folder.Contains("Trash")) {
                    //Let the program delete it
                    alreadyExists = false;
                    return alreadyExists;
                }
                else if (folder.Contains("Imported"))
                {
                    File.Delete(folder + cc.fileName + ".stack");
                    File.WriteAllText(folder + cc.fileName + ".stack", wholeJson);
                    alreadyExists = false;
                    return alreadyExists;
                }
                else
                {
                    Console.WriteLine(cc.fileName + ".stack" + " already exists in the folder " + folder);
                    return alreadyExists;

                }//end else
               
            }//File Exists
            File.WriteAllText(folder + cc.fileName + ".stack", wholeJson);
            alreadyExists = false;
            return alreadyExists;

        }//End Write To

        public void overWrite(String folder, CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "\t";
            String wholeJson = "{" + Environment.NewLine; //{
            String json = this.setJSON(cc);
            
                wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
                wholeJson += json;
            wholeJson += Environment.NewLine + tab + "]" + Environment.NewLine + "}";

            File.WriteAllText(folder + cc.fileName + ".stack", wholeJson);

        }//End Overwrite

        private CloudCoin parseJpeg(String wholeString)
        {

            CloudCoin cc = new CloudCoin();
            int startAn = 40;
            for (int i = 0; i < 25; i++)
            {

                cc.ans[i] = wholeString.Substring(startAn, 32);
                // Console.Out.WriteLine(i +": " + cc.ans[i]);
                startAn += 32;
            }

            // end for
            cc.aoid = null;
            // wholeString.substring( 840, 895 );
            cc.hp = 25;
            // Integer.parseInt(wholeString.substring( 896, 896 ), 16);
            cc.ed = wholeString.Substring(898, 4);
            cc.nn = Convert.ToInt32(wholeString.Substring(902, 2), 16);
            cc.sn = Convert.ToInt32(wholeString.Substring(904, 6), 16);
            for (int i = 0; (i < 25); i++)
            {
                cc.pans[i] = cc.generatePan();
                cc.setPastStatus("undetected", i);
            }
            cc.fileName = cc.getDenomination() + ".CloudCoin."+ cc.nn + "." + cc.sn + ".";
          //  Console.Out.WriteLine("parseJpeg cc.fileName " + cc.fileName);
            // end for each pan
            return cc;
        }// end parse Jpeg

        // en d json test
        public byte[] hexStringToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }//End hex string to byte array

        public int ordinalIndexOf(String str, String substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
                pos = str.IndexOf(substr, pos + 1);
            return pos;
        }

        public string bytesToHexString(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int length = data.Length;
            char[] hex = new char[length * 2];
            int num1 = 0;
            for (int index = 0; index < length * 2; index += 2)
            {
                byte num2 = data[num1++];
                hex[index] = GetHexValue(num2 / 0x10);
                hex[index + 1] = GetHexValue(num2 % 0x10);
            }
            return new string(hex);
        }//End NewConverted

        private char GetHexValue(int i)
        {
            if (i < 10)
            {
                return (char)(i + 0x30);
            }
            return (char)((i - 10) + 0x41);
        }//end GetHexValue


    }//Enc Class File Utils
}//End Namespace
