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
        public static readonly CommandDescriptor Orb_Command_Server_Lightning = new CommandDescriptor
        {
            _Name = "Lightning",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Rapidly changes the server time between DAY and NIGHT once, for visual effect.",
            _Usage = "Usage: /Server.Lightning",
            _Commands = new string[] { "/Server.Lightning", "/Lightning", "/Zeus", "/Thunder" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Server_Lightning_Method,
        };

        public static bool Orb_Command_Server_Lightning_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region manageserver?
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer))
            {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            #endregion
            #region Lightning
            uint apocolypse = 0;
            uint Weather = Server.Weather.Lighting;
            if (Server.Weather.Lighting > 16777216) apocolypse = 16777216;
            if (Server.Weather.Lighting >= 65536) //NIGHT
            {
                Server.Weather.Lighting = apocolypse; //DAY
                Server.AllClients.Send(Server.Weather.Serialise());
                Thread.Sleep(25);
                Server.Weather.Lighting = Weather;
                Server.AllClients.Send(Server.Weather.Serialise());
            }
            else //DAY
            {
                Server.Weather.Lighting = apocolypse | 65536; //NIGHT
                Server.AllClients.Send(Server.Weather.Serialise());
                Thread.Sleep(25);
                Server.Weather.Lighting = Weather;
                Server.AllClients.Send(Server.Weather.Serialise());
            }
            return true;
            #endregion
        }
    }
}