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
        public static readonly CommandDescriptor Orb_Command_Information_Flight = new CommandDescriptor
        {
            _Name = "Flight Command List",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "List the Flight Commands available on the server.",
            _Usage = "Usage: /Commands.Flight",
            _Commands = new string[] { "/Commands.Flight" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_Flight_Method,
        };

        public static bool Orb_Command_Information_Flight_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("&eFlight Commands:");
            NetObj.ClientObject.SendMessage("&a    /Smoke R G B&e:             Changes your smoke color.");
            NetObj.ClientObject.SendMessage("&a    /Rainbow&e:                 Makes your smoke cycle through the hue spectrum.");
            return true;
        }
    }
}