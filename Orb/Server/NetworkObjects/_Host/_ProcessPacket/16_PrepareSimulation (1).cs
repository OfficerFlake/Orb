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
                public void Process_Type16_PrepareSimulation(Network.Packet Packet)
                {
                    Network.Packets.Type16_PrepareSimulation ThisPacket = new Network.Packets.Type16_PrepareSimulation(Packet);
                    if (Parent.Username.ToUpperInvariant() == "PHP BOT")
                    {
                        Parent.ClientObject.SendMessage("Now that your log on has been completed, You have been disconnected from the server.\nTo use the server, Join as a client other then \"PHP bot\".");
                        Parent.Close();
                        return;
                    }
                    Logger.Console.WriteLine("&a" + Parent.Username + " Log on process completed.");
                    //ThisPacket.PrintDebugInfo();
                    //Parent.HostObject.Send(Packet);
                    Parent.ClientObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}