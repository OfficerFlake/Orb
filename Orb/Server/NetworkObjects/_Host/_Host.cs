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
                #region Variables
                public List<Network.Packet> PreviousPackets = new List<Network.Packet>();
                public NetObject Parent;
                public NetObject.Client ClientObject;
                public NetObject.Host HostObject;

                internal Socket HostSocket;
                internal Thread ReceiveThread;
                #endregion

                public void Create(NetObject _Parent)
                {
                    #region Prepare the object to do work.
                    Parent = _Parent;
                    if (Parent.UserObject.IsUtilityUser()) HostSocket = null;
                    else HostSocket = Server.CreateHostMainframe();

                    if ((!(Parent.UserObject.IsUtilityUser())) && HostSocket == null) {
                        //NOT a utility user, but the host service is closed.
                        Parent.ClientObject.SendMessage("There was an error connecting you to the server: The host server is offline.");
                        Parent.ClientObject.SendMessage("Please come back later! :D");
                        Parent.Close();
                        Logger.Console.WriteLine("&cCONNECTING USER WAS DISCONNECTED BECAUSE THE HOST SERVICE IS UNREACHABLE.");
                        return;
                    }

                    ReceiveThread = new Thread(new ThreadStart(ReceiveLoop));
                    //Do not start until the parent tells us to by calling ReceiveThread.Start();
                    #endregion
                }

                public void Start()
                {
                    #region Start the ReceiveLoop thread.
                    ReceiveThread.Start();
                    #endregion
                }

                public void ReceiveLoop()
                {
                    #region Receive Packets from the Client and Action Them.
                    while (HostSocket.Connected && Parent.ClientObject.ClientSocket.Connected)
                    {
                        //Console.WriteLine("NetObject.Client is waiting for data from its Client side socket.");
                        //Console.WriteLine("NetObject.Host Listening...");
                        //Thread.Sleep(1000);
                        Network.Packet InPacket = Network.Sockets.GetPacket(HostSocket);
                        //Console.WriteLine("NetObject.Host Got Some Data.");
                        if (InPacket.Type != 0)
                        {
                            PreviousPackets.Insert(0, InPacket);
                            while (PreviousPackets.Count > 5)
                            {
                                PreviousPackets.RemoveAt(PreviousPackets.Count - 1);
                            }
                            ProcessPacket(InPacket);
                        }
                        else
                        {
                            //The only time Socket Receive returns 0 is when a remote host disconnects, otherwise, it waits.
                            Parent.Close();
                        }
                    }
                    #endregion
                    #region The Client Socket has disconnected!
                    List<Network.Packet> PacketsCache = PreviousPackets;
                    foreach (Network.Packet ThisPacket in PacketsCache)
                    {
                        Logger.Log.Packet(ThisPacket);
                    }
                    //Console.WriteLine("Client Socket Closed inside NetObject.Host");
                    Parent.Close(); //Let the parent handle the client closing action.
                    #endregion
                }

                public void Send(Network.Packet InPacket)
                {
                    //Logger.Console.WriteLine(Parent.Username + " SEND");
                    if (Parent.UserObject.IsUtilityUser()) return;
                    #region Convert the packet to a byte array ready to send to the client socket.
                    byte[] Output = InPacket.Serialise();
                    #endregion
                    #region Discard dodgy packets.
                    if (InPacket.Size > 8192 || InPacket.Type < 0 || InPacket.Type > 60 || InPacket.Size - 4 != InPacket.Data.Length)
                    {
                        //Logger.Console.WriteLine(Parent.Username + " DODGY PACKET");
                        //Logger.Console.WriteLine(Parent.Username + " SIZE" + InPacket.Size.ToString());
                        //Logger.Console.WriteLine(Parent.Username + " TYPE" + InPacket.Type.ToString());
                        //Logger.Console.WriteLine(Parent.Username + " DATA" + InPacket.Data.CleanASCII());
                        //DODGY PACKET DO *NOT* SEND!
                        return;
                    }
                    #endregion
                    #region Send the byte array to the client socket.
                    //Logger.Console.WriteLine(Parent.Username + " SEND FINAL");
                    try
                    {
                        if (HostSocket.Connected)
                        {
                            HostSocket.Send(Output);
                        }
                    }
                    #endregion
                    #region Socket closed as we tried to send data?
                    catch (Exception e)
                    {
                        //Socket Closed?
                        //Logger.Console.WriteLine(Parent.Username + " FAIL");
                        List<Network.Packet> PacketsCache = PreviousPackets;
                        foreach (Network.Packet ThisPacket in PacketsCache)
                        {
                            Logger.Log.Packet(ThisPacket);
                        }
                        Logger.Log.Bug(e, "Could Not Send A Packet... Socket Closed?");
                    }
                    //Logger.Console.WriteLine(Parent.Username + " DONE");
                    #endregion
                }

                public void SendMessage(String Message)
                {
                    Network.Packets.Type32_ChatMessage MessagePacket = new Network.Packets.Type32_ChatMessage();
                    MessagePacket.Message = ColorHandling.StripColors(Message);
                    Send(MessagePacket.Serialise());
                }
            }
        }
    }
}