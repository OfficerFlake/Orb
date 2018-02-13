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
            public partial class Type04_Map : CommonPacket
            {
                public uint Type = 4;
                public string MapName = "NULL";

                public Type04_Map()
                {
                }
                public Type04_Map(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 4 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }
                public Type04_Map(Packet InputPacket)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[InputPacket.Data.Length];
                    Data = InputPacket.Serialise().Skip(8).Take(Data.Length).ToArray();

                    Process(Data);
                }

                internal void Process(byte[] Data)
                {
                    //Actually build the packet...
                    MapName = Packets.ByteToHexString(Data.Take(60).ToArray()).Split('\0')[0];
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("MapName: " + MapName.Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += MapName + new string('\0', 15 - MapName.Length);
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
