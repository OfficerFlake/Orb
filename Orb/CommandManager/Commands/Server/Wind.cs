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
        public static readonly CommandDescriptor Orb_Command_Server_Wind = new CommandDescriptor
        {
            _Name = "Wind",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Changes the wind on the server",
            _Usage = "Usage: /Server.Wind <X> <Y> <Z>",
            _Commands = new string[] { "/Server.Wind", "/Wind" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Server_Wind_Method,
        };

        public static bool Orb_Command_Server_Wind_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region manageserver?
            if (NetObj.UserObject.CanNot(Database.PermissionDB.Strings.ManageServer))
            {
                NetObj.ClientObject.SendMessage("You do not have permission to manage the server.");
                return false;
            }
            #endregion
            #region Wind
            if (Command._CmdArguments.Count() == 1)
            {
                if (Command._CmdArguments[0].ToUpper() == "OFF" ||
                    Command._CmdArguments[0].ToUpper() == "FALSE" ||
                    Command._CmdArguments[0].ToUpper() == "0")
                {
                    Server.Weather.WindX = 0;
                    Server.Weather.WindY = 0;
                    Server.Weather.WindZ = 0;
                    NetObj.ClientObject.SendMessage("Wind turned OFF.");
                }
                else
                {
                    if (Command._CmdArguments[0].ToUpper().EndsWith("KT"))
                    {
                        int direction = 0;
                        int speed = 0;

                        try
                        {
                            direction = Int32.Parse(Command._CmdArguments[0].ToUpper().Slice(0, 3));
                            speed = Int32.Parse(Command._CmdArguments[0].ToUpper().Slice(3, 5));
                        }
                        catch
                        {
                            NetObj.ClientObject.SendMessage("Directional Speed Assignement Format Invalid!");
                            return false;
                        }
                        Server.Weather.WindX = (float)Math.Cos(((double)(direction + 90)).ToRadians()) * -speed; //Negative speed, otherwise the axis is reversed!
                        Server.Weather.WindY = 0;
                        Server.Weather.WindZ = (float)Math.Sin(((double)(direction + 90)).ToRadians()) * speed;
                        NetObj.ClientObject.SendMessage("Wind set to: " + Command._CmdArguments[0] + ".");
                    }
                    else
                    {
                        NetObj.ClientObject.SendMessage("Arguments Invalid. Please use \"OFF\" or a TAF format wind representation.");
                        return false;
                    }
                }
            }
            else if (Command._CmdArguments.Count() >= 3)
            {
                try
                {
                    Server.Weather.WindX = (float)Single.Parse(Command._CmdArguments[0]);
                    Server.Weather.WindY = (float)Single.Parse(Command._CmdArguments[1]);
                    Server.Weather.WindZ = (float)Single.Parse(Command._CmdArguments[2]);
                    NetObj.ClientObject.SendMessage("Wind set to: " + Server.Weather.WindX.ToString() + ", " + Server.Weather.WindY.ToString() + ", " + Server.Weather.WindZ.ToString() + ".");
                }
                catch
                {
                    NetObj.ClientObject.SendMessage("Argument Format Invalid! Use Numbers ONLY.");
                    return false;
                }
            }
            else
            {
                NetObj.ClientObject.SendMessage("Not Enough Arguments!");
                return false;
            }
            Server.AllClients.Send(Server.Weather.Serialise());
            return true;
            #endregion
        }
    }
}