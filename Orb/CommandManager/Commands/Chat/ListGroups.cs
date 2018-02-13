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
        public static readonly CommandDescriptor Orb_Command_Chat_ListGroups = new CommandDescriptor
        {
            _Name = "List Groups",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Shows a list of server groups.",
            _Usage = "Usage: /ListGroups",
            _Commands = new string[] { "/ListGroups", "/Group", "/Groups" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Chat_ListGroups_Method,
        };

        public static bool Orb_Command_Chat_ListGroups_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region ListGroup
            NetObj.ClientObject.SendMessage("&aList of Server Groups:    &e" + Database.GroupDB.ListToString(Database.GroupDB.List));
            return true;
            #endregion
        }
    }
}