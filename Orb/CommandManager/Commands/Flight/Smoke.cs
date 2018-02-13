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
        public static readonly CommandDescriptor Orb_Command_Flight_Smoke = new CommandDescriptor
        {
            _Name = "Smoke",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Changes your inflight smoke color",
            _Usage = "Usage: /Flight.Smoke <R> <G> <B>",
            _Commands = new string[] { "/Flight.Smoke", "/Smoke"},

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Flight_Smoke_Method,
        };

        public static bool Orb_Command_Flight_Smoke_Method(Server.NetObject NetObj, CommandReader Command)
        {
            if (Command._CmdArguments.Count() < 3)
            {
                NetObj.ClientObject.SendMessage("&eFormat incorrect: Use instead: \"/Smoke Red Green Blue\"!");
                return false;
            }

            byte Alpha = 0;
            byte Red = 255;
            byte Green = 255;
            byte Blue = 255;

            try
            {
                Red = Byte.Parse(Command._CmdArguments[0]);
                Green = Byte.Parse(Command._CmdArguments[1]);
                Blue = Byte.Parse(Command._CmdArguments[2]);
            }
            catch
            {
                NetObj.ClientObject.SendMessage("&eFormat incorrect: Be sure you are using values between 0 and 255!");
                return false;
            }
            if (NetObj.Vehicle.ID == 0)
            {
                NetObj.ClientObject.SendMessage("&eYou need to be flying in order to change your smoke color!");
                return false;
            }
            Network.Packets.Type07_SmokeCol SmokePacket = new Network.Packets.Type07_SmokeCol();
            SmokePacket.ID = NetObj.Vehicle.ID;
            SmokePacket.Alpha = Alpha;
            SmokePacket.Red = Red;
            SmokePacket.Green = Green;
            SmokePacket.Blue = Blue;
            NetObj.Smoke = "";
            NetObj.HostObject.Send(SmokePacket.Serialise());
            NetObj.ClientObject.Send(SmokePacket.Serialise());
            return true;
        }
    }
}