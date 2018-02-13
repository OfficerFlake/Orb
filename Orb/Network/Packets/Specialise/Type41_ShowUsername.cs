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
            public partial class Type41_ShowUsername : CommonPacket
            {
                public uint Type = 41;
                public uint Distance = 1;

                public Type41_ShowUsername()
                {
                }
                public Type41_ShowUsername(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type41_ShowUsername(Packet InputPacket)
                {
                    //Size is an auto generated method.
                    //Type is always 43 for this packet type.
                    //Generate data
                    Data = new byte[InputPacket.Data.Length];
                    Data = InputPacket.Serialise().Skip(8).Take(Data.Length).ToArray();

                    Process(Data);
                }

                internal void Process(byte[] Data)
                {
                    //Actually build the packet...
                    Distance = Data.Take(4).ToArray().ToUint();
                }

                public Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(Distance).ToDataString();
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
