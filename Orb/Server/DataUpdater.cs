using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
namespace Orb
{
    public static partial class Server
    {
        public static void DataUpdateThread()
        {
            DateTime LastItertion = DateTime.Now;
            while (true)
            {
                foreach (NetObject ThisNetObj in ClientList.ToArray())
                {
                    if (ThisNetObj.Vehicle.ID != 0)
                    {
                        ThisNetObj.UserObject.FlightHours += DateTime.Now - LastItertion; //update flighttime if the user is flying.
                        ThisNetObj.UserObject.Save(Database.UserDB.Strings.FlightHours);
                    }
                    bool temp;
                    temp = ThisNetObj.UserObject.IsMuted; //Updates the muted flag if necessary.
                    temp = ThisNetObj.UserObject.IsBanned; //Updates the banned flag if necessary
                    ThisNetObj.UserObject.PlayTime += DateTime.Now - LastItertion; //update total playtime.
                    ThisNetObj.UserObject.Save(Database.UserDB.Strings.PlayTime);
                }
                LastItertion = DateTime.Now;
                Thread.Sleep(1000);
                
                
                //Update frequency can be adjusted depending on server load if you need it just uncomment below line.
                //if (Server.ClientList.Count() >= 1) Thread.Sleep((Server.ClientList.Count()-1) * 1000);
            }
        }
    }
}