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
        public static readonly CommandDescriptor Orb_Command_Information_Moderation = new CommandDescriptor
        {
            _Name = "Moderation Command List",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "List the Maintenence Commands available on the server.",
            _Usage = "Usage: /Commands.Moderation",
            _Commands = new string[] { "/Commands.Moderation" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_Moderation_Method,
        };

        public static bool Orb_Command_Information_Moderation_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("&eModeration Commands:");
            NetObj.ClientObject.SendMessage("&a    /User.<Username>.Ban (Reason)&e                Bans a user from the server permanently.");
            NetObj.ClientObject.SendMessage("&a    /User.<Username>.Ban (Duration) (Reason)&e     Bans a user from the server for a duration.");
            NetObj.ClientObject.SendMessage("&a    /User.<Username>.Mute (Reason)&e               Mutes a user on the server permanently.");
            NetObj.ClientObject.SendMessage("&a    /User.<Username>.Mute (Duration) (Reason)&e    Mutes a user on the server for a duration.");
            NetObj.ClientObject.SendMessage("&a    /User.<Username>.Kick (Reason)&e               Kicks a user from the server.");
            return true;
        }
    }
}