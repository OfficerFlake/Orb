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
        public static readonly CommandDescriptor Orb_Command_Flight_Smoke_Rainbow = new CommandDescriptor
        {
            _Name = "Smoke Rainbow",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Changes your inflight smoke color to a rainbow",
            _Usage = "Usage: /Flight.Smoke.Rainbow",
            _Commands = new string[] { "/Flight.Smoke.Rainbow", "/Rainbow"},

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Flight_Smoke_Rainbow_Method,
        };

        public static bool Orb_Command_Flight_Smoke_Rainbow_Method(Server.NetObject NetObj, CommandReader Command)
        {
            byte Alpha = 0;
            byte Red = 255;
            byte Green = 0;
            byte Blue = 0;

            if (NetObj.Vehicle.ID == 0)
            {
                NetObj.ClientObject.SendMessage("&eYou need to be flying in order to change your smoke color!");
                return false;
            }
            NetObj.Smoke = ""; //This will help us cancel other rainbows!
            Thread.Sleep(10);
            NetObj.Smoke = "RAINBOW";
            int Hue = 0;
            while (NetObj.Vehicle.ID != 0 && NetObj.Smoke == "RAINBOW")
            {
                if (Hue >= 360) Hue = 0;
                Network.Packets.Type07_SmokeCol SmokePacket = new Network.Packets.Type07_SmokeCol();
                SmokePacket.ID = NetObj.Vehicle.ID;
                SmokePacket.Alpha = Alpha;
                SmokePacket.Red = 0;
                SmokePacket.Green = 0;
                SmokePacket.Blue = 0;
                if (Hue < 60)
                {
                    Red = 255;
                    Green = (byte)(Math.Ceiling(((Hue) / 60d) * 255));
                }
                else if (Hue < 120)
                {
                    Red = (byte)(Math.Ceiling(((Hue - 60) / 60d) * -255) + 255);
                    Green = 255;
                }
                else if (Hue < 180)
                {
                    Green = 255;
                    Blue = (byte)(Math.Ceiling(((Hue - 120) / 60d) * 255));
                }
                else if (Hue < 240)
                {
                    Green = (byte)(Math.Ceiling(((Hue - 180) / 60d) * -255) + 255);
                    Blue = 255;
                }
                else if (Hue < 300)
                {
                    Blue = 255;
                    Red = (byte)(Math.Ceiling(((Hue - 240) / 60d) * 255));
                }
                else if (Hue < 360)
                {
                    Blue = (byte)(Math.Ceiling(((Hue - 300) / 60d) * -255) + 255);
                    Red = 255;
                }
                SmokePacket.Red = Red;
                SmokePacket.Green = Green;
                SmokePacket.Blue = Blue;
                Hue += 1;
                Thread.Sleep(10);
                if (NetObj.Smoke == "RAINBOW")
                {
                    NetObj.HostObject.Send(SmokePacket.Serialise());
                    NetObj.ClientObject.Send(SmokePacket.Serialise());
                }
            }
            return true;
        }
    }
}