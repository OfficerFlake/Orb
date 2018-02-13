using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Network
    {
        public static partial class Packets
        {
            public partial class CommonPacket
            {
                public byte[] Data;
                public uint Size
                {
                    get
                    {
                        return (uint)Data.Length + 4;
                    }
                    private set
                    {
                    }
                }
                public uint Type;
            }
        }
    }
}
