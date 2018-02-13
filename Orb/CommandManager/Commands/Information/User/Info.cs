using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Commands
    {
        public static readonly CommandDescriptor Orb_Command_Information_User_Info = new CommandDescriptor
        {
            _Name = "User Info",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Shows information for a specific user.",
            _Usage = "Usage: /User.<Name>.Info",
            _Commands = new string[] { "/Info", "/User.*.Info", "/User.*", "/Users.*.Info", "/Users.*" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_User_Info_Method,
        };

        public static bool Orb_Command_Information_User_Info_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.UserDB.User TargetUser = Database.UserDB.Nobody;
            #region FindTargetUser
            if (Command._CmdElements()[0].ToUpperInvariant() == "/USER" | Command._CmdElements()[0].ToUpperInvariant() == "/USERS")
            {
                //    /USER.*

                if (Database.UserDB.Find(Command._CmdElements()[1]) != Database.UserDB.Nobody)
                {
                    TargetUser = Database.UserDB.Find(Command._CmdElements()[1]);
                }
                else
                {
                    NetObj.ClientObject.SendMessage("User not found: \"" + Command._CmdElements()[1] + "\".");
                    return false;
                }
            }
            else
            {
                //     /INFO *
                if (Command._CmdArguments.Count() < 1)
                {
                    TargetUser = NetObj.UserObject;
                }
                else if (Command._CmdArguments[0] == "-")
                {
                    if (NetObj.CommandHandling.PreviousUser == Database.UserDB.Nobody)
                    {
                        NetObj.ClientObject.SendMessage("No previous users iterated over.");
                        return false;
                    }
                    else
                    {
                        TargetUser = NetObj.CommandHandling.PreviousUser;
                    }
                }
                else
                {
                    if (Database.UserDB.Find(Command._CmdArguments[0]) != Database.UserDB.Nobody)
                    {
                        TargetUser = Database.UserDB.Find(Command._CmdArguments[0]);
                    }
                    else
                    {
                        NetObj.ClientObject.SendMessage("User not found: \"" + Command._CmdArguments[0] + "\".");
                        return false;
                    }
                }
            }
            #endregion

            #region DummyObjects
            if (TargetUser == Database.UserDB.Nobody)
            {
                NetObj.ClientObject.SendMessage("This is the dummy object used to protect Orb from crashing when database links break.");
                return true;
            }
            if (TargetUser == Database.UserDB.Connecting)
            {
                NetObj.ClientObject.SendMessage("This is the dummy object used to protect Orb from crashing when a user starts connecting.");
                return true;
            }
            if (TargetUser == Database.UserDB.SuperUser)
            {
                NetObj.ClientObject.SendMessage("This is the dummy object used to represent the Orb Console.");
                return true;
            }
            #endregion
            #region BasicInfo
            Server.NetObject.Client x = NetObj.ClientObject;
            string OnlineStatus = "&cOFFLINE";
            if (Server.ClientList.Select(y => y.UserObject).Contains(TargetUser)) OnlineStatus = "&aONLINE";

            string Status = "";
            if (TargetUser.IsMuted)
            {
                if (TargetUser.MuteExpires == new DateTime()) Status = "&e[&cMUTED&e]";
                else Status = "&e[&cMUTED UNTIL " + Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(TargetUser.MuteExpires)) + "&e]";
            }
            if (TargetUser.IsBanned)
            {
                if (TargetUser.BanExpires == new DateTime()) Status = "&e[&cBANNED&e].";
                else Status = "&e[&cBANNED UNTIL " + Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(TargetUser.BanExpires)) + "&e]";
            }
            if (OnlineStatus.EndsWith("ONLINE"))
            {
                x.SendMessage("&eAbout &f" + TargetUser.Name + "&e: " + OnlineStatus + " &e(Connected from " + TargetUser.LastIP.Mask() + ")." + Status);
            }
            else
            {
                x.SendMessage("&eAbout &f" + TargetUser.Name + "&e: " + OnlineStatus + " &e(Last connected from " + TargetUser.LastIP.Mask() + ")." + Status);
            }
            string[] JoinDate = Utilities.DateTimeUtilities.FormatDateTime(TargetUser.DateJoined);
            x.SendMessage("&e    JoinDate:  &f" + JoinDate[2] + "/" + JoinDate[1] + "/" + JoinDate[0].Slice(2, 4) + "         " +
                          "&e    Logins: &f" + TargetUser.LoginCount.To8BitString() + "");

            x.SendMessage("&e    Playtime:   &f" + Utilities.DateTimeUtilities.GetTotalHoursAs8BitStr(TargetUser.PlayTime) + "Hrs " +
                            "&e    FlightTime: &f" + Utilities.DateTimeUtilities.GetTotalHoursAs8BitStr(TargetUser.FlightHours) + "Hrs " +
                            "&e  TotalFlights: &f" + TargetUser.FlightsFlown.To8BitString());

            string KDR = "Perfect ";
            if (TargetUser.Deaths > 0)
            {
                KDR = "";
                KDR = (Math.Round((double)(TargetUser.Kills / TargetUser.Deaths), 3)).ToString();
                if (KDR.Length > 7)
                {
                    KDR = "Perfect ";
                }
            }
            // x.SendMessage("    TotalKills: " + TargetUser.Kills.To8BitString() + "    " +
            //                 "   TotalDeaths: " + TargetUser.Deaths.To8BitString() + "    " +
            //                 "    K:D Ratio:  " + KDR + "    ");

            //^^ NOT YET COMPLETE!
            #endregion
            #region Groups
            if (TargetUser.Groups.Count() > 12)
            {
                x.SendMessage("&e    Is a Member of " + TargetUser.Groups.Count() + " Groups.");
            }
            else if (TargetUser.Groups.Count() > 0)
            {
                x.SendMessage("&e");
                x.SendMessage("&e    Groups:");
                string output = "";
                int repeats = 0;
                foreach (Database.UserDB.User.GroupReference ThisGroupRef in TargetUser.Groups)
                {
                    string shortgroupname = ThisGroupRef.Group.Name;
                    if (shortgroupname.Length > 7) shortgroupname = shortgroupname.Substring(0, 7);
                    else
                    {
                        shortgroupname = shortgroupname + new string(' ', 7 - shortgroupname.Length);
                    }
                    string shortgrouprank = ThisGroupRef.Rank.Name;
                    if (shortgrouprank.Length > 7) shortgrouprank = shortgrouprank.Substring(0, 7);
                    else
                    {
                        shortgrouprank = shortgrouprank + new string(' ', 7 - shortgrouprank.Length);
                    }

                    if ((repeats + 1) < 3)
                    {
                        if (TargetUser.GroupRepresented == ThisGroupRef.Group) output += "      &e*&f " + shortgroupname + " " + shortgrouprank + " ";
                        else output += "        " + shortgroupname + " " + shortgrouprank + " ";

                        repeats++;
                    }
                    else
                    {
                        if (TargetUser.GroupRepresented == ThisGroupRef.Group) output += "      &e*&f " + shortgroupname + " " + shortgrouprank + " ";
                        else output += "        " + shortgroupname + " " + shortgrouprank + " ";

                        x.SendMessage("&f" + output);
                        output = "";
                        repeats = 0;
                    }
                }
                if (repeats > 0)
                {
                    x.SendMessage("&f" + output);
                    output = "";
                    repeats = 0;
                }
            }
            return true;
            #endregion
        }
    }
}