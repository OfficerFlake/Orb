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
        public static partial class GroupDB
        {
            public partial class Group
            {
                public partial class Rank
                {
                    public void LoadAll(string GroupName, string RankName)
                    {
                        string InputFile = "./Database/Groups/" + GroupName + "/RANKS/" + RankName + "/Info.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName);
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName + "/RANKS/" + RankName);
                        Utilities.IO.PrepareFile(InputFile);
                        string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                        Logger.Log.SystemMessage("Loading Values for Rank \"" + Name + "\"...");
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
                                else if (temp == typeof(DateTime)) Data.ToDateTime();
                                else if (temp == typeof(TimeSpan)) inputvalue = TimeSpan.Parse(Data);
                                else inputvalue = Convert.ChangeType(Data, temp);
                                this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).SetValue(this, inputvalue);
                                //Logger.Console.WriteLine(this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).Name.SuffixTabs(4) + Data);
                            }
                            catch (Exception e)
                            {
                                Logger.Console.WriteLine("&cERROR&e " + e);
                                //Thread.Sleep(50000);
                                Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                                //Unrecognised Value.
                                continue;
                            }

                            #region DECREPTED
                            /*
                            switch (Header.ToUpperInvariant())
                            {
                                #region Switches
                                case Strings.Name:
                                    try { Name = (string)Converted; }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.Name + " given is NOT a string."); }
                                    break;
                                case Strings.DisplayedName:
                                    try { DisplayedName = (string)Converted; }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.DisplayedName + " given is NOT a string."); }
                                    break;
                                #endregion
                                default:
                                    Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                                    //Unrecognised Value.
                                    break;
                            }
                            */
                            #endregion
                        }
                        //Load All Permissions for this rank.
                        //  As permissions in it's own right is a suboject,
                        //  Capabable of it's own specific Load() one var only,
                        //  we can do that instead if a specific permission needs to be reloaded.
                        InputFile = "./Database/Groups/" + GroupName + "/RANKS/" + RankName + "/Permissions.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName);
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName + "/RANKS/" + RankName);
                        Utilities.IO.PrepareFile(InputFile);
                        this.Permissions.LoadAll(InputFile);

                        Logger.Log.SystemMessage("Loaded Values for Rank \"" + Name + "\".");
                        //end of loading is here.
                    }
                }
            }
        }
    }
}