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
                public void Process_Type39_AllowUnguided(Network.Packet Packet)
                {
                    Network.Packets.Type39_AllowUnguided ThisPacket = new Network.Packets.Type39_AllowUnguided(Packet);
                    Parent.HostObject.Send(Packet);
                }
            }
        }
    }
}