using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Orb
{
    public static partial class Server
    {
        public partial class NetObject
        {
            public partial class Client
            {
                public void Process_Type01_Login(Network.Packet Packet)
                {
                    Network.Packets.Type01_Login ThisPacket = new Network.Packets.Type01_Login(Packet);
                    Server.Login.Handle(Parent, ThisPacket);
                    Network.Packet OutPacket = new Network.Packet();
                    OutPacket.Type = 100;
                    OutPacket.RecalcSize();
                    Parent.HostObject.Send(OutPacket);
                }
            }
        }
    }
}