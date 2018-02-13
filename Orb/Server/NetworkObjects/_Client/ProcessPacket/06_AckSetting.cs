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
                public void Process_Type06_AckSetting(Network.Packet Packet)
                {
                    Network.Packets.Type06_AckSetting ThisPacket = new Network.Packets.Type06_AckSetting(Packet);

                    if (ThisPacket.Parameter1 == 50 && ThisPacket.Size >= 20)
                    {
                        //0-4 = 50
                        //4-8 = X
                        //8-12 = Y
                        //12-16 = Z
                        Parent.Vehicle.FormX = BitConverter.ToSingle(ThisPacket.Data.Skip(4).Take(4).ToArray(), 0);
                        Parent.Vehicle.FormY = BitConverter.ToSingle(ThisPacket.Data.Skip(8).Take(4).ToArray(), 0);
                        Parent.Vehicle.FormZ = BitConverter.ToSingle(ThisPacket.Data.Skip(12).Take(4).ToArray(), 0);
                        Logger.Console.WriteLine("Recieved Formation Update");
                    }
                    else
                    {
                        Parent.HostObject.Send(Packet);
                        if (ThisPacket.Parameter1 == 0)
                        {
                            //Logger.Console.WriteLine(ThisPacket.Data.ToHexString());
                        }
                    }
                }
            }
        }
    }
}