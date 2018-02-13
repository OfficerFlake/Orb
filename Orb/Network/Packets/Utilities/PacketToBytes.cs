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
            /*public static byte[] PacketToBytes(Packet InPacket)
            {
                List<Byte> FullPacket = new List<Byte>();

                foreach (byte thisbyte in BitConverter.GetBytes(InPacket.Size))
                {
                    FullPacket.Add(thisbyte);
                }

                foreach (byte thisbyte in BitConverter.GetBytes(InPacket.Type))
                {
                    FullPacket.Add(thisbyte);
                }

                //Console.WriteLine(InPacket.Data);
                foreach (byte thisbyte in InPacket.Data.ToCharArray())
                {
                    FullPacket.Add(thisbyte);
                }

                return FullPacket.ToArray();
            }*/
        }
    }
}
