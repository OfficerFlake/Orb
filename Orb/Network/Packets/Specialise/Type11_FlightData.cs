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
            public partial class Type11_FlightData : CommonPacket
            {
                public uint Type = 11;

                //MORE DATA IS TO BE ADDED TO THIS PACKET.
                public float TimeStamp = 0;
                public int ID = 0;
                public short Version = 0;  //0,1,2 outdated.
                                           //3 normal format
                                           //4 full format
                                           //5 medium format
                public double PosX = 0;
                public double PosY = 0;
                public double PosZ = 0;
                public double HdgX = 0;     //Heading
                public double HdgY = 0;     //Pitch
                public double HdgZ = 0;     //Bank
                public double V_PosX = 0;
                public double V_PosY = 0;
                public double V_PosZ = 0;
                public double V_HdgX = 0;
                public double V_HdgY = 0;
                public double V_HdgZ = 0;

                public double Ammo_SmokeOil = 0;
                public double Weight_Fuel = 0;
                public double Weight_Payload = 0;

                public double FlightState = 0;      //0: Airbourne, 1: Landed, 6: Stopped.
                public byte Anim_VGW = 0;           //0=Extend, 255=Fold
                public byte Anim_Boards = 0;        //0=Fold, 15=Extend
                public byte Anim_Gear = 0;          //0=Fold, 15=Extend
                public byte Anim_Flaps = 0;         //0=Fold, 15=Extend
                public byte Anim_Brake = 0;         //0=Off, 15=On

                public byte Anim_Flags = 0;         //Animations like guns, smoke, lights, etc.
                public bool Anim_Light_Land
                {
                    get
                    {
                        return Anim_Flags.GetBit(7);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(7);
                        else Anim_Flags.UnSetBit(7);
                    }
                }      //1000 0000 - Landing Lights
                public bool Anim_Light_Strobe
                {
                    get
                    {
                        return Anim_Flags.GetBit(6);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(6);
                        else Anim_Flags.UnSetBit(6);
                    }
                }    //0100 0000 - Strobe Lights
                public bool Anim_Light_Nav
                {
                    get
                    {
                        return Anim_Flags.GetBit(5);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(5);
                        else Anim_Flags.UnSetBit(5);
                    }
                }       //0010 0000 - Navigation Lights
                public bool Anim_Light_Beacon
                {
                    get
                    {
                        return Anim_Flags.GetBit(4);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(4);
                        else Anim_Flags.UnSetBit(4);
                    }
                }    //0001 0000 - Beacon Lights
                public bool Anim_Guns
                {
                    get
                    {
                        return Anim_Flags.GetBit(3);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(3);
                        else Anim_Flags.UnSetBit(3);
                    }
                }            //0000 1000 - Guns
                public bool Anim_Contrails
                {
                    get
                    {
                        return Anim_Flags.GetBit(2);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(2);
                        else Anim_Flags.UnSetBit(2);
                    }
                }       //0000 0100 - Contrails
                public bool Anim_Smoke
                {
                    get
                    {
                        return Anim_Flags.GetBit(1);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(1);
                        else Anim_Flags.UnSetBit(1);
                    }
                }           //0000 0010 - Smoke
                public bool Anim_Burners
                {
                    get
                    {
                        return Anim_Flags.GetBit(0);
                    }
                    set
                    {
                        if (value) Anim_Flags.SetBit(0);
                        else Anim_Flags.UnSetBit(0);
                    }
                }         //0000 0001 - Burners

                public byte CPU_Flags = 0;

                public double Ammo_Gun = 0;
                public double Ammo_Rocket = 0;
                public double Ammo_AAM = 0;
                public double Ammo_AGM = 0;
                public double Ammo_B500 = 0;
                public double Health = 0;

                public double gForce = 0;

                public byte Anim_Throttle = 0;
                public sbyte Anim_Elevator = 0;
                public sbyte Anim_Aileron = 0;
                public sbyte Anim_Rudder = 0;
                public sbyte Anim_Trim = 0;

                public byte Anim_ThrustVector = 0;
                public byte Anim_BombBay = 0;

                public byte Anim_ThrustReverse = 0;

                public Type11_FlightData()
                {
                }

                public Type11_FlightData(byte[] Input)
                {
                    //Size is an auto generated method.
                    //Type is always 1 for this packet type.
                    //Generate data
                    Data = new byte[Input.Length - 8];
                    System.Buffer.BlockCopy(Input, 0, Data, 8, Input.Length-8);

                    Process(Data);
                }

                public Type11_FlightData(Packet InputPacket)
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
                    TimeStamp =     Data.Skip(00).Take(04).ToArray().ToFloat(); //confirmed
                    ID =            Data.Skip(04).Take(04).ToArray().ToInt(); //confirmed
                    Version =       Data.Skip(08).Take(02).ToArray().ToShort(); //confirmed

                    switch (Version)
                    {
                        case 3:
                            Handle_Version_3(); //LONG Format
                            break;
                        case 4:
                            Handle_Version_4(); //SHORT Format, w/Thrust Vector and B/Bay
                            break;
                        case 5:
                            Handle_Version_5(); //SHORT Format
                            break;
                    }
                }

                internal void Handle_Version_3()
                {
                    //Logger.Console.Debug("&cBLOCK 1");
                    PosX = Data.Skip(12).Take(04).ToArray().ToFloat(); //confirmed
                    PosY = Data.Skip(16).Take(04).ToArray().ToFloat(); //confirmed
                    PosZ = Data.Skip(20).Take(04).ToArray().ToFloat(); //confirmed
                    HdgX = Data.Skip(24).Take(02).ToArray().ToShort(); //confirmed
                    HdgY = Data.Skip(26).Take(02).ToArray().ToShort(); //confirmed
                    HdgZ = Data.Skip(28).Take(02).ToArray().ToShort(); //confirmed

                    //Logger.Console.Debug("&cBLOCK 2");
                    V_PosX = Data.Skip(30).Take(02).ToArray().ToShort(); //confirmed, EW
                    V_PosY = Data.Skip(32).Take(02).ToArray().ToShort(); //confirmed, ALT
                    V_PosZ = Data.Skip(34).Take(02).ToArray().ToShort(); //confirmed, NS
                    V_HdgX = Data.Skip(36).Take(02).ToArray().ToShort(); //confirmed, pitch
                    V_HdgY = Data.Skip(38).Take(02).ToArray().ToShort(); //confirmed, yaw
                    V_HdgZ = Data.Skip(40).Take(02).ToArray().ToShort(); //confirmed, bank

                    //Logger.Console.Debug("&cBLOCK 3");
                    gForce = Data.Skip(42).Take(02).ToArray().ToShort();

                    return;
                    //BROKEN BELOW, DO NOT TOUCH.

                    Ammo_Gun = Data.Skip(44).Take(02).ToArray().ToShort();
                    Ammo_AAM = Data.Skip(46).Take(02).ToArray().ToShort();
                    Ammo_AGM = Data.Skip(48).Take(02).ToArray().ToShort();
                    Ammo_B500 = Data.Skip(50).Take(02).ToArray().ToShort();
                    Ammo_SmokeOil = Data.Skip(52).Take(02).ToArray().ToShort();

                    //Logger.Console.Debug("&cBLOCK 4");
                    Weight_Fuel = Data.Skip(54).Take(04).ToArray().ToFloat();
                    Weight_Payload = Data.Skip(58).Take(04).ToArray().ToFloat();

                    //Logger.Console.Debug("&cBLOCK 5");
                    Health = Data.Skip(62).Take(02).ToArray().ToShort();
                    FlightState = Data.Skip(64).Take(02).ToArray().ToShort();

                    //Logger.Console.Debug("&cBLOCK 6");
                    Anim_VGW = Data.Skip(66).Take(01).ToArray().ToByte();
                    Anim_Boards = Data.Skip(67).Take(01).ToArray().ToByte().GetTens(); //Confirmed
                    Anim_Gear = Data.Skip(67).Take(01).ToArray().ToByte().GetUnits(); //Confirmed
                    Anim_Flaps = Data.Skip(68).Take(01).ToArray().ToByte().GetTens(); //Confirmed
                    Anim_Brake = Data.Skip(68).Take(01).ToArray().ToByte().GetUnits(); //Confirmed

                    //Logger.Console.Debug("&cBLOCK 7");
                    Anim_Flags = Data.Skip(69).Take(01).ToArray().ToByte(); //Confirmed, specific sub elements are handled by getter/setters, see declerations above..
                    CPU_Flags = Data.Skip(70).Take(01).ToArray().ToByte(); //??? Does this even HAVE a use?

                    //Logger.Console.Debug("&cBLOCK 8");
                    Anim_Throttle = Data.Skip(71).Take(01).ToArray().ToByte(); //Confirmed, 0-100 in decimal.
                    Anim_Elevator = Data.Skip(72).Take(01).ToArray().ToSbyte(); //Confirmed. PULL 0->+100 PUSH -1->-100
                    Anim_Aileron = Data.Skip(73).Take(01).ToArray().ToSbyte(); //Confirmed. LEFT 0->+100 RIGHT -1->-100
                    Anim_Rudder = Data.Skip(74).Take(01).ToArray().ToSbyte(); //Confirmed. LEFT 0->+100 RIGHT -1->-100
                    Anim_Trim = Data.Skip(75).Take(01).ToArray().ToSbyte(); //Confirmed. PULL 0->+100 PUSH -1->-100

                    //Logger.Console.Debug("&cBLOCK 9");
                    Ammo_Rocket = Data.Skip(76).Take(02).ToArray().ToShort();

                    //Logger.Console.Debug("&cBLOCK 10");
                    V_HdgX = Data.Skip(78).Take(04).ToArray().ToFloat();
                    V_HdgY = Data.Skip(82).Take(04).ToArray().ToFloat();
                    V_HdgZ = Data.Skip(86).Take(04).ToArray().ToFloat();

                    //Logger.Console.Debug("&cBLOCK 11");
                    Anim_ThrustVector = Data.Skip(90).Take(01).ToArray().ToByte().GetTens();
                    Anim_ThrustReverse = Data.Skip(90).Take(01).ToArray().ToByte().GetUnits();
                    Anim_BombBay = Data.Skip(91).Take(01).ToArray().ToByte();

                }

                internal void Handle_Version_4()
                {
                    Handle_Version_5();

                    Anim_ThrustVector = Data.Skip(68).Take(01).ToArray().ToByte().GetTens();
                    Anim_ThrustReverse = Data.Skip(68).Take(01).ToArray().ToByte().GetUnits();
                    Anim_BombBay = Data.Skip(69).Take(01).ToArray().ToByte();

                    //One Null To Terminate Here.
                }

                internal void Handle_Version_5()
                {
                    PosX = Data.Skip(10).Take(04).ToArray().ToFloat(); //confirmed
                    PosY = Data.Skip(14).Take(04).ToArray().ToFloat(); //confirmed
                    PosZ = Data.Skip(18).Take(04).ToArray().ToFloat(); //confirmed
                    HdgX = Data.Skip(22).Take(02).ToArray().ToShort(); //confirmed
                    HdgY = Data.Skip(24).Take(02).ToArray().ToShort(); //confirmed
                    HdgZ = Data.Skip(26).Take(02).ToArray().ToShort(); //confirmed

                    V_PosX = Data.Skip(28).Take(02).ToArray().ToShort(); //confirmed, EW
                    V_PosY = Data.Skip(30).Take(02).ToArray().ToShort(); //confirmed, ALT
                    V_PosZ = Data.Skip(32).Take(02).ToArray().ToShort(); //confirmed, NS
                    V_HdgX = Data.Skip(34).Take(02).ToArray().ToShort(); //confirmed, pitch
                    V_HdgY = Data.Skip(36).Take(02).ToArray().ToShort(); //confirmed, yaw
                    V_HdgZ = Data.Skip(38).Take(02).ToArray().ToShort(); //confirmed, bank

                    Ammo_SmokeOil = Data.Skip(40).Take(02).ToArray().ToShort(); //confirmed, Does the aircraft have SMOIL loaded?
                    Weight_Fuel = Data.Skip(42).Take(04).ToArray().ToInt(); //confirmed, WEIGHT of fuel loaded, where none is 0.
                    Weight_Payload = Data.Skip(46).Take(02).ToArray().ToShort(); //???, WEIGHT of payload, appearing as 0 often always...

                    FlightState = Data.Skip(48).Take(01).ToArray().ToByte(); //Confirmed
                    Anim_VGW = Data.Skip(49).Take(01).ToArray().ToByte(); //Confirmed

                    Anim_Boards = Data.Skip(50).Take(01).ToArray().ToByte().GetTens(); //Confirmed
                    Anim_Gear = Data.Skip(50).Take(01).ToArray().ToByte().GetUnits(); //Confirmed

                    Anim_Flaps = Data.Skip(51).Take(01).ToArray().ToByte().GetTens(); //Confirmed
                    Anim_Brake = Data.Skip(51).Take(01).ToArray().ToByte().GetUnits(); //Confirmed

                    Anim_Flags = Data.Skip(52).Take(01).ToArray().ToByte(); //Confirmed, specific sub elements are handled by getter/setters, see declerations above..
                    CPU_Flags = Data.Skip(53).Take(01).ToArray().ToByte(); //??? Does this even HAVE a use?

                    Ammo_Gun = Data.Skip(54).Take(02).ToArray().ToShort(); //Confirmed
                    Ammo_Rocket = Data.Skip(56).Take(02).ToArray().ToShort(); //Confirmed

                    Ammo_AAM = Data.Skip(58).Take(01).ToArray().ToByte();  //Confirmed, AAM-Short ONLY
                    Ammo_AGM = Data.Skip(59).Take(01).ToArray().ToByte(); //Confirmed, AGM ONLY
                    Ammo_B500 = Data.Skip(60).Take(01).ToArray().ToByte(); //Confirmed, B500 ONLY

                    Health = Data.Skip(61).Take(01).ToArray().ToByte(); //Confirmed, Health REMAINING.

                    gForce = (short)Data.Skip(62).Take(01).ToArray().ToSbyte(); //Confirmed, g+ 0-127, g- -1,-127. g/10 for hud.

                    Anim_Throttle = Data.Skip(63).Take(01).ToArray().ToByte(); //Confirmed, 0-100 in decimal.
                    Anim_Elevator = Data.Skip(64).Take(01).ToArray().ToSbyte(); //Confirmed. PULL 0->+100 PUSH -1->-100
                    Anim_Aileron = Data.Skip(65).Take(01).ToArray().ToSbyte(); //Confirmed. LEFT 0->+100 RIGHT -1->-100
                    Anim_Rudder = Data.Skip(66).Take(01).ToArray().ToSbyte(); //Confirmed. LEFT 0->+100 RIGHT -1->-100
                    Anim_Trim = Data.Skip(67).Take(01).ToArray().ToSbyte(); //Confirmed. PULL 0->+100 PUSH -1->-100

                    Anim_ThrustVector = 0;
                    Anim_ThrustReverse = 0;
                    Anim_BombBay = 0;
                    //One Null To Terminate Here.
                }

                public Network.Packet Serialise()
                {
                    Network.Packet OutPack = new Network.Packet();
                    OutPack.Type = Type;
                    OutPack.Data = BitConverter.GetBytes(TimeStamp).ToDataString();
                    OutPack.Data += BitConverter.GetBytes(ID).ToDataString();
                    OutPack.Data += BitConverter.GetBytes(Version).ToDataString();
                    switch (Version)
                    {
                        case 3:
                            OutPack.Data = Data.ToDataString();
                            break;
                        case 4:
                            OutPack = Serialise_Type5(OutPack);
                            OutPack.Data += ((byte)(((int)Anim_ThrustVector.ToTens()) + ((int)Anim_ThrustReverse))).ToByteArray().ToDataString();
                            OutPack.Data += Anim_BombBay.ToByteArray().ToDataString();
                            OutPack.Data += "\0";
                            break;
                        case 5:
                            OutPack = Serialise_Type5(OutPack);
                            OutPack.Data += "\0";
                            break;
                    }
                    return OutPack;
                }

                internal Network.Packet Serialise_Type5(Network.Packet OutPack)
                {
                    OutPack.Data += BitConverter.GetBytes((float)PosX).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((float)PosY).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((float)PosZ).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)HdgX).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)HdgY).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)HdgZ).ToDataString();

                    OutPack.Data += BitConverter.GetBytes((short)V_PosX).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)V_PosY).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)V_PosZ).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)V_HdgX).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)V_HdgY).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)V_HdgZ).ToDataString();

                    OutPack.Data += BitConverter.GetBytes((short)Ammo_SmokeOil).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((int)Weight_Fuel).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)Weight_Payload).ToDataString();

                    OutPack.Data += ((byte)FlightState).ToByteArray().ToDataString();
                    OutPack.Data += ((byte)Anim_VGW).ToByteArray().ToDataString();

                    OutPack.Data += ((byte)(((int)Anim_Boards.ToTens()) + ((int)Anim_Gear))).ToByteArray().ToDataString();
                    OutPack.Data += ((byte)(((int)Anim_Flaps.ToTens()) + ((int)Anim_Brake))).ToByteArray().ToDataString();

                    OutPack.Data += ((byte)Anim_Flags).ToByteArray().ToDataString();
                    OutPack.Data += ((byte)CPU_Flags).ToByteArray().ToDataString();

                    OutPack.Data += BitConverter.GetBytes((short)Ammo_Gun).ToDataString();
                    OutPack.Data += BitConverter.GetBytes((short)Ammo_Rocket).ToDataString();

                    OutPack.Data += ((byte)Ammo_AAM).ToByteArray().ToDataString();
                    OutPack.Data += ((byte)Ammo_AGM).ToByteArray().ToDataString();
                    OutPack.Data += ((byte)Ammo_B500).ToByteArray().ToDataString();

                    OutPack.Data += ((byte)Health).ToByteArray().ToDataString();
                    OutPack.Data += ((sbyte)gForce).ToByteArray().ToDataString();

                    OutPack.Data += ((byte)Anim_Throttle).ToByteArray().ToDataString();
                    OutPack.Data += ((sbyte)Anim_Elevator).ToByteArray().ToDataString();
                    OutPack.Data += ((sbyte)Anim_Aileron).ToByteArray().ToDataString();
                    OutPack.Data += ((sbyte)Anim_Rudder).ToByteArray().ToDataString();
                    OutPack.Data += ((sbyte)Anim_Trim).ToByteArray().ToDataString();

                    return OutPack;
                }
            }
        }
    }
}
