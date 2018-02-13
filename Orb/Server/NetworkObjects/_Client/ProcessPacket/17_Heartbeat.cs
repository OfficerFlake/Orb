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
                public void Process_Type17_Heartbeat(Network.Packet Packet)
                {
                    Network.Packets.Type17_Heartbeat ThisPacket = new Network.Packets.Type17_Heartbeat(Packet);
                    Parent.HostObject.Send(Packet);
                }
            }
        }
    }
}