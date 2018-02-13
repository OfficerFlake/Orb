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
            public partial class Type33_Weather : CommonPacket
            {
                public uint Type = 33;

                public uint Lighting = 0;
                public uint RawOptions = 0;
                public bool Blackout
                {
                    get
                    {
                        return RawOptions.ToBinaryString().Skip(5).Take(1).ToArray().ToByteArray().ToDataString()[0].ToBool();
                    }
                    set
                    {
                        RawOptions = (RawOptions.ToBinaryString().Take(5).ToArray().ToByteArray().ToDataString() + value.ToUint().ToString() + RawOptions.ToBinaryString().Skip(6).Take(2).ToArray().ToByteArray().ToDataString()).FromBinaryStringToUint();
                    }
                }
                public bool LandEverywhere
                {
                    get
                    {
                        return RawOptions.ToBinaryString().Skip(1).Take(1).ToArray().ToByteArray().ToDataString()[0].ToBool();
                    }
                    set
                    {
                        RawOptions = (RawOptions.ToBinaryString().Take(1).ToArray().ToByteArray().ToDataString() + value.ToUint().ToString() + RawOptions.ToBinaryString().Skip(2).Take(6).ToArray().ToByteArray().ToDataString()).FromBinaryStringToUint();
                    }
                }
                public bool Collisions
                {
                    get
                    {
                        return RawOptions.ToBinaryString().Skip(3).Take(1).ToArray().ToByteArray().ToDataString()[0].ToBool();
                    }
                    set
                    {
                        RawOptions = (RawOptions.ToBinaryString().Take(3).ToArray().ToByteArray().ToDataString() + value.ToUint().ToString() + RawOptions.ToBinaryString().Skip(4).Take(4).ToArray().ToByteArray().ToDataString()).FromBinaryStringToUint();
                    }
                }
                public float WindX = 0;
                public float WindY = 0;
                public float WindZ = 0;
                public float Fog = 0;

                public Type33_Weather()
                {
                }

                public Type33_Weather(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length - 8);

                    Process(Data);
                }
                public Type33_Weather(Packet InputPacket)
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
                    Lighting = Data.Take(4).ToArray().ToUint();
                    RawOptions = Data.Skip(4).Take(4).ToArray().ToUint();
                    WindX = Data.Skip(8).Take(4).ToArray().ToFloat();
                    WindY = Data.Skip(12).Take(4).ToArray().ToFloat();
                    WindZ = Data.Skip(16).Take(4).ToArray().ToFloat();
                    Fog = Data.Skip(20).Take(4).ToArray().ToFloat();
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("Day: " + Lighting.ToString().Clean());
                    Logger.Console.WriteLine("RawOptions: " + RawOptions.ToBinaryString().Clean());
                    Logger.Console.WriteLine("-Blackout: " + Blackout.ToString().Clean());
                    Logger.Console.WriteLine("-LandEverywhere: " + LandEverywhere.ToString().Clean());
                    Logger.Console.WriteLine("-Collisions: " + Collisions.ToString().Clean());
                    Logger.Console.WriteLine("WindX: " + WindX.ToString().Clean());
                    Logger.Console.WriteLine("WindY: " + WindY.ToString().Clean());
                    Logger.Console.WriteLine("WindZ: " + WindZ.ToString().Clean());
                    Logger.Console.WriteLine("Fog: " + Fog.ToString().Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(Lighting).ToDataString();
                    outdata += BitConverter.GetBytes(RawOptions).ToDataString();
                    outdata += BitConverter.GetBytes(WindX).ToDataString();
                    outdata += BitConverter.GetBytes(WindY).ToDataString();
                    outdata += BitConverter.GetBytes(WindZ).ToDataString();
                    outdata += BitConverter.GetBytes(Fog).ToDataString();
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}