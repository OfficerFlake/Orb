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
            public partial class Host
            {
                public void Process_Type43_MiscCmd(Network.Packet Packet)
                {
                    //Network.Packets.Type43_MiscCmd ThisPacket = new Network.Packets.Type43_MiscCmd(Packet);
                    Parent.ClientObject.Send(Packet);
                }
            }
        }
    }
}