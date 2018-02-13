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
                public void Process_Type32_ChatMessage(Network.Packet Packet)
                {
                    Network.Packets.Type32_ChatMessage ThisPacket = new Network.Packets.Type32_ChatMessage(Packet);
                    if (true) //Disable to remove take-off message filtering!
                    {
                        if (ThisPacket.Message.Contains("took off") && !ThisPacket.Message.StartsWith("("))
                        {
                            if (Server.AllClients.Select(x => x.Username.ToUpperInvariant().Slice(0, 15)).Contains(ThisPacket.Message.ToUpperInvariant().Split(' ')[0].Slice(0, 15)))
                            {
                                return;
                            }
                            ThisPacket.Message = "&d" + ThisPacket.Message;
                        }
                        if (ThisPacket.Message.Contains("left the") && !ThisPacket.Message.StartsWith("("))
                        {
                            if (Server.AllClients.Select(x => x.Username.ToUpperInvariant().Slice(0, 15)).Contains(ThisPacket.Message.Split(' ')[0].ToUpperInvariant().Slice(0, 15)))
                            {
                                return;
                            }
                            ThisPacket.Message = "&d" + ThisPacket.Message;
                        }
                        if (ThisPacket.Message.Contains(": ") && !ThisPacket.Message.StartsWith("(") && Server.AllClients.Select(x => x.Username.ToUpperInvariant().Slice(0, 15)).Contains(ThisPacket.Message.Split(':')[0].ToUpperInvariant().Slice(0, 15)))
                        {
                            return;
                        }
                        ThisPacket.Message = "&9" + ThisPacket.Message;
                    }
                    Parent.ClientObject.SendMessage(ThisPacket.Message);
                    if (Parent != Server.ClientList[0]) return; //Only show messages ONCE!
                    if (ThisPacket.Message.Contains("Log-on process completed")) return;
                    if (ThisPacket.Message.StartsWith("****")) {
                        ThisPacket.Message = "&c" + ThisPacket.Message;
                    }
                    Logger.Console.WriteLine(ThisPacket.Message);
                    Logger.Log.Chat(ThisPacket.Message);
                }
            }
        }
    }
}