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
        public static readonly CommandDescriptor Orb_Command_Information_Server = new CommandDescriptor
        {
            _Name = "Server Command List",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "List the Maintenence Commands available on the server.",
            _Usage = "Usage: /Commands.Server",
            _Commands = new string[] { "/Commands.Server" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_Server_Method,
        };

        public static bool Orb_Command_Information_Server_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("&eServer Commands:");
            NetObj.ClientObject.SendMessage("&a    /Wind (XXXYYKT)&e              Changes wind to new x y z values");
            NetObj.ClientObject.SendMessage("&a    /Fog (Range)&e                 Changes the fo visibility");
            NetObj.ClientObject.SendMessage("&a    /Apocolypse&e                  Severly Warps the ingame lighting (&cPHOTOSENSITIVE SHOCK WARNING!&e)");
            NetObj.ClientObject.SendMessage("&a    /BlackOut [ON|OFF]&e           Enables/Disables gForce Blackout");
            NetObj.ClientObject.SendMessage("&a    /LandEverywhere [ON|OFF]&e     Enables/Disables Land Everywhere");
            NetObj.ClientObject.SendMessage("&a    /Collisions [ON|OFF]&e         Enables/Disables aircraft collisions");
            return true;
        }
    }
}