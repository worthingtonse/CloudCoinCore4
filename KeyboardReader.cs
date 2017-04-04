using System;
using System.Text;
using System.Linq;


namespace Foundation
{
    class KeyboardReader
    {

        public static int INT_MESSAGE = 0;

        public static int DOUBLE_MESSAGE = 1;

        public static int CHAR_MESSAGE = 2;

        public static int STRING_MESSAGE = 3;

        public static int BOOLEAN_MESSAGE = 4;

        public static int LONG_MESSAGE = 5;

        public static int NUM_ERROR_MESSAGES = 6;

        private static string DEFAULT_ERROR_MESSAGE = "Please reenter. ";

        private string prompt = "> ";

        private string[] errorMessages;

        public KeyboardReader()
        {
            this.prompt = "> ";

        }


        public void setPrompt(string newPrompt)
        {
            this.prompt = newPrompt;
        }



        public void setErrorMessage(int idx, string msg)
        {
            if (((idx >= 0)
                        && (idx < this.errorMessages.Length)))
            {
                this.errorMessages[idx] = msg;
            }

        }

        public void setErrorMessageString(string msg)
        {
            this.errorMessages[STRING_MESSAGE] = msg;
        }

        public void setErrorMessageInt(string msg)
        {
            this.errorMessages[INT_MESSAGE] = msg;
        }

        public void setErrorMessageDouble(string msg)
        {
            this.errorMessages[DOUBLE_MESSAGE] = msg;
        }

        public void setErrorMessageChar(string msg)
        {
            this.errorMessages[CHAR_MESSAGE] = msg;
        }

        public void setErrorMessageBoolean(string msg)
        {
            this.errorMessages[BOOLEAN_MESSAGE] = msg;
        }

        public void setErrorMessageLong(string msg)
        {
            this.errorMessages[LONG_MESSAGE] = msg;
        }

        public string readString()
        {
            char theChar = 'x';
            string result = "";
            bool done = false;
            while (!done)
            {
                theChar = this.nextChar();
                if ((theChar == '\n'))
                {
                    done = true;
                }
                else if ((theChar == '\r'))
                {

                }
                else
                {
                    result = (result + theChar);
                }
            }

            return result;
        }

        public string readString(bool allowEmpty)
        {
            string result = this.readString();
            if (!allowEmpty)
            {
                while ((result.Length == 0))
                {
                    Console.Out.WriteLine(("Empty input not allowed. " + this.errorMessages[STRING_MESSAGE]));
                    Console.Out.Write(this.prompt);
                    result = this.readString();
                }

            }

            return result;
        }

        public string readString(string[] args)
        {
            string result = this.readString();
            result = result.ToLower();
            while (!args.Any(result.Contains))
            {
                Console.Out.WriteLine(("Please enter one of the following: " + ConvertStringArrayToString(args)));
                Console.Out.Write(this.prompt);
                result = this.readString(args);
            }

            return result;
        }

        public string readString(int charLimit)
        {
            string result = this.readString();
            if ((result.Length > charLimit))
            {
                result = result.Substring(0, charLimit);
            }

            return result;
        }

        public string readString(bool allowEmpty, int charLimit)
        {
            string result = this.readString(allowEmpty);
            if ((result.Length > charLimit))
            {
                result = result.Substring(0, charLimit);
            }

            return result;
        }

        public int readInt()
        {
            string inputString = "";
            int number = 0;
            bool done = false;
            while (!done)
            {
                try
                {
                    inputString = this.readString();
                    inputString = inputString.Trim();
                    number = Convert.ToInt32(inputString);
                    done = true;
                }
                catch (FormatException e)
                {
                    Console.Out.WriteLine(("Input is not an integer. " + this.errorMessages[INT_MESSAGE]));
                    Console.Out.Write(this.prompt);
                }

            }

            return number;
        }

        public int readInt(int min, int max)
        {
            string inputString = "";
            int number = 0;
            bool done = false;
            while (!done)
            {
                try
                {
                    inputString = this.readString();
                    inputString = inputString.Trim();
                    number = Convert.ToInt32(inputString);
                    if (((number < min) || (number > max)))
                    {
                        Console.Out.WriteLine(("Please enter an integer between " + (min + (" and " + max))));
                    }
                    else
                    {
                        done = true;
                    }

                }
                catch (FormatException e)
                {
                    Console.Out.WriteLine("Input is not an integer. " + " Please enter an integer between " + min + " and " + max);
                    Console.Out.Write(this.prompt);
                }

            }

            return number;
        }

        public int readInt(int[] args)
        {
            int result = this.readInt();
            while (!this.checkInArray(result, args))
            {
                Console.Out.WriteLine(("Please enter one of the following: " + string.Join(",", args)));
                Console.Out.Write(this.prompt);
                result = this.readInt(args);
            }

            return result;
        }

        private bool checkInArray(int currentState, int[] myArray)
        {
            bool found = false;
            for (int i = 0; (!found
                        && (i < myArray.Length)); i++)
            {
                found = (myArray[i] == currentState);
            }

            return found;
        }


        static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(' ');
            }
            return builder.ToString();
        }//End convert string array to string


        /**
    * Use System.in.read to read the next character from the
    * STDIN stream.
    */
        private char nextChar()
        {
            int charAsInt = -1;
            try
            {
                charAsInt = Console.Read();

            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Fatal error. Exiting program.");
                return (char)charAsInt;
            }

            return (char)charAsInt;
        }//end nextChar
    }
}
