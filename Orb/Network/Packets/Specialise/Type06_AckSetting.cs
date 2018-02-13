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
            public partial class Type06_AckSetting : CommonPacket
            {
                public uint Type = 6;
                public uint Parameter1 = 0;
                public uint Parameter2 = 0;

                public Type06_AckSetting()
                {
                }
                public Type06_AckSetting(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type06_AckSetting(Packet InputPacket)
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
                    Parameter1 = Data.Take(4).ToArray().ToUint();
                    Parameter2 = Data.Skip(4).Take(4).ToArray().ToUint();
                }
            }
        }
    }
}
