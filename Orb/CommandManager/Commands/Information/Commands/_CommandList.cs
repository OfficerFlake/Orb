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
        public static readonly CommandDescriptor Orb_Command_Information_List = new CommandDescriptor
        {
            _Name = "Command List",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "List the Commands available on the server.",
            _Usage = "Usage: /Commands",
            _Commands = new string[] { "/Commands" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_List_Method,
        };

        public static bool Orb_Command_Information_List_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("&eCommand Catagories:");
            NetObj.ClientObject.SendMessage("&a    /Commands.Chat");
            NetObj.ClientObject.SendMessage("&a    /Commands.Flight");
            NetObj.ClientObject.SendMessage("&a    /Commands.Moderation");
            NetObj.ClientObject.SendMessage("&a    /Commands.Maintenence");
            NetObj.ClientObject.SendMessage("&a    /Commands.Server");
            return true;
        }
    }
}