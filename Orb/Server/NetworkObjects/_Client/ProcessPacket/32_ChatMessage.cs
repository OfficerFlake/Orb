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
                public void Process_Type32_ChatMessage(Network.Packet Packet)
                {
                    Network.Packets.Type32_ChatMessage ThisPacket = new Network.Packets.Type32_ChatMessage(Packet);
                    string EditMessage = ThisPacket.Message.Remove(0, 1);
                    EditMessage = EditMessage.Remove(0, Parent.Username.Length);
                    EditMessage = EditMessage.Remove(0, 1);
                    Thread CommandHandle = new Thread(() => CommandManager.Process(this.Parent, EditMessage));
                    CommandHandle.Start();
                    return;
                }
            }
        }
    }
}