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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Users_Demote = new CommandDescriptor
        {
            _Name = "Demote User In Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Demotes a user in a group on the server.",
            _Usage = "Usage: /Group.<Name>.Demote <Username> [RankName]",
            _Commands = new string[] { "/Groups.*.Demote" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Users_Demote_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Users_Demote_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.UserDB.User TargetUser = Database.UserDB.Nobody;
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            Database.GroupDB.Group.Rank TargetRank = Database.GroupDB.NoRank;
            Database.GroupDB.Group.Rank CurrentRank = Database.GroupDB.NoRank;
            int CurrentRankInt;
            int TargetRankInt;
            string Reason = "";
            #region FindTargetUser
            if (Command._CmdArguments.Count() < 1)
            {
                NetObj.ClientObject.SendMessage("No user specified to demote.");
                return false;
            }
            if (Command._CmdArguments[0] == "-")
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
            #endregion
            #region FindTargetGroup
            if (Command._CmdElements()[1] == "-")
            {
                if (NetObj.CommandHandling.PreviousGroup == Database.GroupDB.NoGroup)
                {
                    NetObj.ClientObject.SendMessage("No previous groups iterated over.");
                    return false;
                }
                else TargetGroup = NetObj.CommandHandling.PreviousGroup;
            }
            else
            {
                if (Database.GroupDB.FindGroup(Command._CmdElements()[1]) == Database.GroupDB.NoGroup)
                {
                    NetObj.ClientObject.SendMessage("Group not found: \"" + Command._CmdElements()[1] + "\".");
                    return false;
                }
                TargetGroup = Database.GroupDB.FindGroup(Command._CmdElements()[1]);
            }
            #endregion
            #region TargetIsInGroup?
            if (!(TargetUser.Groups.Select(x => x.Group).Contains(TargetGroup)))
            {
                //User NOT in the group.
                NetObj.ClientObject.SendMessage("User \"" + TargetUser.Name + "\" is not a member of Group \"" + TargetGroup.Name + "\". They cannot be promoted without being a member first.");
                return false;
            }
            CurrentRank = TargetUser.Groups.First(x => x.Group == TargetGroup).Rank;
            CurrentRankInt = TargetGroup.Ranks.IndexOf(TargetUser.Groups.First(y => y.Group == TargetGroup).Rank);
            #endregion
            #region FindTargetRank
            if (Command._CmdArguments.Count() < 2)
            {
                //TargetUser.Groups.First(x => x.Group == TargetGroup).Group.Ranks.IndexOf(TargetUser.Groups.First(x => x.Group == TargetGroup).Rank); //USERS CURRENT RANK;
                //Logger.Console.WriteLine(CurrentRankInt.ToString());
                //Logger.Console.WriteLine((TargetUser.Groups.First(x => x.Group == TargetGroup).Group.Ranks.Count() - 1).ToString());

                if (CurrentRankInt <= 0)
                {
                    //Users current rank IS the lowest rank, cannot demote any lower.
                    TargetRank = Database.GroupDB.NoRank;
                    NetObj.ClientObject.SendMessage("User \"" + TargetUser.Name + "\" is already the lowest rank of Group \"" + TargetGroup.Name + "\". They cannot be demoted any lower.");
                    return false;
                }
                else
                {
                    //There is a rank below this one, set that as the target.
                    TargetRank = TargetUser.Groups.First(x => x.Group == TargetGroup).Group.Ranks[CurrentRankInt - 1];
                    TargetRankInt = CurrentRankInt - 1;
                }
            }
            else
            {
                if (TargetGroup.FindRank(Command._CmdArguments[1]) == Database.GroupDB.NoRank)
                {
                    NetObj.ClientObject.SendMessage("Rank \"" + Command._CmdArguments[1] + "\" not found in Group: \"" + TargetGroup.Name + "\".");
                    return false;
                }
                TargetRank = TargetGroup.FindRank(Command._CmdArguments[1]);
                TargetRankInt = TargetGroup.Ranks.IndexOf(TargetRank);
            }
            if (TargetRank == Database.GroupDB.NoRank)
            {
                NetObj.ClientObject.SendMessage("Rank \"" + Command._CmdArguments[1] + "\" not found in Group: \"" + TargetGroup.Name + "\".");
                return false;
            }
            if (TargetRankInt > CurrentRankInt)
            {
                NetObj.ClientObject.SendMessage("Rank: \"" + TargetRank.Name + "\" is higher then Rank: \"" + CurrentRank.Name + "\" in Group: \"" + TargetGroup.Name + "\". Use the /Promote Command instead.");
                return false;
            }
            #endregion
            #region RankReason?
            if (Command._CmdArguments.Count() >= 3)
            {
                Reason = Command._CmdRawArguments.Split(new string[] {" "}, 3, StringSplitOptions.None)[2];
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject == TargetGroup.Founder)
            {
                //continue
            }
            #endregion
            #region Standard Testing
            else
            {
                if (NetObj.UserObject.Groups.Select(x => x.Group).Contains(TargetGroup))
                {
                    //THIS User is a member of the group. (Required to be able to modify the group.
                    Database.UserDB.User.GroupReference ThisGroupReference = NetObj.UserObject.Groups.First(x => x.Group == TargetGroup);
                    if (ThisGroupReference.Rank.Permissions.Group_ManageMembers <= 0)
                    {
                        NetObj.ClientObject.SendMessage("Your rank does not have permission to manage group memebers in this group.");
                        return false;
                    }
                    if (ThisGroupReference.Rank.Permissions.Group_DemoteableRank < 0)
                    {
                        NetObj.ClientObject.SendMessage("You are not able to demote members in the group as you do not have permission to demote from the lowest rank in the group.");
                        return false;
                    }
                    if (ThisGroupReference.Rank.Permissions.Group_DemoteableRank < CurrentRankInt) //<0 No Member.
                    {
                        NetObj.ClientObject.SendMessage("You are not able to demote User: \"" + TargetUser.Name + "\" as your Rank: \"" + NetObj.UserObject.Groups.First(x => x.Group == TargetGroup).Rank.Name + "\" is unable to demote Rank: \"" + TargetRank.Name + "\" in Group: \"" + TargetGroup.Name + "\".");
                        return false;
                    }
                    //continue
                }
                else
                {
                    NetObj.ClientObject.SendMessage("You need to be a member of the group yourself in order to demote members. (Group founders are excepted from this rule.)");
                    return false;
                }
            }
            #endregion
            Orb_Command_Maintenence_Group_Users_Rank_Common_Method(NetObj, TargetUser, TargetGroup, TargetRank, Reason);
            return true;
        }
    }
}