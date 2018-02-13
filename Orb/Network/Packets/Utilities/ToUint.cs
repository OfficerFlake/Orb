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
            public static UInt32 ByteToUint(byte[] input)
            {
                return BitConverter.ToUInt32(input, 0);
            }
        }
    }
}
