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
            public partial class Type36_WeaponConfig : CommonPacket
            {
                public uint Type = 7;

                public uint ID;
                public ushort Unused_01;

                public Dictionary<ushort, ushort> Weapons;


                public Type36_WeaponConfig()
                {
                }

                public Type36_WeaponConfig(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }

                public Type36_WeaponConfig(Packet InputPacket)
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
                    ID = Data.Take(4).ToArray().ToUint();
                    Unused_01 = Data.Skip(4).Take(2).ToArray().ToUshort();
                    Weapons = new Dictionary<ushort, ushort>();
                    for (int i=6; i <= Data.Length - 4; i+=4) {
                        try
                        {
                            Weapons.Add(Data.Skip(i).Take(2).ToArray().ToUshort(), Data.Skip(i).Skip(2).Take(2).ToArray().ToUshort());
                        }
                        catch
                        {
                        }
                    }
                }

                internal void PrintDebugInfo()
                {
                    Logger.Console.WriteLine("&bWeaponConfig Debug Info");
                    Logger.Console.WriteLine("    ID: " + ID.ToString().Clean());
                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        switch (Weapons.Keys.ToList()[i])
                        {
                            case 1:
                                Logger.Console.WriteLine("    AAM(Short): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 2:
                                Logger.Console.WriteLine("    AGM: " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 3:
                                Logger.Console.WriteLine("    Bomb(500lb): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 4:
                                Logger.Console.WriteLine("    Rocket: " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 6:
                                Logger.Console.WriteLine("    AAM(Mid): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 7:
                                Logger.Console.WriteLine("    Bomb(250lb): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 8:
                                if (Weapons.Values.ToList()[i] == 1) Logger.Console.WriteLine("    Smoke: Loaded");
                                else if (Weapons.Values.ToList()[i] == 0) Logger.Console.WriteLine("    Smoke: Unloaded");
                                else Logger.Console.WriteLine("    Smoke: " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 9:
                                Logger.Console.WriteLine("    Bomb-HD(500lb): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 10:
                                Logger.Console.WriteLine("    A-AAM(Short): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 12:
                                Logger.Console.WriteLine("    FuelTank(Litres): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            case 200:
                                Logger.Console.WriteLine("    Flare: " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;
                            default:
                                Logger.Console.WriteLine("    Unknown(" + Weapons.Keys.ToList()[i].ToString() + "): " + Weapons.Values.ToList()[i].ToString().Clean());
                                break;

                        }
                    }

                }

                internal Network.Packet Serialise()
                {
                    Network.Packet outpack = new Network.Packet();
                    string outdata = "";
                    outdata += BitConverter.GetBytes(ID).ToDataString();
                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        outdata += BitConverter.GetBytes(Weapons.Keys.ToList()[i]).ToDataString();
                        outdata += BitConverter.GetBytes(Weapons.Values.ToList()[i]).ToDataString();
                    }
                    outpack.Data = outdata;
                    outpack.Type = Type;
                    return outpack;
                }
            }
        }
    }
}
