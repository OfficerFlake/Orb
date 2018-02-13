using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Orb
{
    public static partial class Network
    {
        public partial class Sockets
        {
            public static Network.Packet GetPacket(Socket MySocket)
            {
                #region Receiver
                //Console.WriteLine("READY.");
                uint DataLength = 0;
                uint DataType = 0;
                int error = 0;
                byte[] DataBody;
                int ReceivedData = 0;
                byte[] InputBuffer;
                int InputReceived = 0;
                byte[] SizeBuffer = new byte[4];
                byte[] TypeBuffer = new byte[4];
                byte[] BodyBuffer = new byte[4];
                try
                {

                    #region Get The Size
                    ReceivedData = 0;
                    InputReceived = 0;
                    InputBuffer = new byte[4];
                    while (ReceivedData < 4 && MySocket.Connected)
                    {

                        InputReceived = MySocket.Receive(InputBuffer, (4 - ReceivedData), 0);
                        InputBuffer = InputBuffer.Take(InputReceived).ToArray();
                        System.Buffer.BlockCopy(InputBuffer, 0, SizeBuffer, ReceivedData, InputReceived);
                        ReceivedData += InputReceived;
                        InputBuffer = new byte[4 - ReceivedData];
                        if (ReceivedData == 0)
                        {
                            //Console.Write("The Socket Poll Returned an Empty Packet... Perhaps It's trying to close?", ReceivedData, Buffer.Length);
                            //Console.WriteLine("\rSize\tReceived: {0}\t Expected {1} (No Data Received.)", ReceivedData, Buffer.Length);
                            return Network.Packets.New();
                        }
                        if (ReceivedData < 4)
                        {
                            Console.Write(String.Format("\rSize\tReceived: {0}\t Expected {1}\t (Not Enough Data Received.)", ReceivedData, SizeBuffer.Length));
                            error++;
                        }
                    }
                    if (!(MySocket.Connected)) { return Network.Packets.New(); }
                    if (error > 0) Console.WriteLine(String.Format("\rSize\tReceived: {0}\t Expected {1}\t", ReceivedData, SizeBuffer.Length));
                    DataLength = BitConverter.ToUInt32(SizeBuffer, 0);
                    //Console.WriteLine("Size: " + DataLength);
                    //Prevent a datacrash on no packet received...
                    //Console.WriteLine("\rSize\tReceived: {0}\t Expected {1}                            ", ReceivedData, Buffer.Length);
                    if (DataLength == 0) return Network.Packets.New();
                    if (DataLength > 8192) return Network.Packets.New();
                    #endregion
                    #region Get The Type
                    ReceivedData = 0;
                    InputReceived = 0;
                    InputBuffer = new byte[4];
                    while (ReceivedData < 4 && MySocket.Connected)
                    {
                        InputReceived = MySocket.Receive(InputBuffer, (4 - ReceivedData), 0);
                        InputBuffer = InputBuffer.Take(InputReceived).ToArray();
                        System.Buffer.BlockCopy(InputBuffer, 0, TypeBuffer, ReceivedData, InputReceived);
                        ReceivedData += InputReceived;
                        InputBuffer = new byte[4 - ReceivedData];
                        if (ReceivedData == 0)
                        {
                            //Console.Write("The Socket Poll Returned an Empty Packet... Perhaps It's trying to close?", ReceivedData, Buffer.Length);
                            //Console.WriteLine("\rSize\tReceived: {0}\t Expected {1} (No Data Received.)", ReceivedData, Buffer.Length);
                            return Network.Packets.New();
                        }
                        if (ReceivedData < 4)
                        {
                            Console.Write(String.Format("\rType\tReceived: {0}\t Expected {1}\t (Not Enough Data Received.)", ReceivedData, TypeBuffer.Length));
                            error++;
                        }
                    }
                    if (!(MySocket.Connected)) { return Network.Packets.New(); }
                    if (error > 0) Console.WriteLine(String.Format("\rType\tReceived: {0}\t Expected {1}\t", ReceivedData, TypeBuffer.Length));
                    DataType = BitConverter.ToUInt32(TypeBuffer, 0);
                    //Console.WriteLine("\rType\tReceived: {0}\t Expected {1}                               ", ReceivedData, Buffer.Length);
                    #endregion
                    #region Get The Body
                    BodyBuffer = new byte[DataLength - 4];
                    ReceivedData = 0;
                    InputReceived = 0;
                    InputBuffer = new byte[DataLength - 4];
                    while (ReceivedData < (DataLength - 4) && MySocket.Connected)
                    {
                        try
                        {
                            InputReceived = MySocket.Receive(InputBuffer, ((int)(DataLength - 4) - ReceivedData), 0);
                        }
                        catch
                        {
                            //Console.WriteLine("BUFFER: {0}", InputBuffer.Length);
                            //Console.WriteLine("ASKED: {0}", ((int)(DataLength - 4) - ReceivedData));
                        }
                        InputBuffer = InputBuffer.Take(InputReceived).ToArray();
                        System.Buffer.BlockCopy(InputBuffer, 0, BodyBuffer, ReceivedData, InputReceived);
                        InputBuffer = new byte[DataLength - 4 - ReceivedData];
                        ReceivedData += InputReceived;
                        if (ReceivedData == 0)
                        {
                            //Console.Write("The Socket Poll Returned an Empty Packet... Perhaps It's trying to close?", ReceivedData, Buffer.Length);
                            //Console.WriteLine("\rSize\tReceived: {0}\t Expected {1} (No Data Received.)", ReceivedData, Buffer.Length);
                            return Network.Packets.New();
                        }
                        if (ReceivedData < (DataLength - 4))
                        {
                            Console.Write(String.Format("\rData\tReceived: {0}\t Expected {1}\t (Not Enough Data Received.)", ReceivedData, BodyBuffer.Length));
                            error++;
                        }
                    }
                    if (!(MySocket.Connected)) { return Network.Packets.New(); }
                    if (error > 0) Console.WriteLine(String.Format("\rData\tReceived: {0}\t Expected {1}\t", ReceivedData, BodyBuffer.Length));
                    DataBody = BodyBuffer;
                    //Console.WriteLine("\rData\tReceived: {0}\t Expected {1}                                   ", ReceivedData, Buffer.Length);
                    #endregion
                    if (error > 0) Console.WriteLine(String.Format("Size: {0}, Type: {1}, DataLength: {2}", DataLength, DataType, DataBody.Length));
                    #region Make and return a new packet
                    Network.Packet outpacket = Network.Packets.New();

                    outpacket.Type = DataType;
                    foreach (byte thisbyte in DataBody)
                    {
                        outpacket.Data += (char)thisbyte;
                    }
                    //Console.WriteLine("Data: " + BitConverter.ToString(DataBody));
                    //Console.WriteLine("Data.Length: " + outpacket.Data.Length);
                    //Console.WriteLine("Size: " + DataLength.ToString());
                    //Console.WriteLine("Type: " + outpacket.Type.ToString());
                    //Console.WriteLine("NEW Size: " + outpacket.Size.ToString());

                    //Validate Packet Size.
                    if (outpacket.Size != DataLength)
                    {
                        Console.WriteLine(String.Format("The Packet Size is incorrect! Received:{0}/Calculated:{1}", DataLength, outpacket.Size));
                        Logger.Log.Packet(outpacket, "The Packet Size is incorrect!");
                        return Network.Packets.New();
                    }
                    if (outpacket.Type != DataType)
                    {
                        Console.WriteLine(String.Format("The Packet Type is incorrect! Received:{0}/Calculated:{1}", DataType, outpacket.Type));
                        Logger.Log.Packet(outpacket, "The Packet Type is incorrect!");
                        return Network.Packets.New();
                    }
                    if (outpacket.Type <= 0 || outpacket.Type >= 60)
                    {
                        Console.WriteLine(String.Format("The Packet Type is out of range! Received:{0}/Calculated:{1}", DataType, outpacket.Type));
                        Logger.Log.Packet(outpacket, "The Packet Type is out of range!");
                        return Network.Packets.New();
                    }
                    else if (outpacket.Data.Length != DataBody.Length)
                    {
                        Console.WriteLine(String.Format("The Packet Data is incorrect! Received:{0}/Calculated:{1}", DataType, outpacket.Type));
                        Logger.Log.Packet(outpacket, "The Packet Body is incorrect!");
                        return Network.Packets.New();
                    }
                    else
                    {
                        return outpacket;
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    //Socket Closed.
                    //Logger.Log.Bug(e, "Socket Closed.");
                    //Logger.Console.WriteLine("Socket Closed.");
                    //MySocket.Shutdown(SocketShutdown.Send);
                    //MySocket.Close();
                    return Network.Packets.New();
                }
                #endregion
            }
        }
    }
}
