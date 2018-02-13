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
                public void Process_Type08_JoinRequest(Network.Packet Packet)
                {
                    Network.Packets.Type08_JoinRequest ThisPacket = new Network.Packets.Type08_JoinRequest(Packet);
                    Logger.Console.WriteLine(Parent.Username + " Join Request Received.");
                    Parent.HostObject.Send(Packet);
                }
            }
        }
    }
}