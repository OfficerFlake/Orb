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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Owner = new CommandDescriptor
        {
            _Name = "Owner Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Change the owner of a group on the server.",
            _Usage = "Usage: /Group.<Name>.Owner",
            _Commands = new string[] { "/Groups.*.Owner" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Owner_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Owner_Method(Server.NetObject NetObj, CommandReader Command)
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
                //Continue.
            }
            #endregion
            #region Standard Testing
            else
            {
                NetObj.ClientObject.SendMessage("Only Group Owners are able to change ownership of the group. Please contact " + TargetGroup.Founder.Name + " if you wish to take over this group.");
                return false;
            }
            #endregion

            if (NetObj.UserObject == TargetUser)
            {
                NetObj.ClientObject.SendMessage("You already ARE the founder of this group!");
                return false;
            }
            else
            {
                NetObj.ClientObject.SendMessage("You are about to relinquish ownership of Group: \"" + TargetGroup.Name + "\" to \"" + TargetUser.Name + "\".");
                NetObj.ClientObject.SendMessage("THIS IS NOT REVERSABLE!");
                NetObj.ClientObject.SendMessage("Please confirm this action by typing \"/OK\".");
                if (!NetObj.GetCommandConfirmation(5000))
                {
                    NetObj.ClientObject.SendMessage("No response, command cancelled.");
                    return false;
                }
            }
            TargetGroup.Founder = TargetUser;
            TargetGroup.SaveAll();
            NetObj.ClientObject.SendMessage("You have relinquished ownership of Group: \"" + TargetGroup.Name + "\" to \"" + TargetUser.Name + "\".");
            Server.EmptyClientList.Include(TargetUser).SendMessage("You have been assigned ownership of Group: \"" + TargetGroup.Name + "\" by \"" + NetObj.UserObject.Name + "\".");
            Server.AllClients.Except(TargetUser).SendMessage("User: \"" + TargetUser.Name + "\" was assigned ownership of Group: \"" + TargetGroup.Name + "\" by \"" + NetObj.UserObject.Name + "\".");
            return true;
        }
    }
}