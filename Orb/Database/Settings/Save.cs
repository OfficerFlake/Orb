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
        public static partial class Settings
        {
            public static void SaveAll()
            {
                string SettingsFile = "./Settings.Dat";
                Utilities.IO.PrepareFile(SettingsFile);
                string OutTime = Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(DateTime.Now));
                List<String> Output = new List<String>();
                Output.Add("REM AutoCreated by Orb Database Engine, " + OutTime + ".");
                foreach (var ThisData in typeof(Database.Settings).GetFields().ToList())
                {
                    try
                    {
                        string OutString = "";
                        Type temp = ThisData.FieldType;
                        if (temp == typeof(Database.GroupDB.Group)) OutString = GroupDB._List.First(x => x == ThisData.GetValue(typeof(Database.Settings))).Name;
                        else if (temp == typeof(Database.UserDB.User)) OutString = UserDB._List.First(x => x == ThisData.GetValue(typeof(Database.Settings))).Name;
                        else if (temp == typeof(Database.GroupDB.Group.Rank)) continue;
                        else if (temp == typeof(Database.PermissionDB.Permission)) continue; //Saved Later.
                        else if (temp == typeof(DateTime)) OutString = ((DateTime)(ThisData.GetValue(typeof(Database.Settings)))).ToCommonString();
                        else if (temp == typeof(TimeSpan)) OutString = ThisData.GetValue(typeof(Database.Settings)).ToString();
                        else
                        {
                            OutString = ThisData.GetValue(typeof(Database.Settings)).ToString();
                        }
                        //Logger.Console.WriteLine(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                        Output.Add(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e);
                        Logger.Console.WriteLine("&eError Saving Setting: \"" + ThisData.Name + "\".");
                    }
                }
                File.WriteAllLines(SettingsFile, Output.ToList());
            }
        }
    }
}