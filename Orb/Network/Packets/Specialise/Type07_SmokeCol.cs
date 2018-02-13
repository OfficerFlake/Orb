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
            public partial class Type07_SmokeCol : CommonPacket
            {
                public uint Type = 7;
                public uint ID = 0;
                public byte Alpha = 0;
                public byte Red = 255;
                public byte Green = 255;
                public byte Blue = 255;

                //MORE DATA IS TO BE ADDED TO THIS PACKET.


                public Type07_SmokeCol()
                {
                }

                public Type07_SmokeCol(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }

                public Type07_SmokeCol(Packet InputPacket)
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
                    ID = Data.Take(4).ToArray().ToUint();
                    Alpha = Data.Skip(4).Take(1).ToArray()[0];
                    Red = Data.Skip(5).Take(1).ToArray()[0];
                    Green = Data.Skip(6).Take(1).ToArray()[0];
                    Blue = Data.Skip(7).Take(1).ToArray()[0];
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("ID: " + ID.ToString().Clean());
                    Logger.Console.WriteLine("Alpha: " + Alpha.ToString().Clean());
                    Logger.Console.WriteLine("Red: " + Red.ToString().Clean());
                    Logger.Console.WriteLine("Green: " + Green.ToString().Clean());
                    Logger.Console.WriteLine("Blue: " + Blue.ToString().Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(ID).ToDataString();
                    outdata += (char)Alpha;
                    outdata += (char)Red;
                    outdata += (char)Green;
                    outdata += (char)Blue;
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    //PrintDebugInfo();
                    return outpack;
                }
            }
        }
    }
}
