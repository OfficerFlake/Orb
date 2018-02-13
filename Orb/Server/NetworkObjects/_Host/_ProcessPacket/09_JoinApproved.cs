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
                public void Process_Type09_JoinApproved(Network.Packet Packet)
                {
                    Network.Packets.Type09_JoinApproved ThisPacket = new Network.Packets.Type09_JoinApproved(Packet);
                    //Parent.ClientObject.Send(ThisPacket.Serialise());
                    Parent.ClientObject.Send(Packet);
                }
            }
        }
    }
}
