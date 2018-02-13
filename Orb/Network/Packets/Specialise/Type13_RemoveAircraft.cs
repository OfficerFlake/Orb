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
            public partial class Type13_RemoveAircraft : CommonPacket
            {
                public uint Type = 13;

                public uint ID;
                public uint ObjectType;

                public Type13_RemoveAircraft()
                {
                }
                public Type13_RemoveAircraft(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }
                public Type13_RemoveAircraft(Packet InputPacket)
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
                    ID = Data.Take(4).ToArray().ToUint();
                    ObjectType = Data.Skip(4).Take(4).ToArray().ToUint();
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("&bUnjoin Packet Debug Info");
                    Logger.Console.WriteLine("    ID: " + ID.ToString());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(ID).ToDataString();
                    outdata += BitConverter.GetBytes(ObjectType).ToDataString();
                    outpack.Type = Type;
                    outpack.Data = outdata;
                    return outpack;
                }
            }
        }
    }
}
