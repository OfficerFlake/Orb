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
            public partial class Client
            {
                public void ProcessPacket(Network.Packet Packet)
                {
                    if ((Packet.Type != 44 & Packet.Type != 11 & Packet.Type != 21 & Packet.Type != 5))
                    {
                        if (Packet.Type == 6)
                        {
                            Network.Packets.Type06_AckSetting Ack = new Network.Packets.Type06_AckSetting(Packet);
                            Logger.Console.WriteLine("&b" + Packet.Type.ToString() + "(" + Ack.Parameter1 + ":" + Ack.Parameter2 + ")");
                        }
                    }

                    switch (Packet.Type)
                    {
                        case Network.Packets.Types.Login: Process_Type01_Login(Packet); break;
                        case Network.Packets.Types.Error: Process_Type03_Error(Packet); break;
                        case Network.Packets.Types.Map: Process_Type04_Map(Packet); break;
                        case Network.Packets.Types.AcknowledgeSetting: Process_Type06_AckSetting(Packet); break;
                        case Network.Packets.Types.SmokeCol: Process_Type07_SmokeCol(Packet); break;
                        case Network.Packets.Types.JoinRequest: Process_Type08_JoinRequest(Packet); break;
                        case Network.Packets.Types.FlightData: Process_Type11_FlightData(Packet); break;
                        case Network.Packets.Types.LeaveFlight: Process_Type12_Unjoin(Packet); break;
                        case Network.Packets.Types.DestoryAircraft: Process_Type13_RemoveAircraft(Packet); break;
                        case Network.Packets.Types.EndAircraftList: Process_Type16_PrepareSimulation(Packet); break;
                        case Network.Packets.Types.Damage: Process_Type22_Damage(Packet); break;
                        case Network.Packets.Types.MissilesOption: Process_Type31_AllowMissiles(Packet); break;
                        case Network.Packets.Types.Chat: Process_Type32_ChatMessage(Packet); break;
                        case Network.Packets.Types.Weather: Process_Type33_Weather(Packet); break;
                        case Network.Packets.Types.MiscCommand: Process_Type43_MiscCmd(Packet); break;
                        case Network.Packets.Types.AircraftLoading: Process_Type36_WeaponConfig(Packet); break;
                        case Network.Packets.Types.WeaponsOption: Process_Type39_AllowUnguided(Packet); break;
                        case Network.Packets.Types.UsernameDistance: Process_Type41_ShowUsername(Packet); break;
                        case Network.Packets.Types.Airstate: Process_Type38_QueryAirstate(Packet); break;
                        case Network.Packets.Types.AircraftList: Process_Type44_AircraftList(Packet); break;
                        case Network.Packets.Types.Heartbeat: Process_Type17_Heartbeat(Packet); break;
                        default:
                            //Logger.Console.WriteLine("&bCLIENT-> Next Packet:");
                            //Logger.Console.WriteLine("&b\tType: " + Packet.Type.ToString());
                            //Logger.Console.WriteLine("&b\tData: " + BitConverter.ToString(Packet.Serialise().Skip(8).ToArray()));
                            //Logger.Console.WriteLine("&b\tData(String): " + Packet.Serialise().Skip(8).ToArray().ToDataString().CleanASCII());
                            Parent.HostObject.Send(Packet);
                            //Parent.Close();
                            break;
                    }
                    Parent.LastPacket = Packet;
                    Parent.PacketWaiter.Set();
                }
            }
        }
    }
}