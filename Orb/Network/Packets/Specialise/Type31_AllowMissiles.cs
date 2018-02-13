﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Network
    {
        public static partial class Packets
        {
            public partial class Type31_AllowMissiles : CommonPacket
            {
                public uint Type = 31;
                public bool Allowed = true;

                public Type31_AllowMissiles()
                {
                }
                public Type31_AllowMissiles(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type31_AllowMissiles(Packet InputPacket)
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
                    Allowed = Data.Take(4).ToArray().ToUint().ToBool();
                }

                public Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(Allowed.ToUint()).ToDataString();
                    outpack.Data = outdata;
                    return outpack;
                }
            }
        }
    }
}
