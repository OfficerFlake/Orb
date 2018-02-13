using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Orb
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            //PRE OPERATION EVENTS
            try
            {
                Console.Title = "Orb for YSFlight";
                Console.CursorVisible = false;
                Server.ConsoleMode = true;
                Logger.Console.LockInput(true);
            }
            catch
            {
                Database.Settings.Set(Database.Settings.Strings.GUIMode, true);
                Server.ConsoleMode = false;
                //Console is not enabled
            }

            
            //LOAD SETTINGS
                Database.Settings.LoadAll();

            //GUI LOADING
                #region Disable Console
                if (Database.Settings.GUIMode == true)
                {
                    var handle = GetConsoleWindow();

                    // Hide
                    ShowWindow(handle, SW_HIDE);
                    Server.ConsoleMode = false;

                    // Show
                    //ShowWindow(handle, SW_SHOW);
                }
                #endregion
                //Database.Settings.Set(Database.Settings.Strings.GUIMode, true);
                //Database.Settings.Set(Database.Settings.Strings.ProxyMode, true);
                Logger.Console.Initialise();
                //Logger.Console.WriteLine(DateTime.Now.ToString());
                ServerGUI.Start();
                ServerGUI.LockInput(true);

            //DATABASE LOADING
                Logger.Log.Silent = false;
                Logger.Log.SystemMessage("LOADING DATABASE");
                Logger.Console.WriteLine("&9LOADING DATABASE...");
                Logger.Log.SystemMessage("Loading Salt.");
                Logger.Console.WriteLine("&5    Loading Salt...");
                Database.LoadSalt();
                Logger.Log.SystemMessage("Loading Settings.");
                Logger.Console.WriteLine("&5    Loading Settings...");
                Database.Settings.LoadAll();
                Database.Security.StartLock.Run();
                Logger.Log.SystemMessage("Loading Groups, Ranks, and their Permissions.");
                Logger.Console.WriteLine("&5    Loading Groups, Ranks, and their Permissions....");
                Database.GroupDB.LoadAll();
                Logger.Log.SystemMessage("Loading Users, Group References and their Permissions.");
                Logger.Console.WriteLine("&5    Loading Users, Group References and their Permissions...");
                Database.UserDB.LoadAll();
                Logger.Console.WriteLine("&5    Loading Commands.");
                Commands.LoadAll();
                if (Database.NewSaltGenerated)
                {
                    foreach (Database.UserDB.User ThisUser in Database.UserDB.List)
                    {
                        ThisUser.Password = "";
                        ThisUser.UsePassword = false;
                        ThisUser.SaveAll();
                    }
                    if (Database.UserDB.List.Count() > 0)
                    {
                        Logger.Console.WriteLine("&d    Because new Salt was generated, all passwords for all users have been reset!");
                    }
                    else
                    {
                        if (Database.UserDB.List.Count() == 0 && Database.GroupDB.List.Count() == 0)
                        {
                            #region Create Defaults
                            //No Users or Groups in the database, and no orb.dll? sound like a first launch to me!

                            //Create The ADMIN
                            Database.UserDB.User AdminUser = Database.UserDB.New("Admin");
                            Database.UserDB.User ModUser = Database.UserDB.New("Mod");

                            Database.GroupDB.Group ServerGroup = Database.GroupDB.New("SERVER");
                            Database.GroupDB.Group.Rank AdminRank = ServerGroup.NewRank("ADMIN");
                            Database.GroupDB.Group.Rank ModRank = ServerGroup.NewRank("MOD");
                            AdminRank.Permissions.MakeSuper();
                            ModRank.Permissions.MakeModerator();

                            Database.UserDB.User.GroupReference AdminUserGR = AdminUser.AddToGroup(ServerGroup);
                            Database.UserDB.User.GroupReference ModUserGR = ModUser.AddToGroup(ServerGroup);

                            AdminUserGR.Rank = AdminRank;
                            ModUserGR.Rank = ModRank;

                            AdminUser.GroupRepresented = ServerGroup;
                            ModUser.GroupRepresented = ServerGroup;

                            AdminUser.SaveAll();
                            ModUser.SaveAll();

                            ServerGroup.Founder = AdminUser;
                            ServerGroup.SaveAll();
                            #endregion
                            Logger.Console.WriteLine("&d    Default Groups/Users instated!");
                            Version.WriteNewToOrbHelpFile();
                        }
                    }
                }
                Logger.Console.WriteLine("&5    Loading Complete!");

                Utilities.WelcomeFile.WriteDefault();
                //Thread.Sleep(5000);
                //Logger.Console.WriteLine(DateTime.Now.ToString());


            //DISPLAY DATABASE RESULTS
                //Logger.Console.Write(Utilities.IO.GetAllGroupsRanks());
                //Logger.Console.WriteLine();
                //Logger.Console.Write(Utilities.IO.GetAllUsersGroups());
                //Logger.Console.WriteLine();
                //Database.PermissionDB.PermissionsCore.CheckPermission(Database.UserDB.Find("BAWB"), "Default");

            //SAVE DATABASE
                //Database.GroupDB.SaveAll();
                //Database.UserDB.SaveAll();
                //Database.Settings.SaveAll();

            //RUN SERVER
                //ServerGUI.ClearLog();
                //Logger.Console.WriteLine("&bLAGSWITCH100SECONDSIMPLEMENTED");
                //Thread.Sleep(100000);
                #if !DEBUG
                    Logger.Log.SystemMessage("STARTING SERVER");
                    Logger.Console.WriteLine("&9STARTING SERVER.");
                    Thread.Sleep(1000);
                    Logger.Console.ClearLine();
                    Logger.Console.WriteLine("&9STARTING SERVER..");
                    Thread.Sleep(1000);
                    Logger.Console.ClearLine();
                    Logger.Console.WriteLine("&9STARTING SERVER...");
                    Thread.Sleep(1000);
                    Logger.Console.ClearLine();
                    //Thread.Sleep(50000);
                #endif
                Logger.Console.Clear();
                //Logger.Console.WriteLine(Database.Settings.ServerIP.ToString());
                //Logger.Console.WriteLine(Database.Settings.ServerPort.ToString());
                Server.Start();

                

            //SHUTDOWN
                //Server.Shutdown.MasterClose("Test Complete", 10);
        }
    }
}
