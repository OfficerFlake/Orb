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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Users_Remove = new CommandDescriptor
        {
            _Name = "Remove User From Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Remove a user from a group on the server.",
            _Usage = "Usage: /Group.<Name>.Remove <Username>",
            _Commands = new string[] { "/Groups.*.Remove" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Users_Remove_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Users_Remove_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.UserDB.User TargetUser = Database.UserDB.Nobody;
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            #region FindTargetUser
            if (Command._CmdArguments.Count() < 1)
            {
                NetObj.ClientObject.SendMessage("No user specified to change group ownership to.");
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
                    Database.UserDB.User.GroupReference ThisGroupReference = NetObj.UserObject.Groups.Where(x => x.Group == TargetGroup).ToArray()[0];
                    if (ThisGroupReference.Rank.Permissions.Group_ManageMembers <= 0)
                    {
                        NetObj.ClientObject.SendMessage("Your rank does not have permission to manage group memebers in this group.");
                        return false;
                    }
                    if (ThisGroupReference.Rank.Permissions.Group_DemoteableRank < 0) //<0 No Member.
                    {
                        NetObj.ClientObject.SendMessage("You are not able to remove members from the group as you do not have permission to demote to the lowest rank in the group.");
                        return false;
                    }
                    //continue
                }
                else
                {
                    NetObj.ClientObject.SendMessage("You need to be a member of the group yourself in order to remove members. (Group founders are excepted from this rule.)");
                    return false;
                }
            }
            #endregion

            if (TargetGroup.Leavable == false)
            {
                NetObj.ClientObject.SendMessage("Failed to remove User: \"" + TargetUser.Name + "\" from Group: \"" + TargetGroup.Name + "\". The Group is set to un-leaveable.");
                return false;
            }
            if (!(TargetUser.Groups.Select(x => x.Group).Contains(TargetGroup)))
            {
                NetObj.ClientObject.SendMessage("User: \"" + TargetUser.Name + "\" is already not a member of Group: \"" + TargetGroup.Name + "\".");
                return false;
            }
            TargetUser.RemoveFromGroup(TargetGroup);
            TargetUser.SaveAll();
            Server.EmptyClientList.Include(TargetUser).SendMessage("You were removed from Group: \"" + TargetGroup.Name + "\" by \"" + NetObj.UserObject.Name + "\".");
            Server.AllClients.Except(TargetUser).SendMessage("User: \"" + TargetUser.Name + "\" was removed from Group: \"" + TargetGroup.Name + "\" by \"" + NetObj.UserObject.Name + "\".");
            return true;
        }
    }
}