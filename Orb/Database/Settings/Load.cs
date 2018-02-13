using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
namespace Orb
{
    public static partial class Database
    {
        public static partial class Settings
        {
            public static void LoadAll()
            {
                string SettingsFile = "./Settings.Dat";
                if (Utilities.IO.PrepareFile(SettingsFile))
                {
                    //file didn't exist, create the default!
                    Settings.SaveAll();
                    Logger.Log.SystemMessage("Settings not found, created new settings.");
                    return;
                }
                string[] SettingsFileContents = Utilities.IO.ReadAllLines(SettingsFile);
                foreach (string line in SettingsFileContents)
                {
                    #region GetDataCouplets
                    string ThisLine = line;
                    if (!(ThisLine.Contains("\t")))
                    {
                        continue;
                    }
                    while (ThisLine.Contains("\t\t"))
                    {
                        ThisLine = ThisLine.Replace("\t\t", "\t");
                    }
                    string Header = ThisLine.Split(new string[] {"\t"}, 2, StringSplitOptions.None)[0];
                    string Data = ThisLine.Split(new string[] { "\t" }, 2, StringSplitOptions.None)[1];
                    //Logger.Console.WriteLine(Data);
                    var Converted = Utilities.IO.StringToVariable(Data);
                    //NOTE: These WILL Bypass GUI mode if enabled, as the GUI setting is turned on until it is read.
                    //if (Converted is Boolean) Logger.Console.WriteLine(String.Format("Bool: {0}", Data));
                    //if (Converted is IPAddress) Logger.Console.WriteLine(String.Format("IPAddress: {0}", Data));
                    //if (Converted is Decimal) Logger.Console.WriteLine(String.Format("Decimal: {0}", Data));
                    //if (Converted is Int32) Logger.Console.WriteLine(String.Format("Int32: {0}", Data));
                    //if (Converted is String) Logger.Console.WriteLine(String.Format("String: {0}", Data));
                    #endregion
                    #region SkipComments
                    if (Header.ToUpperInvariant().StartsWith("REM") || (Header.ToUpperInvariant().StartsWith("#")) || (Header.ToUpperInvariant().StartsWith("//")))
                    {
                        //We don't worry abbout Comments or remarks.
                        continue;
                    }
                    #endregion

                    object inputvalue = null;
                    //Logger.Console.WriteLine(Converted.ToString());
                    //Console.WriteLine(Converted);
                    try
                    {
                        if (Data == "") continue;
                        Type temp = typeof(Database.Settings).GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase).FieldType;
                        if (temp == typeof(IPAddress))
                        {
                            try
                            {
                                inputvalue = IPAddress.Parse(Data);
                            }
                            catch
                            {
                            }
                            try
                            {
                                inputvalue = Dns.GetHostAddresses(Data)[0];
                            }
                            catch (Exception e)
                            {
                                if (inputvalue == null) throw new System.ArgumentException("IPAddress Invalid: " + Data, e);
                            }
                        }
                        else if (temp == typeof(Database.GroupDB.Group)) inputvalue = Database.GroupDB.FindGroup(Data);
                        else if (temp == typeof(Database.UserDB.User)) inputvalue = Database.UserDB.Find(Data);
                        else inputvalue = Convert.ChangeType(Data, temp);
                        typeof(Database.Settings).GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.IgnoreCase).SetValue(typeof(Database.Settings), inputvalue);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                        //Unrecognised Value.
                        continue;
                    }
                    //Console.WriteLine("IN:" + inputvalue.ToString());
                }
            }
        }
    }
}