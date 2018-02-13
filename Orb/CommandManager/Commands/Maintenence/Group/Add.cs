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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Add = new CommandDescriptor
        {
            _Name = "Add Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Adds a group to the server.",
            _Usage = "Usage: /Group.Add <Name>",
            _Commands = new string[] { "/Group.Add", "/Groups.Add" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Add_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Add_Method(Server.NetObject NetObj, CommandReader Command)
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
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject.Can(Database.PermissionDB.Strings.ManageServer))
            {
                //Continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("Unable to add new Group: \"" + NewGroupName + "\" as you do not have permission to manage the server.");
                return false;
            }
            #endregion

            #region Add Group
            Database.GroupDB.Group NewGroup = Database.GroupDB.New(NewGroupName);
            NewGroup.DateCreated = DateTime.Now;
            NewGroup.DateLastModified = DateTime.Now;
            NewGroup.DisplayedName = NewGroupName;
            NewGroup.Founder = NetObj.UserObject;
            NewGroup.Joinable = true;
            NewGroup.Leavable = true;
            NewGroup.Name = NewGroupName;
            NewGroup.SaveAll();
            Server.EmptyClientList.Include(NetObj.UserObject).SendMessage("You created a new Group: \"" + NewGroup.Name + "\".");
            Server.AllClients.Except(NetObj.UserObject).SendMessage("User: \"" + NetObj.UserObject.Name + "\" created a new Group: \"" + NewGroup.Name + "\".");
            return true;
            #endregion
        }
    }
}