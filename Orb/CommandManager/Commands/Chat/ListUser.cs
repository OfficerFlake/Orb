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
        public static readonly CommandDescriptor Orb_Command_Chat_ListUser = new CommandDescriptor
        {
            _Name = "List Users",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Shows a list of online players.",
            _Usage = "Usage: /ListUser",
            _Commands = new string[] { "/ListUser", "/User", "/Users" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Chat_ListUser_Method,
        };

        public static bool Orb_Command_Chat_ListUser_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region ListUser
            NetObj.ClientObject.SendMessage("&aList of Users Online:    &e" + Server.ClientList.Select(x => x.Username).ToList().ToStringList());
            return true;
            #endregion
        }
    }
}