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
            public partial class Process_Type05_EntityJoined : CommonPacket
            {
                public uint Type = 5;

                //MORE DATA IS TO BE ADDED TO THIS PACKET.
                public uint ObjectType = 0;         //
                public uint ID = 0;                 //
                public uint IFF = 0;                //0,1,2,3
                public float InitialPosX = 0;
                public float InitialPosY = 0;
                public float InitialPosZ = 0;
                public float InitialRotX = 0;       //Rot has no effect on aircraft?
                public float InitialRotY = 0;
                public float InitialRotZ = 0;
                public string ObjectName = "";      //63Char + null
                public uint GroID = 0;              //
                public uint GroFlag = 0;            //
                public string Unknown = "";         //
                public string OwnerName = "";       //55Char + null;


                public Process_Type05_EntityJoined()
                {
                }

                public Process_Type05_EntityJoined(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }

                public Process_Type05_EntityJoined(Packet InputPacket)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[InputPacket.Data.Length];
                    Data = InputPacket.Serialise().Skip(8).Take(Data.Length).ToArray();
                    //Logger.Console.WriteLine("&c" + Data.ToArray().ToDataString().CleanASCII());

                    Process(Data);
                }

                internal void Process(byte[] Data)
                {
                    //Actually build the packet...
                    ObjectType = Data.Take(4).ToArray().ToUint();
                    ID = Data.Skip(4).Take(4).ToArray().ToUint();
                    IFF = Data.Skip(8).Take(4).ToArray().ToUint();
                    InitialPosX = Data.Skip(12).Take(4).ToArray().ToFloat();
                    InitialPosY = Data.Skip(16).Take(4).ToArray().ToFloat();
                    InitialPosZ = Data.Skip(20).Take(4).ToArray().ToFloat();
                    InitialRotX = Data.Skip(24).Take(4).ToArray().ToFloat();
                    InitialRotY = Data.Skip(28).Take(4).ToArray().ToFloat();
                    InitialRotZ = Data.Skip(32).Take(4).ToArray().ToFloat();
                    ObjectName = Data.Skip(36).Take(64).ToArray().ToDataString().Split('\0')[0];
                    GroID = Data.Skip(100).Take(4).ToArray().ToUint();
                    GroFlag = Data.Skip(104).Take(4).ToArray().ToUint();
                    Unknown = Data.Skip(108).Take(16).ToArray().ToDataString();
                    OwnerName = Data.Skip(124).Take(56).ToArray().ToDataString().Split('\0')[0];
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("ObjectType: " + ObjectType.ToString().Clean());
                    Logger.Console.WriteLine("ID: " + ID.ToString().Clean());
                    Logger.Console.WriteLine("IFF: " + IFF.ToString().Clean());
                    Logger.Console.WriteLine("InitialPosX: " + InitialPosX.ToString().Clean());
                    Logger.Console.WriteLine("InitialPosY: " + InitialPosY.ToString().Clean());
                    Logger.Console.WriteLine("InitialPosZ: " + InitialPosZ.ToString().Clean());
                    Logger.Console.WriteLine("InitialRotX: " + InitialRotX.ToString().Clean());
                    Logger.Console.WriteLine("InitialRotY: " + InitialRotY.ToString().Clean());
                    Logger.Console.WriteLine("InitialRotZ: " + InitialRotZ.ToString().Clean());
                    Logger.Console.WriteLine("ObjectName: " + ObjectName.Clean());
                    Logger.Console.WriteLine("GroID: " + GroID.ToString().Clean());
                    Logger.Console.WriteLine("GroFlag: " + GroFlag.ToString().Clean());
                    Logger.Console.WriteLine("Unknown: " + Unknown.ToString().Clean());
                    Logger.Console.WriteLine("OwnerName: " + OwnerName.Clean());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(ObjectType).ToDataString();
                    outdata += BitConverter.GetBytes(ID).ToDataString();
                    outdata += BitConverter.GetBytes(IFF).ToDataString();
                    outdata += BitConverter.GetBytes(InitialPosX).ToDataString();
                    outdata += BitConverter.GetBytes(InitialPosY).ToDataString();
                    outdata += BitConverter.GetBytes(InitialPosZ).ToDataString();
                    outdata += BitConverter.GetBytes(InitialRotX).ToDataString();
                    outdata += BitConverter.GetBytes(InitialRotY).ToDataString();
                    outdata += BitConverter.GetBytes(InitialRotZ).ToDataString();
                    outdata += ObjectName + new string('\0', 64 - ObjectName.Length);
                    outdata += BitConverter.GetBytes(GroID).ToDataString();
                    outdata += BitConverter.GetBytes(GroFlag).ToDataString();
                    outdata += Unknown + new string('\0', 16 - Unknown.Length);
                    outdata += OwnerName + new string('\0', 56 - OwnerName.Length);
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
