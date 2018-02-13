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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Represent = new CommandDescriptor
        {
            _Name = "Represent Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Represent a group on the server.",
            _Usage = "Usage: /Group.<Name>.Represent",
            _Commands = new string[] { "/Groups.*.Represent" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Represent_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Represent_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            #region FindTargetGroup
            if (Database.GroupDB.FindGroup(Command._CmdElements()[1]) == Database.GroupDB.NoGroup)
            {
                NetObj.ClientObject.SendMessage("Group not found: \"" + Command._CmdElements()[1] + "\".");
                return false;
            }
            TargetGroup = Database.GroupDB.FindGroup(Command._CmdElements()[1]);
            #endregion
            #region Test Permissions
            if (NetObj.UserObject.Groups.Select(x => x.Group).Contains(TargetGroup))
            {
                //THIS User is a member of the group. (Required to be able to represent the group.
            }
            else
            {
                NetObj.ClientObject.SendMessage("You need to be a member of the group yourself in order to represent it.");
                return false;
            }
            #endregion
            NetObj.UserObject.GroupRepresented = TargetGroup;
            NetObj.ClientObject.SendMessage("You are now representing Group: \"" + TargetGroup.Name + "\".");
            NetObj.UserObject.SaveAll();
            return true;
        }
    }
}