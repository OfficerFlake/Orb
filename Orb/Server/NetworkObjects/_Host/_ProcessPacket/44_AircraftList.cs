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
            public partial class Host
            {
                public void Process_Type44_AircraftList(Network.Packet Packet)
                {
                    Network.Packets.Type44_AircraftList ThisPacket = new Network.Packets.Type44_AircraftList(Packet);
                    Parent.ClientObject.Send(Packet);
                }
            }
        }
    }
}