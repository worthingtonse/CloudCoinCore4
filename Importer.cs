using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Foundation
{
    class Importer
    {
        /* INSTANCE VARAIBLES */ 
        FileUtils fileUtils;
        
        /* CONSTRUCTOR */
        public Importer(FileUtils fileUtils)
        {
            this.fileUtils = fileUtils;
        }//Constructor

        /* PUBLIC METHODS */
        public bool importAll()
        {
            var ext = new List<string> { ".jpg", ".stack", ".jpeg" };
            var fnamesRaw = Directory.GetFiles(this.fileUtils.importFolder, "*.*", SearchOption.TopDirectoryOnly).Where(s => ext.Contains(Path.GetExtension(s)));
            string[] fnames = new string[fnamesRaw.Count()];
            for (int i = 0; i < fnamesRaw.Count(); i++){
                fnames[i] = Path.GetFileName( fnamesRaw.ElementAt(i) );
            };

            //String[] fnames = new DirectoryInfo(this.fileUtils.importFolder).GetFiles().Select(o => o.Name).ToArray();//Get a list of all in the folder except the directory "imported"

            if (fnames.Length == 0)//   Console.Out.WriteLine("There were no CloudCoins to import. Please place our CloudCoin .jpg and .stack files in your imports" + " folder at " + this.fileUtils.importFolder );
            {
                return false;
            }
            else
            {
              //  Console.ForegroundColor = ConsoleColor.Green;
              //  Console.Out.WriteLine("Importing the following files: ");
              //  Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < fnames.Length; i++)// Loop through each file. 
                { 
                    Console.Out.WriteLine(fnames[i]);
                    this.importOneFile(fnames[i]);
                } // end for each file name
                return true;
            }//end if no files 
        }// End Import All


        /* PRIVATE METHODS */

        /* IMPORT ONE FILE. COULD BE A JPEG, STACK or CHEST */
        private bool importOneFile(String fname)
        {
            String extension = "";
            int indx = fname.LastIndexOf('.');//Get file extension
            if (indx > 0)
            {
                extension = fname.Substring(indx + 1);
            }

            extension = extension.ToLower();
            if (extension == "jpeg" || extension == "jpg")//Run if file is a jpeg
            {
                if (!this.importJPEG(fname))
                {
                    if (!File.Exists(this.fileUtils.trashFolder + fname))
                    {
                        File.Move(this.fileUtils.importFolder + fname, this.fileUtils.trashFolder + fname);
                        Console.Out.WriteLine( StringHolder.importer_1 + fname );// "File moved to trash: " + fname);
                    }
                    else
                    {
                        File.Delete(this.fileUtils.importedFolder + fname);
                        File.Move(this.fileUtils.importFolder + fname, this.fileUtils.importedFolder + fname);
                    }

                    return false;//"Failed to load JPEG file");
                }//end if import fails
            }//end if jpeg
            /*
            else if (extension == "chest" || extension == ".chest")//Run if file is a jpeg
            {
                if (!this.importChest(fname))
                {
                    if (!File.Exists(this.fileUtils.trashFolder + fname))
                    {
                        File.Move(this.fileUtils.importFolder + fname, this.fileUtils.trashFolder + fname);
                        Console.Out.WriteLine("File moved to trash: " +  fname);
                    }
                    return false;//"Failed to load JPEG file");
                }//end if import fails
            }//end if jpeg
            */
            else if (!this.importStack(fname))// run if file is a stack
            {
                if (! File.Exists(this.fileUtils.trashFolder + fname)) {
                    File.Move(this.fileUtils.importFolder + fname, this.fileUtils.trashFolder + fname);
                    Console.Out.WriteLine( StringHolder.importer_1);// "File moved to trash: " + fname);
                }
                return false;//"Failed to load .stack file");
            }

            if (!File.Exists(this.fileUtils.importedFolder + fname))
            {
                File.Move(this.fileUtils.importFolder + fname, this.fileUtils.importedFolder + fname);
            }
            else {
                File.Delete(this.fileUtils.importedFolder + fname);
                File.Move(this.fileUtils.importFolder + fname, this.fileUtils.importedFolder + fname);
            }
            
            //End if the file is there
            return true;
        }//End importOneFile


        /* IMPORT ONE JPEG */
        private bool importJPEG(String fileName)//Move one jpeg to suspect folder. 
        {
            bool isSuccessful = false;
           // Console.Out.WriteLine("Trying to load: " + this.fileUtils.importFolder + fileName );
            try
            {
              //  Console.Out.WriteLine("Loading coin: " + fileUtils.importFolder + fileName);
                //CloudCoin tempCoin = this.fileUtils.loadOneCloudCoinFromJPEGFile( fileUtils.importFolder + fileName );

                /*Begin import from jpeg*/

                /* GET the first 455 bytes of he jpeg where the coin is located */
                String wholeString = "";
                byte[] jpegHeader = new byte[455];
               // Console.Out.WriteLine("Load file path " + fileUtils.importFolder + fileName);
                FileStream fileStream = new FileStream( fileUtils.importFolder + fileName, FileMode.Open, FileAccess.Read);
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

                CloudCoin tempCoin = this.parseJpeg(wholeString);
               // Console.Out.WriteLine("From FileUtils returnCC.fileName " + tempCoin.fileName);

                /*end import from jpeg file */



             //   Console.Out.WriteLine("Loaded coin filename: " + tempCoin.fileName);
                this.fileUtils.writeTo(this.fileUtils.suspectFolder, tempCoin);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Console.Out.WriteLine("File not found: " + fileName + ex);
            }
            catch (IOException ioex)
            {
                Console.Out.WriteLine("IO Exception:" + fileName + ioex);
            }// end try catch
            return isSuccessful;
        }

        /* IMPORT ONE STACK FILE */
        private bool importStack(String fileName)
        {
            bool isSuccessful = false;
            //  System.out.println("Trying to load: " + importFolder + fileName );
            try
            {
                String incomeJson = fileUtils.importJSON( this.fileUtils.importFolder + fileName );//Load file as JSON .stack or .chest
                CloudCoin[] tempCoin = null;
                if (!seemsValidJSON(incomeJson))
                {

                    tempCoin = this.fileUtils.loadManyCloudCoinFromJsonFile(this.fileUtils.importFolder + fileName, incomeJson);

                }
                
                if (tempCoin != null)
                {
                    for (int i = 0; i < tempCoin.Length; i++)
                    {
                        this.fileUtils.writeTo(this.fileUtils.suspectFolder, tempCoin[i]);
                    }//end for each temp Coin
                    return true;
                }//end if lenth not null. 
                else {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.WriteLine(StringHolder.importer_importstack1 );// "The following file does not appear to be valid JSON. It will be moved to the Trash Folder: ");
                    Console.Out.WriteLine(fileName);
                    Console.Out.WriteLine( StringHolder.importer_importstack2);// "Paste the text into http://jsonlint.com/ to check for validity.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;//CloudCoin was null so move to trash
                }
               
            }
            catch (FileNotFoundException ex)
            {
                Console.Out.WriteLine("File not found: " + fileName + ex);
            }
            catch (IOException ioex)
            {
                Console.Out.WriteLine("IO Exception:" + fileName + ioex);
            }

            // end try catch
            return isSuccessful;
        }//import stack


        public bool seemsValidJSON(string json)
        {
            /*This does some simple tests to see if the JSON is valid. It is not precise.*/


            if (json.Count(f => f == '{') != json.Count(f => f == '}'))
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine( StringHolder.importer_seemsValidJSON_1 );// "The stack file did not have a matching number of { }. There were " + json.Count(f => f == '{') + " {, and " + json.Count(f => f == '}') + " }");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of currly brackets open are the same as closed
            if (json.Count(f => f == '[') != json.Count(f => f == ']'))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine( StringHolder.importer_seemsValidJSON_2);//"The stack file did not have a matching number of []. There were " + json.Count(f => f == '[') + " [, and " + json.Count(f => f == ']') + " ]");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of  brackets open are the same as closed
            if (IsOdd(json.Count(f => f == '\"')))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine( StringHolder.importer_seemsValidJSON_3);//"The stack file did not have a matching number of double quotations");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of
            if (IsNotFive(json.Count(f => f == ':') - 1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine( StringHolder.importer_seemsValidJSON_4);//"The stack file did not have a the right number of full colons :");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of
            return true;
        }//end seems valid


        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
        public static bool IsNotFive(int value)
        {
            return value % 5 != 0;
        }

/*
        private bool importChest(String fileName)
        {
            bool isSuccessful = false;
            //  System.out.println("Trying to load: " + importFolder + fileName );
            try
            {
                CloudCoin[] tempCoin = fileUtils.loadManyCloudCoinFromJsonFile(this.fileUtils.importFolder + fileName,);

                for (int i = 0; i < tempCoin.Length; i++)
                {
                    this.fileUtils.writeTo(this.fileUtils.bankFolder, tempCoin[i]);
                }//end for each temp Coin
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Console.Out.WriteLine("File not found: " + fileName + ex);
            }
            catch (IOException ioex)
            {
                Console.Out.WriteLine("IO Exception:" + fileName + ioex);
            }

            // end try catch
            return isSuccessful;
        }//import stack
    */

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
            for (int i = 0; i < 25; i++)
            {
                cc.pans[i] = cc.generatePan();
                cc.setPastStatus("undetected", i);
            }
            cc.fileName = cc.getDenomination() + ".CloudCoin." + cc.nn + "." + cc.sn + ".";
            //Console.Out.WriteLine("parseJpeg cc.fileName " + cc.fileName);
            // end for each pan
            return cc;
        }// end parse Jpeg


    }//end class
}//end namespace
