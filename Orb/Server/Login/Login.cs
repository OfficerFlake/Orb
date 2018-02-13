using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;

namespace Orb
{
    public static partial class Server
    {
        public static partial class Login
        {
            public static void Handle(Server.NetObject Parent, Network.Packets.Type01_Login ThisPacket)
            {
                Parent.Username = ThisPacket.FullUsername;

                if (Parent.Username.ToUpperInvariant() != "PHP BOT")
                {
                    Logger.Console.WriteLine("&e" + Parent.Username + " Connecting...");
                }

                //Create New User?
                //Logger.Console.WriteLine(Parent.Username + " Searching User");
                if (Database.UserDB.Find(Parent.Username) == Database.UserDB.Nobody)
                {
                    //Logger.Console.WriteLine(Parent.Username + " Searching User (" + Parent.Username + ")");
                    //Wasn't in the database... Shall we try and find him in the folders?
                    //Database.UserDB.Load(Parent.Username);
                    //Worked, load his data.
                    Parent.UserObject = Database.UserDB.Find(Parent.Username);
                    if (Parent.UserObject == Database.UserDB.Nobody)
                    {
                        Database.UserDB.User ThisUserObject = Database.UserDB.New(Parent.Username);
                        ThisUserObject.SaveAll();
                        Parent.UserObject = ThisUserObject;
                        //Logger.Console.WriteLine(Parent.Username + " NEW User (" + Parent.UserObject.Name + ")");
                    }
                    else
                    {
                        //Logger.Console.WriteLine(Parent.Username + " FOUND User (" + Parent.UserObject.Name + ")");
                    }
                }
                else
                {
                    Parent.UserObject = Database.UserDB.Find(Parent.Username);
                    Database.UserDB.Load(Parent.Username);
                    //Logger.Console.WriteLine(Parent.Username + " MATCHED User (" + Parent.UserObject.Name + ")");
                }

                //User is not banned?
                if (Parent.UserObject.Banned == true)
                {
                    if (Parent.UserObject.BanExpires < DateTime.Now && Parent.UserObject.BanExpires != new DateTime()) //New Datetime is a zero time!
                    {
                        Logger.Console.WriteLine("&cUSER \"" + Parent.Username + "\" WAS UNBANNED AS THEIR BAN EXPIRY HAS PASSED!");
                        Logger.Console.WriteLine("&c    " + Parent.UserObject.BanExpires.ToString() + " < " + DateTime.Now.ToString());
                        Parent.UserObject.Banned = false;
                        Parent.UserObject.Save(Database.UserDB.Strings.Banned);
                    }
                    else
                    {
                        Parent.ClientObject.SendMessage("YOU ARE BANNED FROM THE SERVER!");
                        Logger.Console.WriteLine("&eBANNED USER \"" + Parent.Username + "\" ATTEMPTED TO LOG IN.");
                        if (Parent.UserObject.BanExpires == new DateTime())
                        {
                            Parent.ClientObject.SendMessage("THIS IS A PERMANENT BAN!");
                        }
                        else
                        {
                            Parent.ClientObject.SendMessage("YOUR BAN WILL EXPIRE ON: " + Parent.UserObject.BanExpires.ToString());
                        }
                        Parent.Close();
                        return;
                    }
                }

                //YSFHQ Mode?
                if (Database.Settings.UseYSFHQAuthentication && Parent.Username.ToUpperInvariant() != "PHP BOT")
                {
                    int i = Parent.Username.Length;
                    if (!(Server.YSFHQ.Authenticate(Parent)))
                    {
                        Parent.Close();
                        return;
                    }
                    foreach (Server.NetObject ThisNetObj in Server.ClientList.ToArray().Where(x => x.Username == Parent.Username && x != Parent))
                    {
                        ThisNetObj.ClientObject.SendMessage("Connected From Elsewhere.");
                        ThisNetObj.Close();
                    }
                    #region DECREPTED
                    //DECREPTED
                    /*
                    Parent.Username = Server.YSFHQ.GetFullUsername.FromShortName(Parent.Username);
                    if (i >= 15)
                    {
                        Parent.ClientObject.SendMessage("NOTE: Orb has assumed that you have logged in with the correct YSFHQ username, as your name is over 15 characters long.");
                        Parent.ClientObject.SendMessage("If no commands etc. work for you, you will need to log out and rejoin with your YSFHQ name only.");
                    }
                    */
                    #endregion
                }
                #region DECREPTED
                //DECREPTED
                /*
                else
                {
                    //Is 15 Characters? Confirm no Overflow!
                    if (Parent.Username.Length >= 15)
                    {
                        Parent.ClientObject.SendMessage("Your username may be too long!");
                        Parent.ClientObject.SendMessage("Orb only supports usernames up to 15 characters!");
                        Parent.ClientObject.SendMessage("Please confirm the length of your username by typing \"OK\" now in YSFlight Chat.");
                        while (true)
                        {
                            Network.Packet CyclePacket = Parent.ClientObject.GetOnePacket();
                            if (CyclePacket.Type == 32)
                            {
                                Network.Packets.Type32_ChatMessage ThisChatMessage = new Network.Packets.Type32_ChatMessage(CyclePacket);
                                Parent.ClientObject.SendMessage(ThisChatMessage.Message.Remove(0, 17).ToUpper());
                                Parent.ClientObject.SendMessage(ThisChatMessage.Message);
                                if (ThisChatMessage.Message.Remove(0, 17).ToUpper().StartsWith("OK"))
                                {
                                    Parent.ClientObject.SendMessage("Thanks! Remember, you can bypass this test by using a 14 character name or less in future! ;)");
                                    break;
                                }
                                else
                                {
                                    Parent.ClientObject.SendMessage("Sorry, I didn't get that. Can you try again please? Or disconnect and return with a 15 char name or less!");
                                }
                            }
                        }
                    }
                }
                */
                #endregion

                //PASSWORD Mode?
                if (!Database.Settings.UseYSFHQAuthentication && Parent.Username.ToUpperInvariant() != "PHP BOT")
                {
                    int i = Parent.Username.Length;
                    if (!(Server.Password.Authenticate(Parent)))
                    {
                        Parent.Close();
                        return;
                    }
                    foreach (Server.NetObject ThisNetObj in Server.ClientList.ToArray().Where(x => x.Username == Parent.Username && x != Parent))
                    {
                        ThisNetObj.ClientObject.SendMessage("Connected From Elsewhere.");
                        ThisNetObj.Close();
                    }
                }

                //
                //Successful Login from here!
                //

                //Send Welcome File!
                string InputFile = "./Welcome.txt";
                Utilities.IO.PrepareFile(InputFile);
                string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                if (Parent.Username.ToUpperInvariant() != "PHP BOT")
                {
                    foreach (string Line in InfoFileContents)
                    {
                        Parent.ClientObject.SendMessage(Line);
                    }
                }
                else
                {
                    Parent.ClientObject.SendMessage("You have joined the server as \"PHP bot\".\nThis username is reserved for the sole purpose of polling the server for YSFHQ.\nYou will be kicked from the server immediatly upon logging in.");
                }

                //Update Player Login Count!
                Parent.UserObject.LoginCount++;
                Parent.UserObject.Save(Database.UserDB.Strings.LoginCount);
                Parent.UserObject.LastIP = (Parent.ClientObject.ClientSocket.RemoteEndPoint as IPEndPoint).Address;
                Parent.UserObject.Save(Database.UserDB.Strings.LastIP);

                if (Parent.UserObject.LoginCount.ToString() == "11") Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "th login!");
                else if (Parent.UserObject.LoginCount.ToString() == "12") Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "th login!");
                else if (Parent.UserObject.LoginCount.ToString() == "13") Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "th login!");
                else if (Parent.UserObject.LoginCount.ToString().EndsWith("1")) Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "st login!");
                else if (Parent.UserObject.LoginCount.ToString().EndsWith("2")) Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "nd login!");
                else if (Parent.UserObject.LoginCount.ToString() .EndsWith("3")) Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "rd login!");
                else Parent.ClientObject.SendMessage("This is your " + Parent.UserObject.LoginCount + "th login!");

                Parent.ClientObject.SendMessage("You last logged in on " + Utilities.DateTimeUtilities.ToYearAscending(Utilities.DateTimeUtilities.FormatDateTime(Parent.UserObject.DateLastVisited)) + ".");
                Parent.UserObject.DateLastVisited = DateTime.Now;
                Parent.UserObject.SaveAll();


                if (Parent.Username.ToUpperInvariant() != "PHP BOT")
                {
                    Logger.Console.WriteLine("&a" + Parent.Username + " Connected.");
                    Logger.Console.WriteLine("&e" + Parent.Username + " Logging in...");
                    Server.ClientList.Except(Parent).SendMessage(Parent.Username + " joined the server.");
                }
                Server.ClientList.Add(Parent);
                ServerGUI.RefreshUsers();
                //Logger.Console.WriteLine(Parent.Username + " PRE");
                Parent.HostObject.Send(ThisPacket.Serialise());
                //Logger.Console.WriteLine(Parent.Username + " POST");
                //Logger.Console.WriteLine(Parent.Username + " log in request sent...");
            }
        }
    }
}