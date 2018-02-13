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
                public void Process_Type03_Error(Network.Packet Packet)
                {
                    Network.Packets.Type03_Error ThisPacket = new Network.Packets.Type03_Error(Packet);
                    Parent.HostObject.Send(Packet);
                }
            }
        }
    }
}