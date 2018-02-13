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
            public static string ByteToHexString(byte[] input)
            {
                string output = "";
                foreach (byte thisbyte in input)
                {
                    output += (char)thisbyte;
                }
                return output;
            }
        }
    }
}
