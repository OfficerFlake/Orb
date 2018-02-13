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
            public partial class Type38_QueryAirstate : CommonPacket
            {
                public uint Type = 38;

                public uint AircraftCount;
                public List<UInt32> AircraftIDs = new List<UInt32>();

                public Type38_QueryAirstate()	
                {
                }
                public Type38_QueryAirstate(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }
                public Type38_QueryAirstate(Packet InputPacket)
                {
                    //Size is an auto generated method.
                    //Type is always 43 for this packet type.
                    //Generate data
                    Data = new byte[InputPacket.Data.Length];
                    Data = InputPacket.Serialise().Skip(8).Take(Data.Length).ToArray();

                    //Actually build the packet...
                    Process(Data);
                }

                internal void Process(byte[] Data) {
                    AircraftCount = Data.Take(4).ToArray().ToUint();
                    byte[] DataIterable = Data.Skip(4).ToArray();
                    for (int i = 0; i < AircraftCount; i++)
                    {
                        try
                        {
                            uint ThisID = DataIterable.Take(4).ToArray().ToUint();
                            AircraftIDs.Add(ThisID);
                            DataIterable = DataIterable.Skip(4).ToArray();
                        }
                        catch
                        {
                            break;
                        }
                    }
                    //Console.WriteLine(AircraftList.ToStringList());
                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(AircraftCount).ToDataString();
                    foreach(uint ID in AircraftIDs)
                    {
                        outdata += BitConverter.GetBytes(ID).ToDataString();
                    }
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
