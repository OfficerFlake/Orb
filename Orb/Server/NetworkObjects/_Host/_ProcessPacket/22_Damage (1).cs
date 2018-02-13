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
                public void Process_Type22_Damage(Network.Packet Packet)
                {
                    Network.Packets.Type22_Damage ThisPacket = new Network.Packets.Type22_Damage(Packet);
                    //ThisPacket.Damage = 0;
                    //ThisPacket.Weapon = 3;
                    //ThisPacket.PrintDebugInfo();
                    Parent.ClientObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}