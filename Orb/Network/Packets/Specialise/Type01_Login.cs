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
            public partial class Type01_Login : CommonPacket
            {
                public uint Type = 1;
                public string Username = "Connecting...";
                public uint Version = 0;
                public string FullUsername = "Connecting...";


                public Type01_Login()
                {
                }

                public Type01_Login(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }

                public Type01_Login(Packet InputPacket)
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
                    Username = Packets.ByteToHexString(Data.Take(16).ToArray()).Split('\0')[0];
                    Version = Network.Packets.ByteToUint(Data.Skip(16).Take(4).ToArray());
                    if (Data.Length > 20)
                    {
                        FullUsername = Packets.ByteToHexString(Data.Skip(20).ToArray()).Split('\0')[0];
                    }
                    else
                    {
                        FullUsername = Username;
                    }
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("Username: " + Username.Clean());
                    Logger.Console.WriteLine("Version: " + Version.ToString().Clean());
                    Logger.Console.WriteLine("FullUsername: " + FullUsername.Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += Username + new string('\0', 16 - Username.Length);
                    outdata += BitConverter.GetBytes(Version).ToDataString();
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
