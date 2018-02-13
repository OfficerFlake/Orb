using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Orb
{
    public static partial class Server
    {
        public partial class NetObject
        {
            public partial class Host
            {
                public void ProcessPacket(Network.Packet Packet)
                {
                    if (true)
                    {
                        if (Packet.Type == 21)
                        {
                            Logger.Console.Clear();
                            Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "\n" + Packet.Data.ToByteArray().ToDebugHexString());
                            Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "\n" + Packet.Data);
                        }
                        if (Packet.Type == 30)
                        {
                            //Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "\n" + Packet.Data.ToByteArray().ToDebugHexString());
                            //Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "\n" + Packet.Data);
                        }
                        if (Packet.Type == 6)
                        {
                            Network.Packets.Type06_AckSetting Ack = new Network.Packets.Type06_AckSetting(Packet);
                            Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "(" + Ack.Parameter1 + ":" + Ack.Parameter2 + ")");
                        }
                        if (Packet.Type == 13)
                        {
                            //Network.Packets.Type13_RemoveAircraft RA = new Network.Packets.Type13_RemoveAircraft(Packet);
                            //Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "(" + RA.ID + ")");
                        }
                        if (Packet.Type == 12)
                        {
                            //Network.Packets.Type12_Unjoin UJ = new Network.Packets.Type12_Unjoin(Packet);
                            //Logger.Console.WriteLine("&e" + Packet.Type.ToString() + "(" + UJ.ID + ")");
                        }
                        else
                        {
                            //Logger.Console.WriteLine("&dSERVER(" + Parent.Username + ")-> Next Packet:\n&d\tType: " + Packet.Type.ToString() + "\n&d\tData: " + BitConverter.ToString(Packet.Serialise().Skip(8).ToArray()) + "&d\n\tData(String): " + Packet.Serialise().Skip(8).ToArray().ToDataString().CleanASCII() + "&d\n\tData(Hex): " + Packet.Serialise().Skip(8).ToArray().ToDebugHexString());
                        }
                        //Logger.Console.WriteLine("&e" + Packet.Type.ToString());
                        //Thread.Sleep(250);
                    }

                    switch (Packet.Type)
                    {
                        case Network.Packets.Types.Map: Process_Type04_Map(Packet); break;
                        case Network.Packets.Types.CreateVehicle: Process_Type05_EntityJoined(Packet); break;
                        case Network.Packets.Types.AcknowledgeSetting: Process_Type06_AckSetting(Packet); break;
                        case Network.Packets.Types.JoinApproved: Process_Type09_JoinApproved(Packet); break;
                        case Network.Packets.Types.FlightData: Process_Type11_FlightData(Packet); break;
                        case Network.Packets.Types.DestoryAircraft: Process_Type13_RemoveAircraft(Packet); break;
                        case Network.Packets.Types.EndAircraftList: Process_Type16_PrepareSimulation(Packet); break;
                        case Network.Packets.Types.ServerVersion: Process_Type29_VersionNotify(Packet); break;
                        case Network.Packets.Types.MissilesOption: Process_Type31_AllowMissiles(Packet); break;
                        case Network.Packets.Types.Chat: Process_Type32_ChatMessage(Packet); break;
                        case Network.Packets.Types.Weather: Process_Type33_Weather(Packet); break;
                        case Network.Packets.Types.AircraftLoading: Process_Type36_WeaponConfig(Packet); break;
                        case Network.Packets.Types.WeaponsOption: Process_Type39_AllowUnguided(Packet); break;
                        case Network.Packets.Types.MiscCommand: Process_Type43_MiscCmd(Packet); break;
                        case Network.Packets.Types.UsernameDistance: Process_Type41_ShowUsername(Packet); break;
                        case Network.Packets.Types.AircraftList: Process_Type44_AircraftList(Packet); break;
                        case Network.Packets.Types.AircraftConfiguration: Parent.ClientObject.Send(Packet); break;
                        default:
                            //Logger.Console.WriteLine("&dSERVER-> Next Packet:");
                            //Logger.Console.WriteLine("&d\tType: " + Packet.Type.ToString());
                            //Logger.Console.WriteLine("&d\tData: " + BitConverter.ToString(Packet.Serialise().Skip(8).ToArray()));
                            //Logger.Console.WriteLine("&d\tData(String): " + Packet.Serialise().Skip(8).ToArray().ToDataString().CleanASCII());
                            Parent.ClientObject.Send(Packet);
                            //Parent.Close();
                            break;
                    }
                }
            }
        }
    }
}