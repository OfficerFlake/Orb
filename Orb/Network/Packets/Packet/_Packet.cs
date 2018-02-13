using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Orb
{
    public static partial class Network
    {
        public partial class Packet
        {
            public uint Size { get; private set; }
            public uint Type;
            private string _Data = "";
            public string Data
            {
                get
                {
                    return _Data;
                }
                set
                {
                    _Data = value;
                    RecalcSize();
                }
            }

            public Packet()
            {
                RecalcSize();
            }

            public byte[] Serialise()
            {
                List<Byte> FullPacket = new List<Byte>();

                foreach (byte thisbyte in BitConverter.GetBytes(this.Size))
                {
                    FullPacket.Add(thisbyte);
                }

                foreach (byte thisbyte in BitConverter.GetBytes(this.Type))
                {
                    FullPacket.Add(thisbyte);
                }

                //Console.WriteLine(InPacket.Data);
                foreach (byte thisbyte in this.Data.ToCharArray())
                {
                    FullPacket.Add(thisbyte);
                }

                return FullPacket.ToArray();
            }
        }
    }
}
