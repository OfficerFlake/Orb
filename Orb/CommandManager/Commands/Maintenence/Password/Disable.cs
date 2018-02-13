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
        public static readonly CommandDescriptor Orb_Command_Maintenence_User_Password_Disable = new CommandDescriptor
        {
            _Name = "Disable Password",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Disables your log in password.",
            _Usage = "Usage: /Password.Enable",
            _Commands = new string[] { "/Password.Disable" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_User_Password_Disable_Method,
        };

        public static bool Orb_Command_Maintenence_User_Password_Disable_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("Password Authentication Disabled. You can no longer login with your password as a fallback if you need to authenticate yourself.");
            NetObj.UserObject.UsePassword = false;
            NetObj.UserObject.SaveAll();
            return true;
        }
    }
}