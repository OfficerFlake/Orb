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
        public static readonly CommandDescriptor Orb_Command_Information_Chat = new CommandDescriptor
        {
            _Name = "Chat Command List",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "List the Chat Commands available on the server.",
            _Usage = "Usage: /Commands.Chat",
            _Commands = new string[] { "/Commands.Chat" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_Chat_Method,
        };

        public static bool Orb_Command_Information_Chat_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("&eChat Commands:");
            NetObj.ClientObject.SendMessage("&a    /Users&e:             Show who is online.");
            NetObj.ClientObject.SendMessage("&a    /Info (User)&e:       Show info for a user.");
            NetObj.ClientObject.SendMessage("&a    /Say&e:               Broadcast a message to the server.");
            NetObj.ClientObject.SendMessage("&a    /Password&e:          Changes your password.");
            NetObj.ClientObject.SendMessage("&a    /EnablePassword&e:    Enables your password.");
            NetObj.ClientObject.SendMessage("&a    /DisablePassword&e:   Disables your password.");
            NetObj.ClientObject.SendMessage("&b    @User&e:              Sends a Private Message to User");
            NetObj.ClientObject.SendMessage("&b    @User1,User2...&e:    Sends a Private Message to Multiple Users");
            NetObj.ClientObject.SendMessage("&b    @@Group&e:            Sends a Private Message to Group");
            NetObj.ClientObject.SendMessage("&b    @@Group1,Group2...&e: Sends a Private Message to Multiple Groups");
            return true;
        }
    }
}