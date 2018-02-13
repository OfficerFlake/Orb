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
        public static partial class UserDB
        {
            public partial class User
            {
                public partial class GroupReference
                {
                    public void LoadAll()
                    {
                        string InputFile = "./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name + "/Info.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Users/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name);
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name);
                        Utilities.IO.PrepareFile(InputFile);
                        string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                        Logger.Log.SystemMessage("Loading Values for Group Reference\"" + Group.Name + "\"...");
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
                                else if (temp == typeof(Database.GroupDB.Group.Rank))
                                {
                                    try
                                    {
                                        inputvalue = this.Group.Ranks.First(x => x.Name == Data);
                                    }
                                    catch (Exception e)
                                    {
                                        inputvalue = GroupDB.NoRank;
                                        //throw new System.ArgumentException("Rank not Found: " + Data, e);
                                    }
                                }
                                else if (temp == typeof(Database.UserDB.User)) inputvalue = Database.UserDB.Find(Data);
                                else if (temp == typeof(DateTime)) inputvalue = Data.ToDateTime();
                                else if (temp == typeof(TimeSpan)) inputvalue = TimeSpan.Parse(Data);
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
                                Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                                //Unrecognised Value.
                                continue;
                            }
                        }
                        Logger.Log.SystemMessage("Loaded Values for Group \"" + Group.Name + "\".");
                        //end of loading is here.
                    }
                    public void Load(string Key)
                    {
                        string InputFile = "./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name + "/Info.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Users/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name);
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name);
                        Utilities.IO.PrepareFile(InputFile);
                        string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                        Logger.Log.SystemMessage("Loading Values for Group \"" + Group.Name + "\"...");
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
                            if (Key.ToUpperInvariant() != Header.ToUpperInvariant())
                            {
                                //Do not load any variable except what we need.
                                continue;
                            }
                            switch (Header.ToUpperInvariant())
                            {
                                #region Switches
                                case Strings.Group:
                                    try { Group = GroupDB.FindGroup(Converted.ToString()); }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.Group + " given is NOT a group."); }
                                    break;
                                case Strings.Rank:
                                    try
                                    {
                                        foreach (GroupDB.Group.Rank TestableRank in Parent.Groups.Select(x => x.Rank))
                                        {
                                            if (TestableRank.Name == Converted.ToString()) Rank = TestableRank;
                                        }
                                    }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.Rank + " given is NOT a rank."); }
                                    break;
                                case Strings.RankReason:
                                    try { RankReason = (string)Converted; }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.RankReason + " given is NOT a string."); }
                                    break;
                                case Strings.RankedBy:
                                    try { RankedBy = UserDB.Find(Converted.ToString()); }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.RankedBy + " given is NOT a user."); }
                                    break;
                                case Strings.PreviousRank:
                                    try
                                    {
                                        foreach (GroupDB.Group.Rank TestableRank in Parent.Groups.Select(x => x.Rank))
                                        {
                                            if (TestableRank.Name == Converted.ToString()) PreviousRank = TestableRank;
                                        }
                                    }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.PreviousRank + " given is NOT a rank."); }
                                    break;
                                case Strings.RankDate:
                                    try { RankDate = DateTime.Parse(Converted.ToString()); }
                                    catch (Exception e) { Logger.Log.Bug(e, Strings.RankDate + " given is NOT a date."); }
                                    break;
                                #endregion
                                default:
                                    Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                                    //Unrecognised Value.
                                    break;
                            }
                        }
                        Logger.Log.SystemMessage("Loaded Values for Group \"" + Group.Name + "\".");
                        //end of loading is here.
                    }
                }
            }
        }
    }
}