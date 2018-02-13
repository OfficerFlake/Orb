using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Orb
{
    public static partial class Server
    {
        public partial class NetObject
        {
            public partial class Host
            {
                public void Process_Type33_Weather(Network.Packet Packet)
                {
                    if (Packet.Size != 4)
                    { //This is not a weather request packet, we need to get its data.
                        Network.Packets.Type33_Weather ThisPacket = new Network.Packets.Type33_Weather(Packet);
                        VanillaWeather = ThisPacket;
                        //VanillaWeather.PrintDebugInfo();
                        //Weather.PrintDebugInfo();
                        Parent.ClientObject.Send(Weather.Serialise());
                    }
                    else Parent.ClientObject.Send(Packet);
                }
            }
        }
    }
}