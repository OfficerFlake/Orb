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
        public static readonly CommandDescriptor Orb_Command_Server_Apocolypse = new CommandDescriptor
        {
            _Name = "Apocolypse",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Changes the time to Apocolypse",
            _Usage = "Usage: /Server.Apocolypse",
            _Commands = new string[] { "/Server.Apocolypse", "/Server.Apoc", "/Apocolypse", "/Apoc" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Server_Apocolypse_Method,
        };

        public static bool Orb_Command_Server_Apocolypse_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region manageserver?
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer))
            {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            #endregion
            #region Apocolypse
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer)) {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            uint apocolypse = 16777216;
            if (Server.Weather.Lighting >= 16777216) Server.Weather.Lighting -= apocolypse;
            else Server.Weather.Lighting += apocolypse;
            string state = "OFF";
            if (Server.Weather.Lighting >= 16777216) state = "ON";
            Server.AllClients.Except(NetObj).SendMessage("User: \"" + NetObj.Username + "\" changed the time to " + state);
            NetObj.ClientObject.SendMessage("Changed the time to " + state + ".");
            Server.AllClients.Send(Server.Weather.Serialise());
        return true;
            #endregion
        }
    }
}