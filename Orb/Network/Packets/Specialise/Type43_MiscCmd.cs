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
            public partial class Type43_MiscCmd : CommonPacket
            {
                public uint Type = 43;

                public string Command = "NULL";
                public string Value = "NULL";

                public Type43_MiscCmd()
                {
                }
                public Type43_MiscCmd(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }
                public Type43_MiscCmd(Packet InputPacket)
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
                    Command = Packets.ByteToHexString(Data.Skip(4).ToArray()).Split(' ')[0];
                    Command = Command.Replace(" ", "");
                    //Logger.Console.WriteLine("$" + Command + "$");
                    Value = Packets.ByteToHexString(Data.Skip(4).ToArray()).Split(' ')[1];
                    Value = Value.Replace(" ", "");
                }
            }
        }
    }
}
