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
        public static partial class GroupDB
        {
            public partial class Group
            {
                public void SaveAll()
                {
                    string OutputFile = "./Database/Groups/" + Name + "/Info.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Groups/");
                    Utilities.IO.PrepareDirectory("./Database/Groups/" + Name);
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
                            else if (temp == typeof(List<Rank>))
                            {
                                foreach (Database.GroupDB.Group.Rank ThisRank in this.Ranks.ToArray().Reverse())
                                {
                                    if (ThisRank == NoRank) continue;
                                    Output.Add("RANK".SuffixTabs(4) + ThisRank.Name);
                                    //Logger.Console.WriteLine("RANK".SuffixTabs(4) + ThisRank.Name);
                                    ThisRank.SaveAll(Name, ThisRank.Name);
                                }
                                continue;
                            }
                            else if (temp == typeof(Database.GroupDB.Group.Rank)) continue;
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
                            Logger.Console.WriteLine("&eError Saving Value for Group: \"" + Name + "\" Value: \"" + ThisData.Name + "\".");
                        }
                            #region Decrepted
                            /*
                            case Strings.DisplayedName:
                                Output.Add(Strings.DisplayedName + "\t\t" + DisplayedName.ToString());
                                break;
                            case Strings.DateCreated:
                                Output.Add(Strings.DateCreated + "\t\t" + DateCreated.ToString());
                                break;
                            case Strings.DateLastModified:
                                Output.Add(Strings.DateLastModified + "\t" + DateLastModified.ToString());
                                break;
                            case Strings.Joinable:
                                Output.Add(Strings.Joinable + "\t\t" + Joinable.ToString());
                                break;
                            case Strings.Leavable:
                                Output.Add(Strings.Leavable + "\t\t" + Leavable.ToString());
                                break;
                            case Strings.Founder:
                                Output.Add(Strings.Founder + "\t\t\t" + Founder.Name.ToString());
                                break;
                            case Strings.Rank:
                                foreach (Rank ThisRank in Ranks)
                                {
                                    Output.Add(Strings.Rank + "\t\t\t" + ThisRank.Name.ToString());
                                    ThisRank.SaveAll(Name, ThisRank.Name);
                                }
                                break;
                            #endregion
                            default:
                                Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", ThisData.GetValue(null).ToString()));
                                //Unrecognised Value.
                                break;
                            */
                            #endregion
                    }
                    File.WriteAllLines(OutputFile, Output.ToList());
                    //Save All Permissions for this group.
                    //  As permissions in it's own right is a suboject,
                    //  Capabable of it's own specific Save() one var only,
                    //  we can do that instead if a specific permission needs to be saved.
                    OutputFile = "./Database/Groups/" + Name + "/Permissions.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Groups/");
                    Utilities.IO.PrepareDirectory("./Database/Groups/" + Name);
                    Utilities.IO.PrepareFile(OutputFile);
                    this.Permissions.SaveAll(OutputFile);
                }
            }
        }
    }
}