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
        public static readonly CommandDescriptor Orb_Command_Server_Fog = new CommandDescriptor
        {
            _Name = "Fog",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Changes the fog distance",
            _Usage = "Usage: /Server.Fog",
            _Commands = new string[] { "/Server.Fog", "/Server.Visibility", "/Fog", "/Visibility" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Server_Fog_Method,
        };

        public static bool Orb_Command_Server_Fog_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region manageserver?
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer))
            {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            #endregion
            #region Fog
            if (Command._CmdArguments.Count() >= 1)
            {
                if (Command._CmdArguments[0].ToUpper() == "OFF" ||
                    Command._CmdArguments[0].ToUpper() == "FALSE" ||
                    Command._CmdArguments[0].ToUpper() == "0")
                {
                    Server.Weather.Fog = 50000;
                    NetObj.ClientObject.SendMessage("Fog turned OFF.");
                }
                else
                {
                    try
                    {
                        Server.Weather.Fog = Single.Parse(Command._CmdArguments[0].ToUpper());
                        NetObj.ClientObject.SendMessage("Fog set to: " + Server.Weather.Fog.ToString());
                    }
                    catch
                    {
                        NetObj.ClientObject.SendMessage("Argument Invalid! Use a number only!");
                        return false;
                    }
                }
                Server.AllClients.Send(Server.Weather.Serialise());
            }
            else
            {
                NetObj.ClientObject.SendMessage("Not Enough Arguments!");
                return false;
            }
            return true;
            #endregion
        }
    }
}