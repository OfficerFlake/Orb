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
            public static Packet New()
            {
                return new Packet();
            }

            public static Packet New(uint type, string data)
            {
                Packet output = new Packet();
                output.Type = type;
                output.Data = data;
                return output;
            }
        }
    }
}
