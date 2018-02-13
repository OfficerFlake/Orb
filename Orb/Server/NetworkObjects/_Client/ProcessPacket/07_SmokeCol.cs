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
            public partial class Client
            {
                public void Process_Type07_SmokeCol(Network.Packet Packet)
                {
                    Network.Packets.Type07_SmokeCol ThisPacket = new Network.Packets.Type07_SmokeCol(Packet);
                    Parent.HostObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}