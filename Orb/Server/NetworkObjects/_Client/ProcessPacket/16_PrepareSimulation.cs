﻿using System;
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
                public void Process_Type16_PrepareSimulation(Network.Packet Packet)
                {
                    Network.Packets.Type16_PrepareSimulation ThisPacket = new Network.Packets.Type16_PrepareSimulation(Packet);
                    //ThisPacket.PrintDebugInfo();
                    //Parent.HostObject.Send(Packet);
                    Parent.HostObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}