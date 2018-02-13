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
                public void Process_Type13_RemoveAircraft(Network.Packet Packet)
                {
                    Network.Packets.Type13_RemoveAircraft ThisPacket = new Network.Packets.Type13_RemoveAircraft(Packet);
                    //ThisPacket.PrintDebugInfo();
                    //Parent.HostObject.Send(Packet);
                    if (ThisPacket.ID == Parent.Vehicle.ID)
                    {
                        Server.Vehicles.RemoveAll(x => x.ID == Parent.Vehicle.ID);
                        Parent.Vehicle.ID = 0;
                        Parent.Vehicle.FormTarget = 0;
                        Server.AllClients.SendMessage(Parent.Username + " left the aircraft.");
                    }
                    Parent.ClientObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}