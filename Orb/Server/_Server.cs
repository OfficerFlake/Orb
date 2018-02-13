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
        public static IPEndPoint ServerAddress;
        public static List<NetObject> ClientList = new List<NetObject>();
        public static List<NetObject> AllClients
        {
            get
            {
                List<NetObject> Output = new List<NetObject>();
                Output.AddRange(ClientList);
                Output.Add(OrbConsole);
                return Output;
            }
        }
        public static List<Vehicle> Vehicles = new List<Vehicle>();

        public static Network.Packets.Type33_Weather Weather = new Network.Packets.Type33_Weather();
        public static Network.Packets.Type33_Weather VanillaWeather = new Network.Packets.Type33_Weather();

        public static Thread DataUpdater = new Thread(() => DataUpdateThread());

        public static NetObject OrbConsole = new NetObject();
        public static bool ConsoleMode = true;

        public static void Send(this List<NetObject> Clients, Network.Packet ThisPacket)
        {
            foreach (NetObject ThisClient in Clients.ToArray()) //ToArray to make a standalone, uneditable copy to iterate over.
            {
                try
                {
                    ThisClient.ClientObject.Send(ThisPacket);
                }
                catch
                {
                    //Unable to send, client disconnected?
                    if (!(ThisClient.ClientObject.ClientSocket.Connected && ThisClient.HostObject.HostSocket.Connected))
                    {
                        ThisClient.Close();
                    }
                }
            }
        }
        public static void SendMessage(this List<NetObject> Clients, String Message)
        {
            foreach (NetObject ThisClient in Clients.ToArray()) //ToArray to make a standalone, uneditable copy to iterate over.
            {
                try
                {
                    ThisClient.ClientObject.SendMessage(Message);
                }
                catch
                {
                    //Unable to send, client disconnected?
                    if (!(ThisClient.ClientObject.ClientSocket.Connected && ThisClient.HostObject.HostSocket.Connected))
                    {
                        ThisClient.Close();
                    }
                }
            }
            //Logger.Console.WriteLine(Message);
        }

        public static List<NetObject> EmptyClientList
        {
            get {
                return new List<NetObject>();
            }
        }
 
        public static void Start()
        {
            //Logger.Console.WriteLine(Database.UserDB.ListToString(Database.UserDB.List));
            //Set Weather
            Weather.RawOptions = "11101001".FromBinaryStringToUint();
            Weather.Lighting = 1;
            Weather.Blackout = false;
            Weather.Collisions = false;
            Weather.LandEverywhere = true;
            Weather.Fog = 50000;
            Weather.WindX = 0;
            Weather.WindY = 0;
            Weather.WindZ = 0;

            //OrbConsole
            OrbConsole.UserObject = Database.UserDB.SuperUser;
            OrbConsole.UserObject.DisplayedName = "&5[SERVER]&9Orb";
            OrbConsole.ClientObject = new Server.NetObject.Client();
            OrbConsole.ClientObject.Create(null, OrbConsole);
            OrbConsole.HostObject = new Server.NetObject.Host();
            OrbConsole.HostObject.Create(OrbConsole);
            OrbConsole.Username = OrbConsole.UserObject.Name;
            OrbConsole.UserObject.Permissions = new Database.PermissionDB.Permission();
            OrbConsole.UserObject.Permissions.MakeSuper(); //Orb console should ALWAYS be able to do EVERYTHING.

            //Console.WriteLine("Orb.Server");
            //Logger.Console.WriteLine(DateTime.Now.ToString());
            Server.DataUpdater.Start();
            Server.ConnectionMainframe.Start();

        }
    }
}
