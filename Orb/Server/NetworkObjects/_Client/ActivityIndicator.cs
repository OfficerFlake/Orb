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
                public AutoResetEvent ComplexTaskComplete = new AutoResetEvent(false);

                public void ComplexTaskWaiterThread()
                {
                    while (true)
                    {
                        SendMessage("...");
                        if (ComplexTaskComplete.WaitOne(5000))
                        {
                            break;
                        }
                    }
                }

                public void ComplexTaskWaiter()
                {
                    Thread TaskWaiter = new Thread(new ThreadStart(ComplexTaskWaiterThread));
                    TaskWaiter.Start();
                }
            }
        }
    }
}