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
                public void SaveAll(string Location)
                {
                    if (Location.EndsWith("Permissions.Dat")) Location = Location.Remove(Location.Length - 15);
                    if (Location.EndsWith("/")) Location = Location.Remove(Location.Length - 1);
                    string OutputFile = Location + "/Permissions.Dat";
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
                            else if (temp == typeof(Database.GroupDB.Group.Rank)) continue;
                            else if (temp == typeof(Database.PermissionDB.Permission)) continue; //Saved Later.
                            else if (temp == typeof(DateTime)) OutString = ((DateTime)(ThisData.GetValue(this))).ToCommonString();
                            else if (temp == typeof(TimeSpan)) OutString = ThisData.GetValue(this).ToString();
                            else if (temp == typeof(Double))
                            {
                                if ((Double)ThisData.GetValue(this) == Double.PositiveInfinity) OutString = "True";
                                else if ((Double)ThisData.GetValue(this) == Double.NegativeInfinity) OutString = "False";
                                else if ((Double)ThisData.GetValue(this) == Double.NaN) OutString = "NaN";
                                else OutString = ThisData.GetValue(this).ToString();
                            }
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
                            Logger.Console.WriteLine("&eError Saving Permission: \"" + ThisData.Name + "\".");
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
                }
            }
        }
    }
}