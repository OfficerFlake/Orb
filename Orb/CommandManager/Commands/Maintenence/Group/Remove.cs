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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Remove = new CommandDescriptor
        {
            _Name = "Remove Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Remove a group from the server.",
            _Usage = "Usage: /Groups.Remove <Name>",
            _Commands = new string[] { "/Groups.Remove" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Remove_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Remove_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            if (Command._CmdArguments.Count() < 1)
            {
                NetObj.ClientObject.SendMessage("Please specify a group name when adding a new group.");
                return false;
            }
            string NewGroupName = Command._CmdArguments[0];
            #region FindTargetGroup
            TargetGroup = Database.GroupDB.FindGroup(Command._CmdArguments[0]);
            if (TargetGroup != Database.GroupDB.NoGroup)
            {
                NetObj.ClientObject.SendMessage("Group: \"" + TargetGroup + "\" already exists. You cannot make duplicate groups.");
                return false;
            }
            #endregion
            #region Test Permissions
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject == TargetGroup.Founder)
            {
                //continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("Unable to remove Group: \"" + TargetGroup.Name + "\" as you do not have permission to manage the server.");
                return false;
            }
            #endregion

            #region Remove Group
            NetObj.ClientObject.SendMessage("You are about to remove Group: \"" + TargetGroup.Name + "\".");
            NetObj.ClientObject.SendMessage("THIS IS NOT REVERSABLE!");
            NetObj.ClientObject.SendMessage("Please confirm this action by typing \"/OK\".");
            if (!NetObj.GetCommandConfirmation(5000))
            {
                NetObj.ClientObject.SendMessage("No response, command cancelled.");
                return false;
            }
            string GroupName = TargetGroup.Name;
            Database.GroupDB.Delete(TargetGroup);
            Server.EmptyClientList.Include(NetObj.UserObject).SendMessage("You successfully removed Group: \"" + GroupName + "\".");
            Server.AllClients.Except(NetObj.UserObject).SendMessage("User: \"" + NetObj.UserObject.Name + "\" removed Group: \"" + GroupName + "\".");
            return true;
            #endregion
        }
    }
}