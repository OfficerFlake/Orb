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
            public partial class Type32_ChatMessage : CommonPacket
            {
                public string Message;
                public uint Type = 32;

                public Type32_ChatMessage()
                {
                }
                public Type32_ChatMessage(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type32_ChatMessage(Packet InputPacket)
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

                    //Console.WriteLine(Data.ToHexString());

                    Message = Data.Skip(8).ToArray().ToDataString().Replace("\0", ""); //(Blah) Message (First 8 characters are garbage).

                    //Console.WriteLine(Data.Skip(8).ToArray().ToDataString());
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("Message: " + Message.Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata = new string('\0', 8);
                    outdata += Message;
                    outdata += '\0';
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
