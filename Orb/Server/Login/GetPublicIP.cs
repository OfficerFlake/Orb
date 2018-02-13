using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Server
    {
        public static partial class GetPublicIP
        {
            public static IPAddress GetPublicIpAddress()
            {
                if (_returnedIP != IPAddress.None) return _returnedIP;
                while (_GetThread.IsAlive)
                    {
                        if (_ThreadComplete.WaitOne(5000))
                        {
                            return _returnedIP;
                        }
                    }
                _GetThread = new Thread(new ThreadStart(_GetPublicIpAddress));
                _GetThread.Start();
                for (int n = 0; n < 10; n++)
                {
                    bool result = _ThreadComplete.WaitOne(5000);
                    if (result)
                    {
                        return _returnedIP;
                    }
                    else {
                        _GetThread.Abort();
                        _GetThread = new Thread(new ThreadStart(_GetPublicIpAddress));
                        _GetThread.Start();
                    }
                }
                return IPAddress.None;
            }

            private static IPAddress _returnedIP = IPAddress.None;
            private static ManualResetEvent _ThreadComplete = new ManualResetEvent(false);
            private static Thread _GetThread = new Thread(new ThreadStart(_GetPublicIpAddress));

            private static void _GetPublicIpAddress()
            {
                var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

                request.UserAgent = "curl"; // this simulate curl linux command

                string publicIPAddress = "";

                //request.Timeout = 1000;

                request.Method = "GET";

                WebResponse response = null;
                IPAddress output = IPAddress.None;
                try
                {
                    response = request.GetResponse();
                    using (response)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            publicIPAddress = reader.ReadToEnd();
                        }
                    }
                    publicIPAddress = publicIPAddress.Replace("\n", "");
                }
                catch
                {
                    _returnedIP = output;
                    return;
                }
                try
                {
                    output = IPAddress.Parse(publicIPAddress);
                }
                catch
                {
                }
                _returnedIP = output;
                _ThreadComplete.Set();
                return;
            }
        }
    }
}
