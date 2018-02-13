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
                public void Process_Type11_FlightData(Network.Packet Packet)
                {
                    Network.Packets.Type11_FlightData ThisPacket = new Network.Packets.Type11_FlightData(Packet);
                    Parent.ClientObject.Send(Packet);

                    /*
                    Logger.Console.WriteLine("&b" + ThisPacket.TimeStamp.ToString());
                    if (ThisPacket.ID == Parent.Vehicle.ID)
                    {
                        Logger.Console.WriteLine("&b####" + ThisPacket.Data.ToHexString());
                    }
                    else
                    {
                        Logger.Console.WriteLine("&b$$$$" + ThisPacket.Data.ToHexString());
                    }
                    */
                }
            }
        }
    }
}