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
            public static string UInt32ToBytes(uint input)
            {
                string output = "";
                byte[] DataBlock = BitConverter.GetBytes(input);
                if (BitConverter.IsLittleEndian)
                {
                    DataBlock = DataBlock.Reverse().ToArray();
                }
                foreach (Byte data in DataBlock)
                {
                    output += (char)data;
                }
                return output;
            }
        }
    }
}
