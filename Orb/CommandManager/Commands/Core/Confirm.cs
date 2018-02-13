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
        public static readonly CommandDescriptor Orb_Command_Core_Confirm = new CommandDescriptor
        {
            _Name = "Confirm",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Confirms a command or action as requested by the server.",
            _Usage = "Usage: /OK",
            _Commands = new string[] { "/OK", "/YES" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Core_Confirm_Method,
        };

        public static bool Orb_Command_Core_Confirm_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.CommandConfirmation.Set();
            return true;
        }
    }
}