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
                public void Process_Type11_FlightData(Network.Packet Packet)
                {
                    Network.Packets.Type11_FlightData ThisPacket = new Network.Packets.Type11_FlightData(Packet);

                    //if this is data packet is for this vehicle
                    if (Parent.Vehicle.ID == ThisPacket.ID)
                    {
                        Parent.Vehicle.HdgX = ThisPacket.HdgX;
                        Parent.Vehicle.HdgY = ThisPacket.HdgY;
                        Parent.Vehicle.HdgZ = ThisPacket.HdgZ;
                        Parent.Vehicle.PosX = ThisPacket.PosX;
                        Parent.Vehicle.PosY = ThisPacket.PosY;
                        Parent.Vehicle.PosZ = ThisPacket.PosZ;

                        Parent.Vehicle.V_HdgX = ThisPacket.V_HdgX;
                        Parent.Vehicle.V_HdgY = ThisPacket.V_HdgY;
                        Parent.Vehicle.V_HdgZ = ThisPacket.V_HdgZ;
                        Parent.Vehicle.V_PosX = ThisPacket.V_PosX;
                        Parent.Vehicle.V_PosY = ThisPacket.V_PosY;
                        Parent.Vehicle.V_PosZ = ThisPacket.V_PosZ;
                    }

                    /*
                    foreach (Server.NetObject ThisNetObj in Server.ClientList)
                    {
                        #region This Client will attempt to form
                        //if has legit form target
                        if (ThisNetObj.Vehicle.FormTarget != 0 && Server.Vehicles.Where(x => x.ID == ThisNetObj.Vehicle.FormTarget).Count() > 0)
                        {

                            //if this is data packet is for this vehicle
                            #region FlightDataPacket is from this client
                            if (!ThisPacket.Anim_Light_Strobe)
                            {
                                Parent.HostObject.Send(Packet);
                                Logger.Console.WriteLine("Lights Off, send normal packet.");
                            }
                            #endregion
                        }
                        #endregion

                        #region Flight Data does not belong to this client
                        if (ThisNetObj.Vehicle.ID != ThisPacket.ID)
                        {
                            /*
                            #region Update all Formation Clients
                            foreach (Server.Vehicle ThisVehicle in Server.Vehicles.Where(x => x.FormTarget == ThisNetObj.Vehicle.FormTarget))
                            {
                                ThisPacket = new Network.Packets.Type11_FlightData(Packet);
                                ThisPacket.PosX -= ThisVehicle.FormX;
                                ThisPacket.PosY -= ThisVehicle.FormY;
                                ThisPacket.PosZ -= ThisVehicle.FormZ;
                                Parent.HostObject.Send(ThisPacket.Serialise());
                                Parent.ClientObject.Send(ThisPacket.Serialise());
                                //Logger.Console.WriteLine("Updated A Formation Aircraft");
                            }
                            #endregion
                            /
                            //Send own update
                            Parent.HostObject.Send(Packet);
                            return;
                        }
                        #endregion
                    }
                    */

                    //No formations at all - just send it away!
                    Parent.HostObject.Send(ThisPacket.Serialise());

                    /*
                    Logger.Console.WriteLine("&e" + ThisPacket.TimeStamp.ToString());
                    if (ThisPacket.ID == Parent.Vehicle.ID)
                    {
                        Logger.Console.WriteLine("&e####" + ThisPacket.Data.ToHexString());
                    }
                    else
                    {
                        Logger.Console.WriteLine("&e$$$$" + ThisPacket.Data.ToHexString());
                    }
                    */
                    //Parent.HostObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}