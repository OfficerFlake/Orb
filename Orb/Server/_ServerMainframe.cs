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
        public static partial class ConnectionMainframe {
            #region Variables
            static Socket ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            #endregion

            public static void Start()
            {

                Orb.Version.TestVersionWarningAlpha();
                //Console.WriteLine("Orb.Server.ConnectionMainFrame.Start():Prepare");
                #region Prepare Proxy Socket
                if (Database.Settings.Get(Database.Settings.Strings.LocalTestMode).Equals(true))
                {
                    Logger.Console.WriteLine("WARNING: PROXY SERVICE IS IN 'LOCALTESTMODE'");
                    Logger.Log.SystemMessage("WARNING: PROXY SERVICE IS IN 'LOCALTESTMODE'");
                    //ServerAddress = new IPEndPoint(IPAddress.Any, 7915);
                    ServerAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Database.Settings.OrbPort);
                }
                else
                {
                    //ServerAddress = new IPEndPoint(IPAddress.Any, 7915);
                    ServerAddress = new IPEndPoint(IPAddress.Any, Database.Settings.OrbPort);
                }
                #endregion
                //Console.WriteLine("Orb.Server.ConnectionMainFrame.Start():Bind");
                #region Bind Proxy Socket To Port
                try
                {
                    ListenerSocket.Bind(ServerAddress);
                }
                catch (Exception e)
                {
                    Logger.Log.Bug(e, String.Format("Orb Server could not start on {0}:{1}. Something else may be using the port.", ServerAddress.Address.ToString(), ServerAddress.Port.ToString()));
                    Server.Shutdown.MasterClose("CRITICAL ERROR: FAILED TO START THE SERVER.\n(PORT &f" + Database.Settings.OrbPort.ToString() + "&c IS IN USE BY ANOTHER PROCESS!)", 30);
                }
                //Logger.Console.WriteLine("Managed To Bind.");
                #endregion
                //Console.WriteLine("Orb.Server.ConnectionMainFrame.Start():Listen...");
                #region Accept and Hand Off Clients
                ListenerSocket.Listen(1);
                Logger.Console.WriteLine("&3Listening For Clients...");
                ServerGUI.LockInput(false);
                Logger.Console.LockInput(false);
                //Logger.Console.WriteLine(DateTime.Now.ToString());
                //Pass ServerSocket to ClientReceiver as new thread.
                //ClientReceiver:
                while (true)
                {
                    Socket ConnectingClientSocket;
                    try
                    {
                        ConnectingClientSocket = ListenerSocket.Accept();

                        //Logger.Console.WriteLine("Got a New Client from " + (ConnectingClientSocket.RemoteEndPoint as IPEndPoint).Address.ToString());

                        NetObject ConnectingObject = new NetObject();
                        ConnectingObject.Create(ConnectingClientSocket);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Could Not Add A Client OR Socket Closed.");
                        //Socket has been closed, we should break now.
                        break;
                    }
                }
                //Pass ClientSocket to new client thread.
                #endregion
            }

            public static void Stop()
            {
                ListenerSocket.Close();
                Logger.Console.WriteLine("Managed To Stop Listening...");
                Logger.Console.WriteLine("Managed To Unbind...");
            }
        }

        public partial class ProxyMainframe
        {
            #region Variables
            internal Socket HostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //int HostPort = Database.Settings.OrbPort;
            //IPAddress HostIP = Database.Settings.OrbIP;

            int HostPort = 7915;
            //IPAddress HostIP = Dns.GetHostAddresses("42south.dyndns.org")[0];


            internal IPEndPoint HostAddress = new IPEndPoint(Database.Settings.ServerIP, Database.Settings.ServerPort);
            //internal IPEndPoint HostAddress = new IPEndPoint(Dns.GetHostAddresses("42south.dyndns.org")[0], 7915);
            //internal IPEndPoint HostAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7914);
            #endregion
        }

        public partial class ServerMainframe {
            internal Socket HostSocket;
        }

        public static Socket CreateHostMainframe() {

            if (Database.Settings.ProxyMode)
            {
                ProxyMainframe Mainframe = new ProxyMainframe();
                try
                {
                    Mainframe.HostSocket.Connect(Mainframe.HostAddress);
                }
                catch
                {
                    return null;
                }
                return Mainframe.HostSocket;
            }
            else
            {
                ServerMainframe Mainframe = new ServerMainframe();
                return Mainframe.HostSocket;
            }
        }

        #region Some Error Throwing Function, Not needed anyway...
        //static object UsingServerMainFrame = Server.CreateHostMainframe();
        #endregion
    }
}
