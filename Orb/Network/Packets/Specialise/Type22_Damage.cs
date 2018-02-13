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
            public partial class Type22_Damage : CommonPacket
            {
                public uint Type = 22;

                public uint VictimType = 0;
                public uint VictimID = 0;
                public uint AttackerType = 0;
                public ushort Damage = 0;
                public ushort Weapon = 0;
                public uint Unknown = 0;

                public Type22_Damage()
                {
                }

                public Type22_Damage(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type22_Damage(Packet InputPacket)
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
                    VictimType = Data.Take(4).ToArray().ToUint();
                    VictimID = Data.Skip(4).Take(4).ToArray().ToUint();
                    AttackerType = Data.Skip(8).Take(4).ToArray().ToUint();
                    Damage = Data.Skip(12).Take(2).ToArray().ToUshort();
                    Weapon = Data.Skip(14).Take(2).ToArray().ToUshort();
                    Unknown = Data.Skip(16).Take(4).ToArray().ToUint();
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("VictimType: " + VictimType.ToString().Clean());
                    Logger.Console.WriteLine("VictimID: " + VictimID.ToString().Clean());
                    Logger.Console.WriteLine("AttackerType: " + AttackerType.ToString().Clean());
                    Logger.Console.WriteLine("Damage: " + Damage.ToString().Clean());
                    Logger.Console.WriteLine("Weapon: " + Weapon.ToString().Clean());
                    Logger.Console.WriteLine("Unknown: " + Unknown.ToString().Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(VictimType).ToDataString();
                    outdata += BitConverter.GetBytes(VictimID).ToDataString();
                    outdata += BitConverter.GetBytes(AttackerType).ToDataString();
                    outdata += BitConverter.GetBytes(Damage).ToDataString();
                    outdata += BitConverter.GetBytes(Weapon).ToDataString();
                    outdata += BitConverter.GetBytes(Unknown).ToDataString();
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}