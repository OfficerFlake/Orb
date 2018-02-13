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
            public partial class Type44_AircraftList : CommonPacket
            {
                public uint Type = 44;

                public byte ListVersion;
                public byte AircraftCount;
                public ushort TwoNulls;
                public List<String> AircraftList = new List<String>();

                public Type44_AircraftList()
                {
                }
                public Type44_AircraftList(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }
                public Type44_AircraftList(Packet InputPacket)
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
                    ListVersion = Data.Take(1).ToArray()[0];
                    AircraftCount = Data.Skip(1).Take(1).ToArray()[0];
                    TwoNulls = Data.Skip(2).Take(2).ToArray().ToUshort();
                    AircraftList = Data.Skip(4).ToArray().ToDataString().Split('\0').ToList();
                    AircraftList.RemoveAt(AircraftList.Count - 1);
                    //Logger.Console.WriteLine("&aAircraft List: " + AircraftCount + " Aircraft in the packet. (" + AircraftList.Count + ")");
                    //Console.WriteLine(AircraftList.ToStringList());
                }
            }
        }
    }
}
