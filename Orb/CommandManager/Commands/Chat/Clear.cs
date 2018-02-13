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
        public static readonly CommandDescriptor Orb_Command_Chat_Clear = new CommandDescriptor
        {
            _Name = "Clear",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Clears the console",
            _Usage = "Usage: /Clear",
            _Commands = new string[] { "/Clear" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Chat_Clear_Method,
        };

        public static bool Orb_Command_Chat_Clear_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region Spam
            if (NetObj.UserObject.Muted)
            {
                NetObj.UserObject.MuteNotifier();
                return false;
            }
            if (NetObj.UserObject.Can(Database.PermissionDB.Strings.Say))
            {
                System.Console.Clear();
                return true;
            }
            else
            {
                NetObj.ClientObject.SendMessage("You do not have enough permission to \"Clear\".");
                return false;
            }
            #endregion
        }
    }
}