using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Orb
{
    public static partial class Network
    {
        public static partial class Packets
        {
            public static string PacketToHex(Packet packetobject) {
                string output = Network.Packets.UInt32ToBytes(packetobject.Size) + Network.Packets.UInt32ToBytes(packetobject.Type) + packetobject.Data;
                return Utilities.IO.StringToHex(output);
            }
        }
    }
}
