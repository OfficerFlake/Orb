using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

namespace Orb
{
    public static partial class Utilities
    {
        public static class IO
        {
            /// <summary>
            /// Returns True if it needed to create the file.
            /// </summary>
            /// <param name="Filename"></param>
            /// <returns></returns>
            public static bool PrepareFile(string Filename)
            {
                if (!(File.Exists(Filename)))
                {
                    //.Dispose releases the file for use by the OS again.
                    File.Create(Filename).Dispose();
                    return true;
                }
                return false;
            }
            public static void PrepareDirectory(string DirectoryName)
            {
                if (!(Directory.Exists(DirectoryName))) Directory.CreateDirectory(DirectoryName);
            }
            public static void WriteFile(string Filename, string[] Message)
            {
                try
                {
                    File.AppendAllLines(Filename, Message);
                }
                catch (Exception e)
                {
                        //The Bug Logger itself is in use!
                }
            }
            public static object StringToVariable(string Input)
            {
                Decimal DecimalValue = (decimal)0;
                IPAddress IP = IPAddress.Any;
                Int32 Integer = 0;

                if (Input == "0")
                {
                    return 0;
                }

                if (!(Input.Contains(".")))
                {
                    if (Int32.TryParse(Input, out Integer))
                    {
                        return Integer;
                    }
                }

                if (Decimal.TryParse(Input, out DecimalValue))
                {
                    return DecimalValue;
                }

                if (IPAddress.TryParse(Input, out IP))
                {
                    return IP;
                }

                switch (Input.ToUpperInvariant())
                {
                    case "TRUE":
                        return true;
                    case "FALSE":
                        return false;
                    default:
                        return Input;
                }
            }
            public static string StringToHex(string Input)
            {
                string completestring = "";

                for (int i = 0; i < Input.Length; i++)
                {

                    int First;
                    int Second;
                    First = ((int)Input[i]) / 16;
                    Second = ((int)Input[i]) - (((int)Input[i]) / 16) * 16;
                    completestring += IntToHex(First) + IntToHex(Second) + ":";
                }
                completestring = completestring.Remove(completestring.Length - 1, 1);
                return completestring;
            }
            public static string IntToHex(int i)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return i.ToString();
                    case 10:
                        return "A";
                    case 11:
                        return "B";
                    case 12:
                        return "C";
                    case 13:
                        return "D";
                    case 14:
                        return "E";
                    case 15:
                        return "F";
                }
                return "?";
            }
            public static string GetAllGroupsRanks()
            {
                string output = "LISTING GROUP RANKS\n";
                output += "===================\n";
                foreach (Database.GroupDB.Group ThisGroup in Database.GroupDB.List)
                {
                    output += "    Ranks for Group: \"" + ThisGroup.Name + "\"\n";
                    output += "        " + ThisGroup.ListRanksToString() + "\n";
                }
                return output;
            }
            public static string GetAllUsersGroups()
            {
                string output = "LISTING USERS GROUPS\n";
                output += "====================\n";
                foreach (Database.UserDB.User ThisUser in Database.UserDB.List)
                {
                    output += "    Groups for User: \"" + ThisUser.Name + "\"\n";

                    if (ThisUser.Groups.Count == 0)
                    {
                        output += "        No Groups.\n";
                    }

                    foreach (Database.UserDB.User.GroupReference ThisGroupRefernce in ThisUser.Groups)
                    {
                        output += "        " + ThisGroupRefernce.Group.Name + ": " + ThisGroupRefernce.Rank.Name + "\n";
                    }
                }
                return output;
            }
            public static string[] ReadAllLines(string FileName)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        return File.ReadAllLines(FileName);
                    }
                    catch
                    {
                    }
                }
                return new string[] { "" };
            }
        }
        public static class DateTimeUtilities
        {
            public static bool TryParseMiniTimespan(string text, out TimeSpan result)
            {
                if (text == null) throw new ArgumentNullException("text");
                try
                {
                    result = ParseMiniTimespan(text);
                    return true;
                }
                catch (ArgumentException)
                {
                }
                catch (OverflowException)
                {
                }
                catch (FormatException) { }
                result = TimeSpan.Zero;
                return false;
            }
            public static TimeSpan ParseMiniTimespan(string text)
            {
                if (text == null) throw new ArgumentNullException("text");

                text = text.Trim();
                bool expectingDigit = true;
                TimeSpan result = TimeSpan.Zero;
                int digitOffset = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (expectingDigit)
                    {
                        if (text[i] < '0' || text[i] > '9')
                        {
                            throw new FormatException();
                        }
                        expectingDigit = false;
                    }
                    else
                    {
                        if (text[i] < '0' || text[i] > '9')
                        {
                            string numberString = text.Substring(digitOffset, i - digitOffset);
                            digitOffset = i + 1;
                            int number = Int32.Parse(numberString);
                            switch (Char.ToLower(text[i]))
                            {
                                case 's':
                                    result += TimeSpan.FromSeconds(number);
                                    break;
                                case 'm':
                                    result += TimeSpan.FromMinutes(number);
                                    break;
                                case 'h':
                                    result += TimeSpan.FromHours(number);
                                    break;
                                case 'd':
                                    result += TimeSpan.FromDays(number);
                                    break;
                                case 'w':
                                    result += TimeSpan.FromDays(number * 7);
                                    break;
                                default:
                                    throw new FormatException();
                            }
                        }
                    }
                }
                return result;
            }
            public static string[] FormatDateTime(DateTime ThisDateTime)
            {
                #region Year
                string Year = ThisDateTime.Year.ToString();
                if (ThisDateTime.Year.ToString().Length != 4)
                {
                    Year = Year.Slice(0, 4);
                    Year = "2013".Slice(0, Year.Length-4) + Year;
                }
                #endregion
                #region Month
                string Month = ThisDateTime.Month.ToString();
                if (ThisDateTime.Month.ToString().Length == 1)
                {
                    Month = "0" + ThisDateTime.Month.ToString();
                }
                #endregion
                #region Day
                string Day = ThisDateTime.Day.ToString();
                if (ThisDateTime.Day.ToString().Length == 1)
                {
                    Day = "0" + ThisDateTime.Day.ToString();
                }
                #endregion
                #region Hour
                string Hour = ThisDateTime.Hour.ToString();
                if (ThisDateTime.Hour.ToString().Length == 1)
                {
                    Hour = "0" + ThisDateTime.Hour.ToString();
                }
                #endregion
                #region Minute
                string Minute = ThisDateTime.Minute.ToString();
                if (ThisDateTime.Minute.ToString().Length == 1)
                {
                    Minute = "0" + ThisDateTime.Minute.ToString();
                }
                #endregion
                #region Second
                string Second = ThisDateTime.Second.ToString();
                if (ThisDateTime.Second.ToString().Length == 1)
                {
                    Second = "0" + ThisDateTime.Second.ToString();
                }
                #endregion
                string[] Output = { Year, Month, Day, Hour, Minute, Second };
                return Output;
            }
            public static string GetTotalHoursAs8BitStr(TimeSpan ThisTimeSpan)
            {
                double i = Math.Floor(ThisTimeSpan.TotalHours);
                if (i >= 1000000000)
                {
                    return "    INF ";
                }
                if (i >= 100000000)
                {
                    return Math.Round((i / 1000000), 2).ToString() + "M ";
                }
                if (i >= 10000000)
                {
                    return Math.Round((i / 1000000), 3).ToString() + "M ";
                }
                if (i >= 1000000)
                {
                    return Math.Round((i / 1000000), 4).ToString() + "M ";
                }
                if (i >= 100000)
                {
                    return Math.Round((i / 1000), 2).ToString() + "K ";
                }
                if (i >=10000) {
                    return Math.Round((i / 1000), 3).ToString() + "K ";
                }
                else {
                    double j = Math.Floor((TimeSpan.FromMinutes(Math.Floor(ThisTimeSpan.TotalMinutes)) - TimeSpan.FromHours(Math.Floor(ThisTimeSpan.TotalHours))).TotalMinutes);
                    string decimalending = j.ToString();
                    while (decimalending.Length < 2)
                    {
                        decimalending = "0" + decimalending;
                    }
                    string output = i.ToString() + "." + decimalending + " ";
                    while (output.Length < 8)
                    {
                        output = " " + output;
                    }
                    if (output.Length > 8) return "    ??? ";
                    return output;
                }
            }
            public static string ToYearTimeDescending(string[] DateTime)
            {
                return DateTime[0] + "/" + DateTime[1] + "/" + DateTime[2] + " " + DateTime[3] + ":" + DateTime[4] + ":" + DateTime[5];
            }
            public static string ToYearAscending(string[] DateTime)
            {
                return DateTime[2] + "/" + DateTime[1] + "/" + DateTime[0];
            }
        }

        /// <summary>
        /// Returns the selected area of a string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Slice(this string source, int start, int end)
        {
            if (end > source.Length)
            {
                end = source.Length;
            }
            if (start > source.Length)
            {
                start = source.Length;
            }
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }
            if (start < 0) // Keep this for negative end support
            {
                start = source.Length + start;
            }
            if (end < 0) // Keep this for negative end support
            {
                end = 0;
            }
            if (start < 0) // Keep this for negative end support
            {
                start = 0;
            }
            int len = end - start;               // Calculate length
            try
            {
                return source.Substring(start, len); // Return Substring of length
            }
            catch
            {
                return "NULL";
            }
        }

        /// <summary>
        /// Converts an Integer to a string whose length is exactly 8 Char long.
        /// </summary>
        /// <param name="ThisNumber"></param>
        /// <returns></returns>
        public static string To8BitString(this int ThisNumber)
        {
            if (ThisNumber >= 10000000)
            {
                return "TooHigh ";
            }
            else
            {
                string output = ThisNumber.ToString() + " ";
                while (output.Length < 8)
                {
                    output = " " + output;
                }
                if (output.Length > 8) return "    ??? ";
                return output;
            }
        }

        /// <summary>
        /// Converts an Unsigned Integer to a Binary String ("10010001" etc...)
        /// </summary>
        /// <param name="ThisNumber"></param>
        /// <returns></returns>
        public static string ToBinaryString(this uint ThisNumber)
        {
            string m = "";
            for (int i = 7; i >= 0; i--)
            {
                //Logger.Console.WriteLine(ThisNumber.ToString());
                //Logger.Console.WriteLine((Math.Pow(2, i)).ToString());
                //Logger.Console.WriteLine((ThisNumber / (Math.Pow(2, i))).ToString());
                m += Math.Floor((ThisNumber / (Math.Pow(2, i)))).ToString();
                if (Math.Floor((ThisNumber / (Math.Pow(2, i)))) > 0)
                {
                    ThisNumber = (uint)(ThisNumber - (Math.Floor(ThisNumber / (Math.Pow(2, i))) * (Math.Pow(2, i))));
                }
            }
            return m;
        }

        /// <summary>
        /// Converts a Binary String ("10010011" etc...) back to an Unsigned Integer
        /// </summary>
        /// <param name="ThisString"></param>
        /// <returns></returns>
        public static uint FromBinaryStringToUint(this string ThisString)
        {
            //Logger.Console.WriteLine(ThisString);
            string tempstring = ThisString;
            tempstring = tempstring.Replace("0", "");
            tempstring = tempstring.Replace("1", "");
            if (tempstring.Length > 0 || ThisString.Length != 8)
            {
                return 0;
            }
            uint x = 0;
            if (ThisString[0] == '1') x += 128;
            if (ThisString[1] == '1') x += 64;
            if (ThisString[2] == '1') x += 32;
            if (ThisString[3] == '1') x += 16;
            if (ThisString[4] == '1') x += 8;
            if (ThisString[5] == '1') x += 4;
            if (ThisString[6] == '1') x += 2;
            if (ThisString[7] == '1') x += 1;
            //Logger.Console.WriteLine(x.ToString());
            return x;
        }
        
        /// <summary>
        /// Converts an Unsigned Integer to a Boolean, where 1 is True, and all other values are False.
        /// </summary>
        /// <param name="ThisUint"></param>
        /// <returns></returns>
        public static bool ToBool(this uint ThisUint)
        {
            if (ThisUint == 0) return false;
            else if (ThisUint == 1) return true;
            else return false;
        }

        /// <summary>
        /// Converts a Char to a Boolean, where 1 is True, and all other values are False.
        /// </summary>
        /// <param name="ThisChar"></param>
        /// <returns></returns>
        public static bool ToBool(this char ThisChar)
        {
            if (ThisChar == '0') return false;
            else if (ThisChar == '1') return true;
            else return false;
        }

        /// <summary>
        /// Converts a Byte Array of length 4 to an Unsigned Integer
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static uint ToUint(this byte[] ThisByte)
        {
            if (ThisByte.Length != 4)
            {
                Logger.Console.WriteLine("Byte[].ToUint() -> Invalid byte[] (not 4 bytes long) {" + ThisByte.Length + "}");
                return 0;
            }
            return BitConverter.ToUInt32(ThisByte, 0);
        }

        /// <summary>
        /// Converts a Byte Array of length 4 to a Signed Integer
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static int ToInt(this byte[] ThisByte)
        {
            if (ThisByte.Length != 4)
            {
                Logger.Console.WriteLine("Byte[].ToInt() -> Invalid byte[] (not 4 bytes long) {" + ThisByte.Length + "}");
                return 0;
            }
            return BitConverter.ToInt32(ThisByte, 0);
        }

        /// <summary>
        /// Converts a Boolean to an Unsigned Integer.
        /// </summary>
        /// <param name="ThisBool"></param>
        /// <returns></returns>
        public static uint ToUint(this bool ThisBool)
        {
            if (ThisBool) return 1;
            else return 0;
        }

        /// <summary>
        /// Converts a Byte Array of length 4 to an Unsigned Float.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static float ToFloat(this byte[] ThisByte)
        {
            if (ThisByte.Length != 4)
            {
                Logger.Console.WriteLine("Byte[].ToFloat() -> Invalid byte[] (not 4 bytes long) {" + ThisByte.Length + "}");
                return 0;
            }
            return BitConverter.ToSingle(ThisByte, 0);
        }

        /// <summary>
        /// Changes all Null Characters in a string to Spaces, to avoid console tearing.
        /// </summary>
        /// <param name="ThisString"></param>
        /// <returns></returns>
        public static string Clean(this string ThisString)
        {
            return ThisString.Replace("\0", " ");
        }

        /// <summary>
        /// Masks all control characters in a string, as to safely write to the console.
        /// </summary>
        /// <param name="ThisString"></param>
        /// <returns></returns>
        public static string CleanASCII(this string ThisString)
        {
            ThisString = ThisString.Replace("\0", "#");
            string output = "";
            foreach (byte thisbyte in ThisString.ToArray().ToByteArray())
            {
                if (thisbyte < 32 || thisbyte > 127)
                {
                    output += "#";
                }
                else
                {
                    output += (char)thisbyte;
                }
            }
            return output;
        }

        /// <summary>
        /// Converts a byte array of length 2 to a Unsigned Short.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static ushort ToUshort(this byte[] ThisByte)
        {
            if (ThisByte.Length != 2)
            {
                Logger.Console.WriteLine("Byte[].ToUshort() -> Invalid byte[] (not 2 bytes long) {" + ThisByte.Length + "}");
                return 0;
            }
            return BitConverter.ToUInt16(ThisByte, 0);
        }

        /// <summary>
        /// Converts a byte array of length 2 to a Unsigned Short.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static short ToShort(this byte[] ThisByte)
        {
            if (ThisByte.Length != 2)
            {
                Logger.Console.WriteLine("Byte[].ToUshort() -> Invalid byte[] (not 2 bytes long) {" + ThisByte.Length + "}");
                return 0;
            }
            return BitConverter.ToInt16(ThisByte, 0);
        }

        /// <summary>
        /// Converts a byte array of length 1 to a Unsigned Byte.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static byte ToByte(this byte[] ThisByte)
        {
            if (ThisByte.Length != 1)
            {
                Logger.Console.WriteLine("Byte[].ToByte() -> Invalid byte[] (not 1 byte long) {" + ThisByte.Length + "}");
                return 0;
            }
            return (byte)ThisByte[0];
        }

        public static byte[] ToByteArray(this byte ThisByte)
        {
            return new byte[] { ThisByte };
        }

        public static byte[] ToByteArray(this sbyte ThisSByte)
        {
            return new byte[] { (byte)ThisSByte };
        }

        /// <summary>
        /// Gets a boolean value based on the selected column of a binary byte (Right to Left)
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public static bool GetBit(this byte ThisByte, int Cell)
        {
            if (Cell >= 7 || Cell < 0)
            {
                Logger.Console.WriteLine("Byte.GetBit() -> Invalid Cell Position (not 0-7) {" + Cell.ToString() + "}");
                return false;
            }
            if ((ThisByte & (int)(Math.Pow((double)2, (double)Cell))) == 1)
            {
                return true;
            }
            else if ((ThisByte & (int)(Math.Pow((double)2, (double)Cell))) == 0)
            {
                return false;
            }
            else
            {
                Logger.Console.WriteLine("Byte.GetBit() -> Result != 0|1 {" + Cell.ToString() + "}");
                Logger.Console.WriteLine("Byte.GetBit() -> Input != 0~255 {" + ((int)ThisByte).ToString() + "}");
                Thread.Sleep(5000);
                return false;
            }
        }

        /// <summary>
        /// Sets the sepecific bit of a byte to 1.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public static byte SetBit(this byte ThisByte, int Cell)
        {
            if (Cell >= 8 || Cell < 0)
            {
                Logger.Console.WriteLine("Byte.SetBit() -> Invalid Cell Position (not 0-7) {" + Cell.ToString() + "}");
                return ThisByte;
            }
            byte SetTo = (byte)(Math.Pow((double)2, (double)Cell));
            return (byte)(ThisByte & SetTo);
        }

        /// <summary>
        /// UnSets the sepecific bit of a byte to 1.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public static byte UnSetBit(this byte ThisByte, int Cell)
        {
            if (Cell >= 8 || Cell < 0)
            {
                Logger.Console.WriteLine("Byte.UnSetBit() -> Invalid Cell Position (not 0-7) {" + Cell.ToString() + "}");
                return ThisByte;
            }
            byte SetTo = (byte)(Math.Pow((double)2, (double)Cell));
            return (byte)(ThisByte & ~SetTo);
        }

        /// <summary>
        /// returns the UNITS column of a Hex number between 0 and 255, as a value from 0 to 15.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static byte GetUnits(this byte ThisByte)
        {
            return (byte)(ThisByte & 15);
        }

        /// <summary>
        /// returns the TENS column of a Hex number between 0 and 255, as a value from 0 to 15.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static byte GetTens(this byte ThisByte)
        {
            return (byte)((ThisByte & 240)/16);
        }

        /// <summary>
        /// returns a value from 0 to 255, where the byte given is the units only. (overflow bits not supported.) [Bitwise it shifts the numbers 4 binary to the left]
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static byte ToTens(this byte ThisByte)
        {
            return (byte)((int)ThisByte * 16);
        }

        /// <summary>
        /// returns a value from 0 to 15, where the byte given is the tens only. (underflow bits not supported.) [Bitwise it shifts the numbers 4 binary to the right]
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static byte ToUnits(this byte ThisByte)
        {
            return (byte)((int)ThisByte / 16);
        }

        /// <summary>
        /// returns a double format percentile of the given byte whos value is between 0 and 15.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static double AsUnitPercentile(this byte ThisByte)
        {
            return ((double)ThisByte / (double)15) * 100;
        }

        /// <summary>
        /// returns a double format percentile of the given byte whos value is between 0 and 240.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static double AsTenthPercentile(this byte ThisByte)
        {
            return ((double)ThisByte / (double)240) * 100;
        }

        /// <summary>
        /// returns a byte whos value is between 0 and 15, from a percentage.
        /// </summary>
        /// <param name="ThisDouble"></param>
        /// <returns></returns>
        public static byte AsUnitByte(this double ThisDouble)
        {
            //100 = 15
            //0 = 0
            return (byte)((ThisDouble / 100) * 15);
        }

        /// <summary>
        /// returns a byte whos value is between 0 and 240, from a percentage.
        /// </summary>
        /// <param name="ThisDouble"></param>
        /// <returns></returns>
        public static byte AsTenthByte(this double ThisDouble)
        {
            //100 = 240
            //0 = 0
            return (byte)(((byte)((ThisDouble / 100) * 15))* 16);
        }

        /// <summary>
        /// Converts a byte array of length 1 to a Signed Byte.
        /// </summary>
        /// <param name="ThisByte"></param>
        /// <returns></returns>
        public static sbyte ToSbyte(this byte[] ThisByte)
        {
            if (ThisByte.Length != 1)
            {
                Logger.Console.WriteLine("Byte[].ToUshort() -> Invalid byte[] (not 1 byte long) {" + ThisByte.Length + "}");
                return 0;
            }
            return ((sbyte[])((Array)ThisByte))[0];
        }

        /// <summary>
        /// Converts a Byte Array to a string where each char in the string is the actual value of the byte.
        /// </summary>
        /// <param name="ByteString"></param>
        /// <returns></returns>
        public static string ToDataString(this byte[] ByteString)
        {
            string output = "";
            foreach (byte ThisByte in ByteString)
            {
                output += (char)ThisByte;
            }
            return output;
        }

        /// <summary>
        /// Converts a Byte Array to a string where each byte is it's numerical equivilent.
        /// </summary>
        /// <param name="ByteString"></param>
        /// <returns></returns>
        public static string ToDebugString(this byte[] ByteString)
        {
            string output = "";
            foreach (byte ThisByte in ByteString)
            {
                if (ThisByte < 32 || ThisByte >= 127)
                {
                    output += "[" + ThisByte.ToString() + "]";
                }
                else
                {
                    output += (char)ThisByte;
                }
            }
            return output;
        }

        /// <summary>
        /// Converts a Byte Array to a string showing the HexiDecimal value of each byte it contains.
        /// </summary>
        /// <param name="ByteString"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] ByteString)
        {
            string output = "";
            output = BitConverter.ToString(ByteString);
            return output;
        }

        /// <summary>
        /// Converts a Byte Array to a string showing the HexiDecimal value of each byte it contains.
        /// </summary>
        /// <param name="ByteString"></param>
        /// <returns></returns>
        public static string ToDebugHexString(this byte[] ByteString)
        {
            string output = "";
            int i = 0;
            foreach (byte ThisByte in ByteString)
            {
                string Letter = "f";
                if (i % 8 == 0) output += "\n";
                if (i % 16 == 15) Letter = "a";
                if (i % 16 == 14) Letter = "a";
                if (i % 16 == 13) Letter = "a";
                if (i % 16 == 12) Letter = "a";
                if (i % 16 == 11) Letter = "b";
                if (i % 16 == 10) Letter = "b";
                if (i % 16 == 09) Letter = "b";
                if (i % 16 == 08) Letter = "b";
                if (i % 16 == 07) Letter = "c";
                if (i % 16 == 06) Letter = "c";
                if (i % 16 == 05) Letter = "c";
                if (i % 16 == 04) Letter = "c";
                if (i % 16 == 03) Letter = "d";
                if (i % 16 == 02) Letter = "d";
                if (i % 16 == 01) Letter = "d";
                if (i % 16 == 00) Letter = "d";
                string number = i.ToString();
                while (number.Length < 4)
                {
                    number = "0" + number;
                }
                output += "&8" + number + "&7(" + "&" + Letter;
                output += ThisByte.ToByteArray().ToHexString();
                output += "&7)-";
                i++;
                //if (i > 15) i = 0;
            }
            return output;
        }

        /// <summary>
        /// Converts a String to a Byte Array for use with other functions.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string Input)
        {
            List<byte> Output = new List<byte>();
            foreach (byte ThisByte in Input)
            {
                Output.Add(ThisByte);
            }
            return Output.ToArray();
        }

        /// <summary>
        /// Removes the final comma and space from a building string list, and adds the finalising period.
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static string FinaliseStringList(this string InputString)
        {
            if (InputString.Length < 2) return InputString;
            return InputString.Remove(InputString.Length - 2) + ".";
        }

        /// <summary>
        /// Converts a List of Strings to a comma seperated, period finalised list.
        /// </summary>
        /// <param name="ThisStringList"></param>
        /// <returns></returns>
        public static string ToStringList(this List<String> ThisStringList)
        {
            string Output = "";
            foreach (string ThisString in ThisStringList)
            {
                Output += ThisString + ", ";
            }
            if (Output == "") return "None.";
            return Output.FinaliseStringList();
        }
        
        /// <summary>
        /// Converts a Char Array to a Byte Array.
        /// </summary>
        /// <param name="ThisCharArray"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this char[] ThisCharArray)
        {
            List<byte> Output = new List<byte>();
            foreach (char ThisChar in ThisCharArray)
            {
                Output.Add((byte)ThisChar);
            }
            return Output.ToArray();
        }

        /// <summary>
        /// Adds spaces to the end of a string, as to make the total string length fix exactly in the specified number of columns.
        /// </summary>
        /// <param name="ThisString"></param>
        /// <param name="Columns"></param>
        /// <returns></returns>
        public static string SuffixTabs(this string ThisString, int Columns)
        {
            int tabsize = 8;
            int TotalLength = Columns * tabsize;
            if (ThisString.Length > TotalLength)
            {
                ThisString.Remove(ThisString.Length - 1);
                return  ThisString + "\t";
            }
            int difference = TotalLength - ThisString.Length;
            for (int i = 0; i < Math.Ceiling((decimal)difference/8); i++)
            {
                ThisString += "\t";
            }
            ThisString.Remove(ThisString.Length - 1);
            return ThisString + "\t";
        }

        /// <summary>
        /// Masks an IPv4 Address's inner two digits.
        /// </summary>
        /// <param name="ThisIP"></param>
        /// <returns></returns>
        public static string Mask(this IPAddress ThisIP)
        {
            if (ThisIP == IPAddress.None || ThisIP == IPAddress.Any || ThisIP.ToString() == "0.0.0.0")
            {
                return "nowhere";
            }
            string output = "";
            if (ThisIP.ToString().Split('.').Count() != 4)
            {
                output = "Some IPv6 Address...";
            }
            else
            {
                output = ThisIP.ToString().Split('.')[0] + ".*.*." + ThisIP.ToString().Split('.')[3];
            }
            return output;
        }

        /// <summary>
        /// Converts an Orb Common Format DateTime String to a System DateTime.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string Input)
        {
            int Day = 0;
            int Month = 0;
            int Year = 0;
            int Hour = 0;
            int Minute = 0;
            int Second = 0;
            try
            {
                Day = Int32.Parse(Input.Slice(0, 2));
                #region month
                switch (Input.ToUpperInvariant().Slice(2, 5))
                {
                    case "JAN":
                        Month = 1;
                        break;
                    case "FEB":
                        Month = 2;
                        break;
                    case "MAR":
                        Month = 3;
                        break;
                    case "APR":
                        Month = 4;
                        break;
                    case "MAY":
                        Month = 5;
                        break;
                    case "JUN":
                        Month = 6;
                        break;
                    case "JUL":
                        Month = 7;
                        break;
                    case "AUG":
                        Month = 8;
                        break;
                    case "SEP":
                        Month = 9;
                        break;
                    case "OCT":
                        Month = 10;
                        break;
                    case "NOV":
                        Month = 11;
                        break;
                    case "DEC":
                        Month = 12;
                        break;
                    default:
                        Month = 1;
                        break;
                }
                #endregion
                Year = Int32.Parse(Input.Slice(5, 7));
                Hour = Int32.Parse(Input.Slice(8, 10));
                Minute = Int32.Parse(Input.Slice(11, 13));
                Second = Int32.Parse(Input.Slice(14, 16));
            }
            catch (Exception e)
            {
                Logger.Console.WriteLine("&cERROR&e: " + e.Message);
                Logger.Console.WriteLine("&cERROR&b: " + Input);
            }
            try
            {
                return new DateTime(Year, Month, Day, Hour, Minute, Second);
            }
            catch
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// Converts a System DateTime to the Orb Common Format.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static String ToCommonString(this DateTime Input)
        {
            string OutString = "";

            string temp = "";

            temp = Input.Day.ToString();
            if (temp.Length == 1) {
                temp = "0" + temp;
            }
            OutString += temp;

            int i = Input.Month;
            #region month
            switch (i)
            {
                case 1:
                    temp = "Jan";
                    break;
                case 2:
                    temp = "Feb";
                    break;
                case 3:
                    temp = "Mar";
                    break;
                case 4:
                    temp = "Apr";
                    break;
                case 5:
                    temp = "May";
                    break;
                case 6:
                    temp = "Jun";
                    break;
                case 7:
                    temp = "Jul";
                    break;
                case 8:
                    temp = "Aug";
                    break;
                case 9:
                    temp = "Sep";
                    break;
                case 10:
                    temp = "Oct";
                    break;
                case 11:
                    temp = "Nov";
                    break;
                case 12:
                    temp = "Dec";
                    break;
                default:
                    temp = "Jan";
                    break;
            }
            #endregion
            OutString += temp;

            temp = Input.Year.ToString();
            if (temp.Length == 4)
            {
                temp = temp.Slice(2, 4);
            }
            else
            {
                temp = "00";
            }
            OutString += temp;

            OutString += " ";

            temp = Input.Hour.ToString();
            if (temp.Length == 1)
            {
                temp = "0" + temp;
            }
            OutString += temp;

            OutString += ":";

            temp = Input.Minute.ToString();
            if (temp.Length == 1)
            {
                temp = "0" + temp;
            }
            OutString += temp;

            OutString += ":";

            temp = Input.Second.ToString();
            if (temp.Length == 1)
            {
                temp = "0" + temp;
            }
            OutString += temp;

            return OutString;
        }

        /// <summary>
        /// Converts Degrees to Radians, for use with Math Functions.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static double ToRadians(this double Input)
        {
            return (Math.PI / 180) * Input;
        }

        /// <summary>
        /// Converts Degrees to Radians, for use with Math Functions.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static double ToRadiansFromShort(this double Input)
        {
            return (Math.PI / 180) * (Input/(65536/2));
        }

        /// <summary>
        /// Converts Degrees to Radians, for use with Math Functions.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static double ToDegreesFromShort(this double Input)
        {
            return (180) * (Input / (65536 / 2));
        }

        /// <summary>
        /// Converted a Signed Byte to a Percentage Value. Used to convert CLA values to Orb Workable Percentages.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static double ToPercentage(this byte Input)
        {
            if (Input > 127)
            {
                return (Input - 128) / 127 * -100;
            }
            else
            {
                return Input / 127 * 100;
            }
        }

        /// <summary>
        /// Adds spaces to the end of a string, or removes characters from the end of a string, to ensure it is the specifed length.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Limit"></param>
        /// <returns></returns>
        public static string SuffixLimit(this string Input, int Limit)
        {
            while (Input.Length < Limit)
            {
                Input = Input + " ";
            }
            while (Input.Length > Limit)
            {
                Input = Input.Slice(0, -1);
            }
            return Input;
        }

        public static double RotateShort(this double Input, double Degrees)
        {
            double OneDegree = 65536 / 360;
            int RotatedVector = (int)Math.Round(OneDegree * Degrees);
            Input += RotatedVector;
            while (Input >= (65536 / 2))
            {
                Input -= 65536;
            }
            return Input;
        }

        public static double ShortAngleFromDegrees(double i)
        {
            if (i >= 65536/2) return 0;
            if (i <= -65536 / 2) return 0;
            while (i < 0)
            {
                i += 360;
            }
            while (i >= 180)
            {
                i -= 360;
            }
            return i * (65536 / 360);
        }
    }
}