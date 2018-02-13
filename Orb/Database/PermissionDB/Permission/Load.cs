using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Orb
{
    public static partial class Database
    {
        public static partial class PermissionDB
        {
            public partial class Permission
            {
                public void LoadAll(string Location)
                {
                    if (Location.EndsWith("Permissions.Dat")) Location = Location.Remove(Location.Length - 15);
                    if (Location.EndsWith("/")) Location = Location.Remove(Location.Length - 1);
                    string InputFile = Location + "/Permissions.Dat";
                    Utilities.IO.PrepareFile(InputFile);
                    string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                    Logger.Log.SystemMessage("Loading Permissions...");
                    foreach (string line in InfoFileContents)
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
                        string Header = ThisLine.Split('\t')[0];
                        string Data = ThisLine.Remove(0, ((Header.Length) + 1));
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
                            Type temp = this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).FieldType;
                            if (temp == typeof(IPAddress)) inputvalue = IPAddress.Parse(Data);
                            else if (temp == typeof(Database.GroupDB.Group)) inputvalue = Database.GroupDB.FindGroup(Data);
                            else if (temp == typeof(Database.UserDB.User)) inputvalue = Database.UserDB.Find(Data);
                            else if (temp == typeof(DateTime)) inputvalue = Data.ToDateTime();
                            else if (temp == typeof(TimeSpan)) inputvalue = TimeSpan.Parse(Data);
                            else if (temp == typeof(Double))
                            {
                                if (Data.ToUpperInvariant() == "TRUE") inputvalue = Double.PositiveInfinity;
                                else if (Data.ToUpperInvariant() == "FALSE") inputvalue = Double.NegativeInfinity;
                                else if (Data.ToUpperInvariant() == "NaN") inputvalue = Double.NaN;
                                else inputvalue = Double.Parse(Data);
                            }
                            else inputvalue = Convert.ChangeType(Data, temp);
                            this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).SetValue(this, inputvalue);
                            //Logger.Console.WriteLine(this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).Name.SuffixTabs(4) + Data);
                        }
                        catch (Exception e)
                        {
                            ///*
                            Logger.Console.WriteLine("&cERROR&e " + e);
                            //Thread.Sleep(50000);
                            //*/
                            //System.Console.WriteLine(e);
                            //Logger.Console.WriteLine("&b" + String.Format("Unrecognised Permission: {0}, {1}", Header.ToUpperInvariant(), Data));
                            Logger.Log.SystemMessage(String.Format("Unrecognised Permission: {0}, {1}", Header.ToUpperInvariant(), Data));
                            //Unrecognised Value.
                            continue;
                        }
                    }
                    Logger.Log.SystemMessage("Loaded Permissions.");
                    //end of loading is here.
                }
            }
        }
    }
}