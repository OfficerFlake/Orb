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
        public static partial class Password
        {
            public static bool Authenticate(NetObject NetObj)
            {
                if (!NetObj.UserObject.UsePassword)
                {
                    //NetObj.ClientObject.SendMessage("Password Clear.");
                    return true;
                }
                else
                {
                    if (!(_AuthenticateOnly(NetObj)))
                    {
                        return false;
                    }
                    else return true;
                }
            }

            public static bool _AuthenticateOnly(NetObject NetObj)
            {
                #region VerifyPass
                if (NetObj.UserObject.IsUtilityUser())
                {
                    //Password Auth not applicable to non-clients.
                    return true;
                }
                NetObj.ClientObject.SendMessage("Password Authentication Mode Initialised.");
                NetObj.ClientObject.SendMessage("Please Enter Your Password: ");
                while (true)
                {
                    AutoResetEvent WaitForMessage = new AutoResetEvent(false);
                    NetObj.TextWaiters.Add(WaitForMessage);
                    NetObj.ClientObject.GetChatPacketBeforeLoginStarter();
                    if (!WaitForMessage.WaitOne(30000))
                    {
                        NetObj.ClientObject.SendMessage("Still waiting for a response for the Password Authentication Module.");
                        NetObj.ClientObject.SendMessage("Please Enter a Password, or you will be disconnected in 30 Seconds.");
                        NetObj.TextWaiters.Add(WaitForMessage);
                        NetObj.ClientObject.GetChatPacketBeforeLoginStarter();
                        if (!WaitForMessage.WaitOne(30000))
                        {
                            NetObj.ClientObject.SendMessage("No Response for the Password Authentication Module. You have been disconnected.");
                            NetObj.TextWaiters.RemoveAll(x => x == WaitForMessage);
                            return false;
                        }
                    }
                    string ThisMessage = NetObj.TextInput;
                    if (ThisMessage.ToUpper() == "/EXIT")
                    {
                        NetObj.ClientObject.SendMessage("Leaving the Password Authentication Mode...");
                        return false;
                    }
                    if (SaltGenerator.Encrypt(ThisMessage) == NetObj.UserObject.Password || ThisMessage == NetObj.UserObject.Password && NetObj.UserObject.Password == "")
                    {
                        NetObj.ClientObject.SendMessage("Password Correct, You are now logged in.");
                        NetObj.ClientObject.SendMessage("\n\n\n\n\n\n\n\n\n\n");
                        return true;
                    }
                    else
                    {
                        NetObj.ClientObject.ComplexTaskWaiter();
                        Thread.Sleep(1000); //This is to stop brute force bots from breaking or lagging the server out!
                        NetObj.ClientObject.ComplexTaskComplete.Set();
                        NetObj.ClientObject.SendMessage("Password incorrect, Please Try Again.");
                    }
                }
                #endregion
            }
        }
    }
}