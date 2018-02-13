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
                    public void SaveAll()
                    {
                        string OutputFile = "./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name + "/Info.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Users/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name);
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name);
                        Utilities.IO.PrepareFile(OutputFile);
                        string OutTime = Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(DateTime.Now));
                        List<String> Output = new List<String>();
                        Output.Add("REM AutoCreated by Orb Database Engine, " + OutTime + ".");
                        foreach (var ThisData in this.GetType().GetFields().ToList())
                        {
                            try
                            {
                                string OutString = "";
                                Type temp = ThisData.FieldType;
                                if (temp == typeof(Database.GroupDB.Group)) OutString = GroupDB._List.First(x => x == ThisData.GetValue(this)).Name;
                                else if (temp == typeof(Database.UserDB.User)) OutString = UserDB._List.First(x => x == ThisData.GetValue(this)).Name;
                                else if (temp == typeof(Database.GroupDB.Group.Rank)) OutString = this.Rank.Name;
                                else if (temp == typeof(Database.PermissionDB.Permission)) continue; //Saved Later.
                                else if (temp == typeof(DateTime)) OutString = ((DateTime)(ThisData.GetValue(this))).ToCommonString();
                                else if (temp == typeof(TimeSpan)) OutString = ThisData.GetValue(this).ToString();
                                else
                                {
                                    OutString = ThisData.GetValue(this).ToString();
                                }
                                //Logger.Console.WriteLine(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                                Output.Add(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                            }
                            catch (Exception e)
                            {
                                Logger.Console.WriteLine("&cError&e " + e);
                                Logger.Console.WriteLine("&eError Saving Value for Group Reference: \"" + Group.Name + "\" Value: \"" + ThisData.Name + "\".");
                            }
                        }
                        File.WriteAllLines(OutputFile, Output.ToList());
                    }
                    public void Save(string Key)
                    {
                        bool found = false;
                        string OutputFile = "./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name + "/Info.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Users/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name);
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/");
                        Utilities.IO.PrepareDirectory("./Database/Users/" + Parent.Name + "/GROUPS/" + Group.Name);
                        Utilities.IO.PrepareFile(OutputFile);
                        string[] OutputFileContents = Utilities.IO.ReadAllLines(OutputFile);
                        string OutTime = Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(DateTime.Now));
                        List<String> Output = new List<String>();
                        foreach (string line in OutputFileContents)
                        {
                            #region GetDataCouplets
                            string ThisLine = line;
                            if (!(ThisLine.Contains("\t")))
                            {
                                Output.Add(line);
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
                            if (Key.ToUpperInvariant() != Header.ToUpperInvariant())
                            {
                                Output.Add(line);
                                continue;
                            }
                            switch (Header.ToUpperInvariant())
                            {
                                #region Switches
                                case Strings.Group:
                                    Output.Add(Strings.Group + "\t\t" + Group.Name);
                                    found = true;
                                    break;
                                case Strings.Rank:
                                    Output.Add(Strings.Rank + "\t\t" + Rank.Name);
                                    found = true;
                                    break;
                                case Strings.RankReason:
                                    Output.Add(Strings.RankReason + "\t\t" + RankReason);
                                    found = true;
                                    break;
                                case Strings.RankedBy:
                                    Output.Add(Strings.RankedBy + "\t\t" + RankedBy.Name);
                                    found = true;
                                    break;
                                case Strings.PreviousRank:
                                    Output.Add(Strings.PreviousRank + "\t\t" + PreviousRank.Name);
                                    found = true;
                                    break;
                                case Strings.RankDate:
                                    Output.Add(Strings.RankDate + "\t\t" + RankDate.ToString());
                                    found = true;
                                    break;
                                #endregion
                                default:
                                    Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header));
                                    //Unrecognised Value.
                                    break;
                            }
                        }
                        if (!found)
                        {
                            switch (Key)
                            {
                                #region Switches
                                case Strings.Group:
                                    Output.Add(Strings.Group + "\t\t" + Group.Name);
                                    found = true;
                                    break;
                                case Strings.Rank:
                                    Output.Add(Strings.Rank + "\t\t" + Rank.Name);
                                    found = true;
                                    break;
                                case Strings.RankReason:
                                    Output.Add(Strings.RankReason + "\t\t" + RankReason);
                                    found = true;
                                    break;
                                case Strings.RankedBy:
                                    Output.Add(Strings.RankedBy + "\t\t" + RankedBy.Name);
                                    found = true;
                                    break;
                                case Strings.PreviousRank:
                                    Output.Add(Strings.PreviousRank + "\t\t" + PreviousRank.Name);
                                    found = true;
                                    break;
                                case Strings.RankDate:
                                    Output.Add(Strings.RankDate + "\t\t" + RankDate.ToString());
                                    found = true;
                                    break;
                                #endregion
                                default:
                                    Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                                    //Unrecognised Value.
                                    break;
                            }
                        }
                        File.WriteAllLines(OutputFile, Output.ToList());
                    }
                }
            }
        }
    }
}