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
        public static partial class YSFHQ
        {
            public static partial class GetUserID {
                public static int From15CharString(string Input) {
                    WebClient Query = new WebClient();
                    string Content = "";
                    try
                    {
                        Content = Query.DownloadString("http://forum.ysfhq.com/orb/ysf_username_query.php?name=" + Input);
                        //Content = Query.DownloadString("http://forum.ysfhq.com/orb/ysf_username_query.php?name=Stingx-russiank");
                    }
                    catch
                    {
                        return -1;
                    }
                    string[] MultipleEntries = Content.Split('\n');
                    if (Content.ToUpperInvariant().StartsWith("NOT_FOUND"))
                    {
                        return -2;
                    }
                    if (Content.ToUpperInvariant().StartsWith("NAME_MISSING"))
                    {
                        return -3;
                    }
                    foreach (string ThisLine in MultipleEntries)
                    {
                        try
                        {
                            string[] Iterable = ThisLine.Split(new string[] { " " }, 3, StringSplitOptions.None);
                            IPAddress ThisIP = IPAddress.Parse(Iterable[0]);
                            int ThisID = Int32.Parse(Iterable[1]);
                            string ThisUser = Iterable[2];

                            if (ThisUser.ToUpper().StartsWith(Input.ToUpper()))
                            {
                                return ThisID;
                            }
                        }
                        catch
                        {
                            return -3;
                        }
                    }
                    return -2;
                }
            }

            public static partial class GetIPAddress
            {
                public static IPAddress FromID(int ID)
                {
                    WebClient Query = new WebClient();
                    string UserName = Query.DownloadString("http://forum.ysfhq.com/orb/hq_username_query.php?id=" + ID.ToString());
                    if (UserName == "NOT_FOUND") return IPAddress.None;
                    string Content = Query.DownloadString("http://forum.ysfhq.com/orb/ysf_username_query.php?name=" + UserName);
                    string[] MultipleEntries = Content.Split('\n');
                    foreach (string ThisLine in MultipleEntries)
                    {
                        try
                        {
                            string[] Iterable = ThisLine.Split(new string[] { " " }, 3, StringSplitOptions.None);
                            IPAddress ThisIP = IPAddress.Parse(Iterable[0]);
                            //Logger.Console.WriteLine(ThisIP.ToString());
                            //Logger.Console.WriteLine(Iterable[0]);
                            int ThisID = Int32.Parse(Iterable[1]);
                            string ThisUser = Iterable[2];

                            if (UserName.ToUpper().StartsWith(ThisUser.ToUpper()) && MultipleEntries.Count() == 1)
                            {
                                Logger.Log.SystemMessage("Matched User" + ThisUser);
                                if (ThisIP.ToString().Split('.').Count() != 4)
                                {
                                    Logger.Log.SystemMessage("IP For User" + ThisUser + "is IPv6... Denied.");
                                    return IPAddress.None;
                                }
                                return ThisIP;
                            }
                            else
                            {
                                Logger.Log.SystemMessage("No Matches for user" + ThisUser);
                                return IPAddress.None;
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Log.Bug(e, "YSFHQ IP COMPARISON");
                            return IPAddress.None;
                        }
                    }
                    return IPAddress.None;
                }
            }

            public static partial class GetFullUsername
            {
                public static string FromShortName(String Input)
                {
                    int ID = GetUserID.From15CharString(Input);
                    WebClient Query = new WebClient();
                    string Content = Query.DownloadString("http://forum.ysfhq.com/orb/hq_username_query.php?id=" + ID);
                    if (Content == "NOT_FOUND" || Content == "ID_MISSING") return Input;
                    return Content;
                }
            }

            public static bool Authenticate(NetObject ThisClient)
            {
                ThisClient.ClientObject.SendMessage("This Server uses YSFlight Headquarters Authentication.");
                ThisClient.ClientObject.SendMessage("Now Checking your details against the YSFlight Headquarters Records, please be patient.");
                ThisClient.ClientObject.ComplexTaskWaiter();
                //Logger.Console.WriteLine("&cUSERNAME: " + ThisClient.Username);
                int ID = GetUserID.From15CharString(ThisClient.Username);
                //Logger.Console.WriteLine("&cID: " + ID.ToString());
                if (ID == -3)
                {
                    ThisClient.ClientObject.SendMessage("There was a bug connecting you to YSFlight Headquarters.");
                    ThisClient.ClientObject.SendMessage("A bug report has been made for the Server Owner to investigate.");
                    ThisClient.ClientObject.SendMessage("Sorry!");
                    Logger.Console.WriteLine("Client " + ThisClient.Username + " disconnected. (YSFHQ Authentication Bug).");
                    Logger.Log.Bug("Unable to get YSFHQ Information for User: + \"" + ThisClient.Username + "\".");
                    return false;
                }
                if (ID == -2)
                {
                    ThisClient.ClientObject.SendMessage("Your supplied username has not matched any records on YSFlight Headquarters.");
                    ThisClient.ClientObject.SendMessage("Please use your YSFlight Headquarters username when logging into this server. (No squadtags etc.)");
                    Logger.Console.WriteLine("Client " + ThisClient.Username + " disconnected. (YSFHQ Username Mismatch).");
                    Logger.Log.SystemMessage("No Matching YSFHQ Information for User: + \"" + ThisClient.Username + "\".");
                    return false;
                }
                if (ID < 0)
                {
                    ThisClient.ClientObject.SendMessage("There was an error authenticating you. (Service unreachable?)");
                    ThisClient.ClientObject.SendMessage("Please try again later, sorry for the inconvenience!");
                    Logger.Console.WriteLine("Client " + ThisClient.Username + " disconnected. (YSFHQ Authentication Unreachable).");
                    Logger.Log.SystemMessage("Not able to connect to YSFHQ for User: + \"" + ThisClient.Username + "\".");
                    return false;
                }
                //Logger.Console.WriteLine("&cIP1: " + GetIPAddress.FromID(ID).ToString());
                //Logger.Console.WriteLine("&e" + "#" + GetIPAddress.FromID(ID).ToString() + "#");
                //Logger.Console.WriteLine("&e" + "$" + ThisClient.ClientObject.PublicIP.ToString() + "$");
                if ((ThisClient.ClientObject.ClientSocket.RemoteEndPoint as IPEndPoint).Address.ToString() == "127.0.0.1" && Database.Settings.AlwaysAllowLocalHost)
                {
                    ThisClient.ClientObject.ComplexTaskComplete.Set();
                    ThisClient.ClientObject.SendMessage("You are automatically logged in, as you are connecting from LocalHost.");
                    return true;
                }
                else if ((ThisClient.ClientObject.ClientSocket.RemoteEndPoint as IPEndPoint).Address.ToString() == "127.0.0.1" && !Database.Settings.AlwaysAllowLocalHost)
                {
                    ThisClient.ClientObject.ComplexTaskComplete.Set();
                    ThisClient.ClientObject.SendMessage("As you are connecting from LocalHost, I need to check with a polling service to check your IP address.");
                    ThisClient.ClientObject.SendMessage("This takes longer then the standard log in, please be patient.");
                    ThisClient.ClientObject.ComplexTaskWaiter();
                    if (ThisClient.ClientObject.PublicIP == IPAddress.None)
                    {
                        ThisClient.ClientObject.ComplexTaskComplete.Set();
                        ThisClient.ClientObject.SendMessage("The polling service failed to retreive your IP Address.");
                        ThisClient.ClientObject.SendMessage("Please try again later.");
                        Logger.Console.WriteLine("Client " + ThisClient.Username + " disconnected. (YSFHQ IP Confirmation Failure).");
                        ThisClient.Close();
                        return false;
                    }
                }
                if (GetIPAddress.FromID(ID).ToString() == ThisClient.ClientObject.PublicIP.ToString())
                {
                    ThisClient.ClientObject.ComplexTaskComplete.Set();
                    ThisClient.ClientObject.SendMessage("Thanks for your patience, you are now logged in.");
                    if (!ThisClient.UserObject.UsePassword) ThisClient.ClientObject.SendMessage("You do not have a password fallback for authentication set! Use \"/CHANGEPASSWORD\" to set one!");
                    ThisClient.ClientObject.SendMessage("\n\n\n\n\n\n\n\n\n\n");
                    Logger.Log.SystemMessage("YSFHQ Authentication successful for User: + \"" + ThisClient.Username + "\".");
                    //Logger.Console.WriteLine("&cIP: " + ThisClient.ClientObject.PublicIP.ToString());
                    return true;
                }
                else
                {
                    //Logger.Console.WriteLine("&cIP: " + ThisClient.ClientObject.PublicIP.ToString());
                    ThisClient.ClientObject.ComplexTaskComplete.Set();
                    ThisClient.ClientObject.SendMessage("Your records do not appear to match what is on record with YSFlight Headquarters.");
                    try
                    {
                        ThisClient.ClientObject.SendMessage("(You are connecting to Orb from " + ThisClient.ClientObject.PublicIP.ToString() + " but your YSFHQ IP Address is " + GetIPAddress.FromID(ID).Mask() + ").");
                    }
                    catch
                    {
                    }
                    if (ThisClient.UserObject.UsePassword)
                    {
                        ThisClient.ClientObject.SendMessage("As you have a password on your account, You will now be redirected to the Password Authentication Module.");
                        ThisClient.ClientObject.SendMessage("Alternatively, please log out/in to YSFHQ if necessary to refresh your IP Address!");
                        if (Server.Password._AuthenticateOnly(ThisClient))
                        {
                            Logger.Log.SystemMessage("Password Fallback Authentication successful for User: + \"" + ThisClient.Username + "\".");
                            return true;
                        }
                        else return false;
                    }
                    if (ThisClient.UserObject.LoginCount < 10)
                    {
                        ThisClient.ClientObject.ComplexTaskComplete.Set();
                        ThisClient.ClientObject.SendMessage("As you have a low login count, and you have a matching HQ username, you have been allowed into the server.");
                        ThisClient.ClientObject.SendMessage("Please set a fallback password using \"/CHANGEPASSWORD\" so you can login in future if you do not match the YSFHQ records!");
                        return true;
                    }
                    ThisClient.ClientObject.SendMessage("Please log out/in to YSFHQ if necessary to refresh your IP Address!");
                    Logger.Console.WriteLine("Client " + ThisClient.Username + " disconnected. (YSFHQ Authentication Failed) (" + ThisClient.ClientObject.PublicIP.ToString() + ")/(" + GetIPAddress.FromID(ID).ToString() + ").");
                    Logger.Log.SystemMessage("Authentication Failure in YSFHQ Module for User: + \"" + ThisClient.Username + "\". (" + ThisClient.ClientObject.PublicIP.ToString() + ")/(" + GetIPAddress.FromID(ID).ToString() + ").");
                    //return true; //DISABLE THIS ONCE TESTING IS COMPLETE!!!
                    return false;
                }
            }
        }
    }
}
