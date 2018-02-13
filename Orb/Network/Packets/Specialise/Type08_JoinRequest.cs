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
            public partial class Type08_JoinRequest : CommonPacket
            {
                public uint Type = 8;

                //MORE DATA IS TO BE ADDED TO THIS PACKET.
                public uint IFF = 0;                //0,1,2,3
                public string Aircraft = "";        //31 char then null
                public string StartPosition = "";   //31 char then null
                public ushort Unknown1 = 1;         //??? 01-00
                public ushort Unknown2 = 75;        //??? 48-00
                public ushort Unknown3 = 1;         //??? 01-00


                public Type08_JoinRequest()
                {
                }

                public Type08_JoinRequest(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }

                public Type08_JoinRequest(Packet InputPacket)
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
                    IFF = Data.Take(4).ToArray().ToUint();
                    Aircraft = Data.Skip(4).Take(32).ToArray().ToDataString().Split('\0')[0];
                    StartPosition = Data.Skip(4).Skip(32).Take(32).ToArray().ToDataString().Split('\0')[0];
                    Unknown1 = Data.Skip(4).Skip(32).Skip(32).Take(2).ToArray().ToUshort();
                    Unknown2 = Data.Skip(4).Skip(32).Skip(32).Skip(2).Take(2).ToArray().ToUshort();
                    Unknown3 = Data.Skip(4).Skip(32).Skip(32).Skip(4).Take(2).ToArray().ToUshort();
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("IFF: " + IFF.ToString().Clean());
                    Logger.Console.WriteLine("Aircraft: " + Aircraft.Clean());
                    Logger.Console.WriteLine("StartPosition: " + StartPosition.Clean());
                    Logger.Console.WriteLine("Unknown1: " + Unknown1.ToString().Clean());
                    Logger.Console.WriteLine("Unknown2: " + Unknown2.ToString().Clean());
                    Logger.Console.WriteLine("Unknown3: " + Unknown3.ToString().Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(IFF).ToDataString();
                    outdata += Aircraft + new string('\0', 32 - Aircraft.Length);
                    outdata += StartPosition + new string('\0', 32 - StartPosition.Length);
                    outdata += BitConverter.GetBytes(Unknown1).ToDataString();
                    outdata += BitConverter.GetBytes(Unknown2).ToDataString();
                    outdata += BitConverter.GetBytes(Unknown3).ToDataString();
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
