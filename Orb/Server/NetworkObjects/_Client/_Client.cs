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
                #region Variables
                public List<DateTime> SleepTimers = new List<DateTime>();
                public List<Network.Packet> PreviousPackets = new List<Network.Packet>();
                public NetObject Parent;
                public NetObject.Client ClientObject;
                public NetObject.Host HostObject;

                internal Socket ClientSocket;
                internal Thread ReceiveThread;

                public IPAddress PublicIP
                {
                    get
                    {
                        if ((ClientSocket.RemoteEndPoint as IPEndPoint).Address.ToString() == "127.0.0.1")
                        {
                            return Server.GetPublicIP.GetPublicIpAddress();
                        }
                        else
                        {
                            return (ClientSocket.RemoteEndPoint as IPEndPoint).Address;
                        }
                    }
                }
                    
                #endregion

                public void Create(Socket _clientsock, NetObject _Parent)
                {
                    #region Prepare the object to do work.
                    Parent = _Parent;
                    ClientSocket = _clientsock;

                    ReceiveThread = new Thread(new ThreadStart(ReceiveLoop));
                    //Do not start until the parent tells us to by calling ReceiveThread.Start();
                    #endregion
                }

                public void Start()
                {
                    #region Start the ReceiveLoop thread.
                    ReceiveThread.Start();
                    //PollThread.Start();
                    #endregion
                }

                public void ReceiveLoop()
                {
                    #region Receive Packets from the Client and Action Them.
                    //Console.WriteLine("NetObject.Client Ready");
                    while (ClientSocket.Connected && Parent.HostObject.HostSocket.Connected)
                    {
                        //Console.WriteLine("NetObject.Client is waiting for data from its Client side socket.");
                        //Console.WriteLine("NetObject.Client Listening...");
                        Network.Packet InPacket = Network.Sockets.GetPacket(ClientSocket);
                        //Console.WriteLine("NetObject.Client Got Some Data.");
                        if (InPacket.Type != 0)
                        {
                            PreviousPackets.Insert(0, InPacket);
                            while (PreviousPackets.Count > 5)
                            {
                                PreviousPackets.RemoveAt(PreviousPackets.Count - 1);
                            }
                            if (!_UnSleep()) continue;
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
                    //Console.WriteLine("Client Socket Closed inside NetObject.Client");
                    Parent.Close(); //Let the parent handle the client closing action.
                    //Console.WriteLine("CLOSE5");
                    #endregion
                }

                public void GetChatPacketBeforeLoginStarter()
                {
                    Thread Get32Thread = new Thread(new ThreadStart(_GetChatPacketBeforeLogin));
                    Get32Thread.Start();
                }

                /// <summary>
                /// Receive a Chat Packet from the Client and set it as the TextInput for TextInputWaiters.
                /// </summary>
                public void _GetChatPacketBeforeLogin()
                {
                    #region Receive a Chat Packet from the Client and set it as the TextInput for TextInputWaiters.
                    //Console.WriteLine("NetObject.Client Ready");
                    while (ClientSocket.Connected)
                    {
                        //Console.WriteLine("NetObject.Client is waiting for data from its Client side socket.");
                        //Console.WriteLine("NetObject.Client Listening...");
                        Logger.Console.WriteLine("Waiting");
                        Network.Packet InPacket = Network.Sockets.GetPacket(ClientSocket);
                        Logger.Console.WriteLine("Received");
                        if (InPacket.Type == 32)
                        {
                            Network.Packets.Type32_ChatMessage ThisPacket = new Network.Packets.Type32_ChatMessage(InPacket);
                            string EditMessage = ThisPacket.Message.Remove(0, 1);
                            EditMessage = EditMessage.Remove(0, Parent.Username.Length);
                            EditMessage = EditMessage.Remove(0, 1);
                            //Console.WriteLine(EditMessage);
                            Parent.TextInput = EditMessage;
                            Logger.Console.WriteLine(Parent.TextInput);
                            foreach (AutoResetEvent ThisEvent in Parent.TextWaiters.ToArray())
                            {
                                ThisEvent.Set();
                                Parent.TextWaiters.Remove(ThisEvent);
                            }
                            return;
                        }
                        //Console.WriteLine("NetObject.Client Got Some Data.");
                        if (InPacket.Type != 0)
                        {
                            PreviousPackets.Insert(0, InPacket);
                            while (PreviousPackets.Count > 5)
                            {
                                PreviousPackets.RemoveAt(PreviousPackets.Count - 1);
                            }
                            Logger.Console.WriteLine("!=32");
                            //ProcessPacket(InPacket);
                            //DONT DO THAT! WE DONT WANT A LOG IN TO HAPPEN ETC.
                        }
                        else
                        {
                            Logger.Console.WriteLine("CLOSE");
                            //The only time Socket Receive returns 0 is when a remote host disconnects, otherwise, it waits.
                            Parent.Close();
                        }

                    }
                    Logger.Console.WriteLine("CLOSE2");
                    #endregion
                }

                #region Decrepted (GetOnePacket)
                /*public Network.Packet GetOnePacket()
                {
                    if (Parent.UserObject.IsUtilityUser())
                    {
                        return new Network.Packet();
                    }
                    #region Receive Packet from the Client, and return..
                    //Console.WriteLine("NetObject.Client Ready");
                    Network.Packet InPacket = new Network.Packet();
                    if (ClientSocket.Connected && Parent.HostObject.HostSocket.Connected)
                    {
                        //Console.WriteLine("NetObject.Client is waiting for data from its Client side socket.");
                        //Console.WriteLine("NetObject.Client Listening...");
                        InPacket = Network.Sockets.GetPacket(ClientSocket);
                        //Console.WriteLine("NetObject.Client Got Some Data.");
                        if (InPacket.Type != 0)
                        {
                            PreviousPackets.Insert(0, InPacket);
                            while (PreviousPackets.Count > 5)
                            {
                                PreviousPackets.RemoveAt(PreviousPackets.Count - 1);
                            }
                        }
                        else
                        {
                            if (!(ClientSocket.Connected || Parent.HostObject.HostSocket.Connected))
                            {
                                Parent.Close();
                            }
                        }
                    }
                    return InPacket;
                    #endregion
                    #region The Client Socket has disconnected!
                    List<Network.Packet> PacketsCache = PreviousPackets;
                    foreach (Network.Packet ThisPacket in PacketsCache)
                    {
                        Logger.Log.Packet(ThisPacket);
                    }
                    //Console.WriteLine("Client Socket Closed inside NetObject.Client");
                    Parent.Close(); //Let the parent handle the client closing action.
                    #endregion
                }*/
                #endregion

                public void Send(Network.Packet InPacket)
                {
                    if (!_UnSleep()) return;
                    if (Parent.UserObject.IsUtilityUser()) return;
                    #region Convert the packet to a byte array ready to send to the client socket.
                    byte[] Output = InPacket.Serialise();
                    #endregion
                    #region Discard dodgy packets.
                    if (InPacket.Size > 8192 || InPacket.Type < 0 || InPacket.Type > 60 || InPacket.Size - 4 != InPacket.Data.Length)
                    {
                        //DODGY PACKET DO *NOT* SEND!
                        return;
                    }
                    #endregion
                    #region Send the byte array to the client socket.
                    try
                    {
                        if (ClientSocket != null)
                        {
                            try
                            {
                                ClientSocket.Send(Output);
                            }
                            catch
                            {
                            }
                        }
                    }
                    #endregion
                    #region Socket closed as we tried to send data?
                    catch (Exception e)
                    {
                        //Socket Closed?
                        List<Network.Packet> PacketsCache = PreviousPackets;
                        foreach (Network.Packet ThisPacket in PacketsCache)
                        {
                            Logger.Log.Packet(ThisPacket);
                        }
                        Logger.Log.Bug(e, "Could Not Send A Packet... Socket Closed?");
                    }
                    #endregion
                }

                public void SendMessage(String Message)
                {
                    if (Parent == Server.OrbConsole)
                    {
                        Logger.Console.WriteLine(Message);
                        return;
                    }
                    Network.Packets.Type32_ChatMessage MessagePacket = new Network.Packets.Type32_ChatMessage();
                    MessagePacket.Message = ColorHandling.StripColors(Message);
                    Send(MessagePacket.Serialise());
                }

                public void Sleep(double MS)
                {
                    SleepTimers.Add(DateTime.Now + TimeSpan.FromMilliseconds(MS));
                }

                private bool _UnSleep()
                {
                    foreach (DateTime ThisDT in SleepTimers.ToArray())
                    {
                        if (DateTime.Now < ThisDT) return false;
                        else SleepTimers.Remove(ThisDT);
                    }
                    return true;
                }
            }
        }
    }
}