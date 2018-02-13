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
        public static readonly CommandDescriptor Orb_Command_Maintenence_User_Password_Enable = new CommandDescriptor
        {
            _Name = "Enable Password",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Enables your log in password.",
            _Usage = "Usage: /Password.Enable",
            _Commands = new string[] { "/Password.Enable" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_User_Password_Enable_Method,
        };

        public static bool Orb_Command_Maintenence_User_Password_Enable_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("Password Authentication Enabled. You can now login with your password as a fallback if you need to authenticate yourself.");
            NetObj.UserObject.UsePassword = true;
            NetObj.UserObject.SaveAll();
            return true;
        }
    }
}