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
        public static readonly CommandDescriptor Orb_Command_Chat_Say = new CommandDescriptor
        {
            _Name = "Say",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Broadcasts a raw message to the server.",
            _Usage = "Usage: /Say <Message>",
            _Commands = new string[] { "/Say" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Chat_Say_Method,
        };

        public static bool Orb_Command_Chat_Say_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region Say
            if (NetObj.UserObject.Muted)
            {
                NetObj.UserObject.MuteNotifier();
                return false;
            }
            if (NetObj.UserObject.Can(Database.PermissionDB.Strings.Say))
            {
                Server.AllClients.Except(Server.OrbConsole).SendMessage(Command._CmdRawArguments);
                Logger.Console.WriteLine(NetObj.UserObject.DisplayedName + "&b(Say)&f: " + Command._CmdRawArguments);
            }
            else NetObj.ClientObject.SendMessage("You do not have enough permission to \"Say\".");
            return true;
            #endregion
        }
    }
}