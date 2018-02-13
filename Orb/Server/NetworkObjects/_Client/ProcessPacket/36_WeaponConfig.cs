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
                public void Process_Type36_WeaponConfig(Network.Packet Packet)
                {
                    Network.Packets.Type36_WeaponConfig ThisPacket = new Network.Packets.Type36_WeaponConfig(Packet);
                    //ThisPacket.PrintDebugInfo();
                    Parent.HostObject.Send(Packet);
                }
            }
        }
    }
}