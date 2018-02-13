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
        public static readonly CommandDescriptor Orb_Command_Information_Group_Info = new CommandDescriptor
        {
            _Name = "Group Info",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Shows information for a specific group.",
            _Usage = "Usage: /Group.<Name>.Info",
            _Commands = new string[] { "/Group.*.Info", "/Groups.*.Info" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_Group_Info_Method,
        };

        public static bool Orb_Command_Information_Group_Info_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            #region FindTargetGroup
            if (Command._CmdElements()[0].ToUpperInvariant() == "GROUP" || Command._CmdElements()[0].ToUpperInvariant() == "GROUPS")
            {
                //    /GROUP.*

                if (Database.GroupDB.FindGroup(Command._CmdElements()[1]) != Database.GroupDB.NoGroup)
                {
                    TargetGroup = Database.GroupDB.FindGroup(Command._CmdElements()[1]);
                }
                else
                {
                    NetObj.ClientObject.SendMessage("Group not found: \"" + Command._CmdElements()[1] + "\".");
                    return false;
                }
            }
            else
            {
                //     /INFO *
                if (Command._CmdArguments.Count() < 1)
                {
                    TargetGroup = NetObj.UserObject.GroupRepresented;
                }
                else if (Command._CmdArguments[0] == "-")
                {
                    if (NetObj.CommandHandling.PreviousGroup == Database.GroupDB.NoGroup)
                    {
                        NetObj.ClientObject.SendMessage("No previous groups iterated over.");
                        return false;
                    }
                    else
                    {
                        TargetGroup = NetObj.CommandHandling.PreviousGroup;
                    }
                }
                else
                {
                    if (Database.GroupDB.FindGroup(Command._CmdArguments[0]) != Database.GroupDB.NoGroup)
                    {
                        TargetGroup = Database.GroupDB.FindGroup(Command._CmdArguments[0]);
                    }
                    else
                    {
                        NetObj.ClientObject.SendMessage("Group not found: \"" + Command._CmdArguments[0] + "\".");
                        return false;
                    }
                }
            }
            #endregion

            #region Info
            NetObj.ClientObject.SendMessage("&eAbout Group \"" + TargetGroup.Name + "\":");
            string[] CreationDate = Utilities.DateTimeUtilities.FormatDateTime(TargetGroup.DateCreated);
            string[] ModofiedDate = Utilities.DateTimeUtilities.FormatDateTime(TargetGroup.DateLastModified);
            NetObj.ClientObject.SendMessage("&e    Created:&f " + (CreationDate[2] + "/" + CreationDate[1] + "/" + CreationDate[0].Slice(2, 4)).SuffixLimit(15) + " " +
                                            "&eModified:&f " + (CreationDate[2] + "/" + CreationDate[1] + "/" + CreationDate[0].Slice(2, 4)).SuffixLimit(15) + " " +
                                            "&eMembers:&f " + Database.UserDB.List.Where(x=>x.Groups.Select(y=>y.Group).Contains(TargetGroup)).Count().ToString().SuffixLimit(15));

            NetObj.ClientObject.SendMessage("&e    Founder:&f " + TargetGroup.Founder.Name.Slice(0, 15).SuffixLimit(15) + " " +
                                            "&eJoinable:&f " + TargetGroup.Joinable.ToString().SuffixLimit(15) + " " +
                                            "&eLeavable:&f " + TargetGroup.Leavable.ToString().SuffixLimit(15));

            #region Ranks
            if (TargetGroup.Ranks.Count() > 12)
            {
                NetObj.ClientObject.SendMessage("&e    Contains " + TargetGroup.Ranks.Count() + " Ranks.");
            }
            else if (TargetGroup.Ranks.Count() > 0)
            {
                NetObj.ClientObject.SendMessage("&e");
                NetObj.ClientObject.SendMessage("&e    Ranks:");
                string output = "";
                int repeats = 0;
                foreach (Database.GroupDB.Group.Rank ThisRank in TargetGroup.Ranks)
                {
                    string shortrankname = ThisRank.Name;
                    if (shortrankname.Length > 7) shortrankname = shortrankname.Substring(0, 7);
                    else
                    {
                        shortrankname = shortrankname + new string(' ', 7 - shortrankname.Length);
                    }

                    int ThisCount = Database.UserDB.List.Where(x => x.Groups.Select(y => y.Rank).Contains(ThisRank)).Count();
                    string ThisCount2 = ThisCount.ToString();
                    while (ThisCount2.Length < 2)
                    {
                        ThisCount2 = " " + ThisCount2;
                    }
                    if (ThisCount2.Length > 2)
                    {
                        ThisCount2 = ">99";
                    }
                    else
                    {
                        ThisCount2 = " " + ThisCount2;
                    }

                    if ((repeats + 1) < 3)
                    {
                        if (Database.UserDB.List.Where(x => x.Groups.Select(y => y.Group).Contains(TargetGroup)).Count() > 0) output += "    &e" + ThisCount2 + "&f " + shortrankname;
                        else output += "        " + shortrankname;

                        repeats++;
                    }
                    else
                    {
                        if (Database.UserDB.List.Where(x => x.Groups.Select(y => y.Group).Contains(TargetGroup)).Count() > 0) output += "    &e" + ThisCount2 + "&f " + shortrankname;
                        else output += "        " + shortrankname;

                        NetObj.ClientObject.SendMessage("&f" + output);
                        output = "";
                        repeats = 0;
                    }
                }
                if (repeats > 0)
                {
                    NetObj.ClientObject.SendMessage("&f" + output);
                    output = "";
                    repeats = 0;
                }
            }
            return true;
            #endregion
            #endregion
        }
    }
}