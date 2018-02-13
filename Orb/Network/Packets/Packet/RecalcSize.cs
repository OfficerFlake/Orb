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
            public uint RecalcSize() {
                Size = 4 + (uint)Data.Length;
                return Size;
            }
        }
    }
}
