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
            #region Variables
            public NetObject.Host HostObject;
            public NetObject.Client ClientObject;
            public Database.UserDB.User UserObject;
            public AutoResetEvent CommandConfirmation = new AutoResetEvent(false);
            public bool GetCommandConfirmation(int Timeout)
            {
                return CommandConfirmation.WaitOne(Timeout);
            }
            public string LastTypedCommand = "";
            public List<AutoResetEvent> TextWaiters = new List<AutoResetEvent>();
            public string TextInput = "";
            public string Username = "Connecting...";
            public uint Version = 20110207;
            public Server.Vehicle Vehicle = new Server.Vehicle();
            public string Smoke = "";
            public class _CommandHandling
            {
                public Database.UserDB.User PreviousUser = Database.UserDB.Nobody;
                public Database.GroupDB.Group PreviousGroup = Database.GroupDB.NoGroup;
            }
            public _CommandHandling CommandHandling = new _CommandHandling();
            public AutoResetEvent PacketWaiter = new AutoResetEvent(false);
            public Network.Packet LastPacket = new Network.Packet();
            #endregion

            public void Create(Socket clientsock)
            {
                UserObject = Database.UserDB.Connecting;
                ClientObject = new NetObject.Client();
                HostObject = new NetObject.Host();

                ClientObject.Create(clientsock, this);
                HostObject.Create(this);

                if (HostObject.HostSocket == null)
                {
                    Close();
                    return;
                }

                //Console.WriteLine("New Client Created.");

                ClientObject.Start();
                HostObject.Start();

                //Console.WriteLine("Client Threads Started.");
            }

            public void Close()
            {
                //Console.WriteLine("Close() has been called.");
                try { ClientObject.ClientSocket.Shutdown(SocketShutdown.Send); }
                catch (Exception e) { /*Logger.Log.Bug(e, "ClientSocket.Shutdown");*/ }

                try { HostObject.HostSocket.Shutdown(SocketShutdown.Send); }
                catch (Exception e) { /*Logger.Log.Bug(e, "HostSocket.Shutdown");*/ }

                try { ClientObject.ClientSocket.Close(); }
                catch (Exception e) { /*Logger.Log.Bug(e, "ClientSocket.Close");*/ }

                try { HostObject.HostSocket.Close(); }
                catch (Exception e) { /*Logger.Log.Bug(e, "HostSocket.Close");*/ }

                bool WasOnline = false;
                while (Server.ClientList.Contains(this)) {
                    WasOnline = true;
                    try
                    {
                        Server.ClientList.Remove(this);
                    }
                    catch (Exception e) { Logger.Log.Bug(e, "ClientList.Remove"); }
                }
                if (WasOnline)
                {
                    if (Username.ToUpperInvariant() != "PHP BOT")
                    {
                        ClientList.SendMessage(this.Username + " has left the server.");
                        Logger.Console.WriteLine("&eClient " + this.Username + " disconnected.");
                    }
                    ServerGUI.RefreshUsers();
                }
                
            }
        }

        public static List<NetObject> Except(this List<NetObject> List, NetObject ExceptThis)
        {
            while (true)
            {
                if (List.Contains(ExceptThis)) List.Remove(ExceptThis);
                else break;
            }
            return List;
        }
        public static List<NetObject> Except(this List<NetObject> List, IEnumerable<NetObject> ExceptThis)
        {
            IEnumerable<NetObject> Iterable = ExceptThis.ToArray().ToList();
            foreach (NetObject ThisNetObj in Iterable)
            {
                while (true)
                {
                    if (List.Contains(ThisNetObj)) List.Remove(ThisNetObj);
                    else break;
                }
            }
            return List;
        }
        public static List<NetObject> Except(this List<NetObject> List, Database.UserDB.User ExceptThis)
        {
            IEnumerable<NetObject> Iterable = List.ToArray().ToList().Where(x => x.UserObject == ExceptThis);
            foreach (NetObject ThisNetObj in Iterable)
            {
                while (true)
                {
                    if (List.Contains(ThisNetObj)) List.Remove(ThisNetObj);
                    else break;
                }
            }
            return List;
        }
        public static List<NetObject> Except(this List<NetObject> List, Database.GroupDB.Group ExceptThis)
        {
            IEnumerable<NetObject> Iterable = List.ToArray().ToList().Where(x => x.UserObject.Groups.Select(y => y.Group).Contains(ExceptThis));
            foreach (NetObject ThisNetObj in Iterable)
            {
                while (true)
                {
                    if (List.Contains(ThisNetObj)) List.Remove(ThisNetObj);
                    else break;
                }
            }
            return List;
        }
        public static List<NetObject> Except(this List<NetObject> List, Database.GroupDB.Group.Rank ExceptThis)
        {
            IEnumerable<NetObject> Iterable = List.ToArray().ToList().Where(x => x.UserObject.Groups.Select(y => y.Rank).Contains(ExceptThis));
            foreach (NetObject ThisNetObj in Iterable)
            {
                while (true)
                {
                    if (List.Contains(ThisNetObj)) List.Remove(ThisNetObj);
                    else break;
                }
            }
            return List;
        }
        public static List<NetObject> Include(this List<NetObject> List, NetObject IncludeThis)
        {
            if (!(List.Contains(IncludeThis)))
            {
                List.Add(IncludeThis);
            }
            return List;
        }
        public static List<NetObject> Include(this List<NetObject> List, IEnumerable<NetObject> IncludeThis)
        {
            IEnumerable<NetObject> Iterable = Server.AllClients.ToArray().ToList();
            foreach (NetObject ThisNetObj in Iterable)
            {
                if (!(List.Contains(ThisNetObj)))
                {
                    List.Add(ThisNetObj);
                }
            }
            return List;
        }
        public static List<NetObject> Include(this List<NetObject> List, Database.UserDB.User IncludeThis)
        {
            IEnumerable<NetObject> Iterable = Server.AllClients.ToArray().ToList().Where(x => x.UserObject == IncludeThis);
            foreach (NetObject ThisNetObj in Iterable)
            {
                if (!(List.Contains(ThisNetObj)))
                {
                    List.Add(ThisNetObj);
                }
            }
            return List;
        }
        public static List<NetObject> Include(this List<NetObject> List, Database.GroupDB.Group IncludeThis)
        {
            IEnumerable<NetObject> Iterable = Server.AllClients.ToArray().ToList().Where(x => x.UserObject.Groups.Select(y => y.Group).Contains(IncludeThis));
            foreach (NetObject ThisNetObj in Iterable)
            {
                if (!(List.Contains(ThisNetObj)))
                {
                    List.Add(ThisNetObj);
                }
            }
            return List;
        }
        public static List<NetObject> Include(this List<NetObject> List, Database.GroupDB.Group.Rank IncludeThis)
        {
            IEnumerable<NetObject> Iterable = Server.AllClients.ToArray().ToList().Where(x => x.UserObject.Groups.Select(y => y.Rank).Contains(IncludeThis));
            foreach (NetObject ThisNetObj in Iterable)
            {
                if (!(List.Contains(ThisNetObj)))
                {
                    List.Add(ThisNetObj);
                }
            }
            return List;
        }
    }
}
