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
        public static readonly CommandDescriptor Orb_Command_Server_LandEverywhere = new CommandDescriptor
        {
            _Name = "LandEverywhere",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Turns Land Everywhere ON or OFF.",
            _Usage = "Usage: /Server.LandEverywhere <ON|OFF>",
            _Commands = new string[] { "/Server.LandEverywhere", "/LandEverywhere" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Server_LandEverywhere_Method,
        };

        public static bool Orb_Command_Server_LandEverywhere_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region manageserver?
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer))
            {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            #endregion
            #region LandEverywhere
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer)) {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            bool state = Server.Weather.LandEverywhere;
            if (Command._CmdArguments.Count() < 1)
            {
                state = !state; //inverse the state!
            }
            else {
                if (Command._CmdArguments[0].ToUpper() == "OFF" ||
                    Command._CmdArguments[0].ToUpper() == "FALSE" ||
                    Command._CmdArguments[0].ToUpper() == "0")
                {
                    state = false;
                }
                else if (Command._CmdArguments[0].ToUpper() == "ON" ||
                         Command._CmdArguments[0].ToUpper() == "TRUE" ||
                         Command._CmdArguments[0].ToUpper() == "1")
                {
                    state = true;
                }
                else
                {
                    NetObj.ClientObject.SendMessage("Argument not Recognised: \"" + Command._CmdArguments[0] + "\". Try \"ON\" or \"OFF\" only.");
                    return false;
                }
            }
            if (!state)
            {
                Server.Weather.LandEverywhere = false;
                Server.AllClients.Except(NetObj).SendMessage("User: \"" + NetObj.Username + "\" turned Land Everywhere OFF");
                NetObj.ClientObject.SendMessage("Land Everywhere turned OFF.");
                Server.AllClients.Send(Server.Weather.Serialise());
                return true;
            }
            else
            {
                Server.Weather.LandEverywhere = true;
                Server.AllClients.Except(NetObj).SendMessage("User: \"" + NetObj.Username + "\" turned Land Everywhere ON");
                NetObj.ClientObject.SendMessage("Land Everywhere turned ON.");
                Server.AllClients.Send(Server.Weather.Serialise());
                return true;
            }
            #endregion
        }
    }
}