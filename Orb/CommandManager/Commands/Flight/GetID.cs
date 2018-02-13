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
        public static readonly CommandDescriptor Orb_Command_Flight_VehicleID = new CommandDescriptor
        {
            _Name = "VehicleID",
            _Version = 1.0,
            _Date = new DateTime(2014, 06, 01),
            _Author = "OfficerFlake",
            _Descrption = "Get your current Vehicle ID.",
            _Usage = "Usage: /Flight.VehicleID",
            _Commands = new string[] { "/Flight.GetID", "/GetID", "/Flight.VehicleID", "/VehicleID" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Flight_VehicleID_Method,
        };

        public static bool Orb_Command_Flight_VehicleID_Method(Server.NetObject NetObj, CommandReader Command)
        {
            if (NetObj.Vehicle.ID == 0)
            {
                NetObj.ClientObject.SendMessage("&eYou are not currently in a vehicle!");
                return false;
            }
            NetObj.ClientObject.SendMessage("&eYour Vehicle ID is \"" + NetObj.Vehicle.ID.ToString() + "\".");
            return true;
        }
    }
}