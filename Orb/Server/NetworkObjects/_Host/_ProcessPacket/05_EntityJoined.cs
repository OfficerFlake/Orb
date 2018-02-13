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
                public void Process_Type05_EntityJoined(Network.Packet Packet)
                {
                    Network.Packets.Process_Type05_EntityJoined ThisPacket = new Network.Packets.Process_Type05_EntityJoined(Packet);
                    if (ThisPacket.OwnerName != "" && Parent.Username.StartsWith(ThisPacket.OwnerName))
                    {
                        
                        //Parent.VehicleID = ThisPacket.ID;
                        Parent.Vehicle.ID = ThisPacket.ID;
                        Parent.Vehicle.Name = ThisPacket.ObjectName;
                        Parent.Vehicle.IFF = ThisPacket.IFF;
                        Parent.Vehicle.Type = ThisPacket.ObjectType;

                        Parent.Vehicle.PosX = ThisPacket.InitialPosX; //East/West
                        Parent.Vehicle.PosY = ThisPacket.InitialPosY; //Altitude
                        Parent.Vehicle.PosZ = ThisPacket.InitialPosZ; //North/South
                        Parent.Vehicle.HdgX = ThisPacket.InitialRotX; //Heading
                        Parent.Vehicle.HdgY = ThisPacket.InitialRotY; //Pitch
                        Parent.Vehicle.HdgZ = ThisPacket.InitialRotZ; //Roll

                        //ThisPacket.InitialRotZ = (float)(ThisPacket.InitialRotZ + (-90).ToRadians());
                        //ThisPacket.InitialPosY = 3000;

                        Parent.UserObject.FlightsFlown++;
                        Server.Vehicles.Add(Parent.Vehicle);
                        Parent.UserObject.Save(Database.UserDB.Strings.FlightsFlown);
                        Server.AllClients.SendMessage(Parent.Username + " took off (" + ThisPacket.ObjectName + ")");
                    }
                    else
                    {
                        Server.NetObject Owner = null;
                    }
                    Parent.ClientObject.Send(ThisPacket.Serialise());
                }
            }
        }
    }
}