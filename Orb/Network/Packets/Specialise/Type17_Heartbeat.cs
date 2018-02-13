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
            public partial class Type17_Heartbeat : CommonPacket
            {
                public uint Type = 17;

                public Type17_Heartbeat()
                {
                }
                public Type17_Heartbeat(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type17_Heartbeat(Packet InputPacket)
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

                    //Nothing to do. This is just an empty packet that says "I'm still here!"
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("Heartbeats contain no data.");
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
