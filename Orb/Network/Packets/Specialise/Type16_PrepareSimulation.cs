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
            public partial class Type16_PrepareSimulation : CommonPacket
            {
                public uint Type = 16;

                public Type16_PrepareSimulation()
                {
                }
                public Type16_PrepareSimulation(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }
                public Type16_PrepareSimulation(Packet InputPacket)
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
                }

                internal void PrintDebugInfo()
                {
                }

                public Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
