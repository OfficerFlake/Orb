using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Orb
{
    public static partial class Database
    {
        public static partial class Security
        {
            public static class StartLock
            {
                public static bool Authenticated = false;
                public static void Run()
                {
                    Authenticated = false;
                    if (!Database.Settings.StartLock)
                    {
                        Logger.Log.SystemMessage("The Servers Start Lock is not in place.");
                        Authenticated = true;
                        return;
                    }
                    if (Database.Settings.StartPass == "") Authenticated = true;
                    double Error = 0;
                    string Password = "";
                    int WaitTime = 0;
                    Logger.Log.SystemMessage("The Servers Start Lock is in place.");
                    Logger.Console.WriteLine("Please enter the password to start the server.");
                    Logger.Console.WriteLine("=============================================");
                    Logger.Console.WriteLine("");
                    while (Database.Settings.StartLock && !Authenticated)
                    {
                        Logger.Console.Write("Enter Password: ");
                        Password = Logger.Console.ReadPass();
                        string MaskedPass = new String('*', Password.Length);
                        if (Boolean.Parse(Database.Settings.Get(Database.Settings.Strings.GUIMode).ToString()))
                        {
                            Logger.Console.WriteLine(MaskedPass);
                        }
                        if (Password != Database.Settings.StartPass)
                        {
                            Error++;
                            if (Error < 3) Logger.Console.WriteLine("Password Incorrect, Please try again.");
                            else
                            {
                                ServerGUI.LockInput(true); //Blocks the user from entering more passwords and flooding the feed.
                                WaitTime = (int)Math.Pow(10d, (Error / 3));
                                Console.WriteLine("");
                                for (int i = WaitTime; i > 0; i--)
                                {
                                    Logger.Console.Write(String.Format("\rPassword Incorrect. Please wait {0} seconds before trying again.", i));
                                    Thread.Sleep(1000);
                                    Logger.Console.ClearLine();
                                }
                                ServerGUI.LockInput(false); //Allows the user to enter passwords again.
                                Logger.Console.WriteLine("\rPassword Incorrect. Please try again.");
                            }
                        }
                        else
                        {
                            Logger.Console.WriteLine("Password Correct.");
                            for (int i = 3; i < 0; i--)
                            {
                                Logger.Console.Write(String.Format("Server Launching in {0} Seconds.", i));
                            }
                            Logger.Console.Clear();
                            Authenticated = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}

