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
                public partial class Rank
                {
                    public void SaveAll(string GroupName, string RankName)
                    {
                        string OutputFile = "./Database/Groups/" + GroupName + "/RANKS/" + RankName + "/Info.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName);
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName + "/RANKS/" + RankName);
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
                                else if (temp == typeof(List<Rank>)) continue;
                                else if (temp == typeof(Database.GroupDB.Group.Rank)) continue;
                                else if (temp == typeof(Database.PermissionDB.Permission)) continue; //Saved Later.
                                else if (temp == typeof(DateTime)) OutString = ((DateTime)(ThisData.GetValue(this))).ToCommonString();
                                else if (temp == typeof(TimeSpan)) OutString = ThisData.GetValue(this).ToString();
                                else OutString = ThisData.GetValue(this).ToString();
                                //Logger.Console.WriteLine(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                                Output.Add(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                            }
                            catch (Exception e)
                            {
                                Logger.Console.WriteLine("&eError Saving Value for Group: \"" + Name + "\" Value: \"" + ThisData.Name);
                            }
                        }
                        #region DECREPTED
                        /*foreach (var ThisData in typeof(Strings).GetFields().ToList())
                        {
                            switch (ThisData.GetValue(null).ToString())
                            {
                                #region Switches
                                case Strings.Name:
                                    Output.Add(Strings.Name + "\t\t" + Name);
                                    break;
                                case Strings.DisplayedName:
                                    Output.Add(Strings.DisplayedName + "\t\t" + DisplayedName.ToString());
                                    break;
                                #endregion
                                default:
                                    Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", ThisData.GetValue(null).ToString()));
                                    //Unrecognised Value.
                                    break;
                            }
                        }*/
                        #endregion
                        File.WriteAllLines(OutputFile, Output.ToList());
                        //Save All Permissions for this rank.
                        //  As permissions in it's own right is a suboject,
                        //  Capabable of it's own specific Load() one var only,
                        //  we can do that instead if a specific permission needs to be saved.
                        OutputFile = "./Database/Groups/" + GroupName + "/RANKS/" + RankName + "/Permissions.Dat";
                        Utilities.IO.PrepareDirectory("./Database/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/");
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName);
                        Utilities.IO.PrepareDirectory("./Database/Groups/" + GroupName + "/RANKS/" + RankName);
                        Utilities.IO.PrepareFile(OutputFile);
                        this.Permissions.SaveAll(OutputFile);

                    }
                }
            }
        }
    }
}