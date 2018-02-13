using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Orb
{
    public static class ServerGUI
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static ServerConsole ServerConsoleForm;
        public static Thread GUIThread;
        public static bool ConsoleOutputAutoScrollDown = true;
        public static ManualResetEvent FormLoadingWaiter = new ManualResetEvent(false);
        public static volatile List<AutoResetEvent> ConsoleWriteWait = new List<AutoResetEvent>();

        [STAThread]

        public static void Start()
        {
            if (Boolean.Parse(Database.Settings.Get(Database.Settings.Strings.GUIMode).ToString()))
            {
                if (Server.ConsoleMode)
                {
                    ColorHandling.ConsoleHandler("\r" + new string(' ', System.Console.WindowWidth));
                    System.Console.SetCursorPosition(0, System.Console.CursorTop - 1);
                    ColorHandling.ConsoleHandler("&eLoading GUI...\n");
                    ColorHandling.ConsoleHandler(Logger.Console.ConsolePrompt + Logger.Console.Input);
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ServerConsoleForm = new ServerConsole();
                //Run();

                GUIThread = new Thread(new ThreadStart(Run));
                GUIThread.Start();
                FormLoadingWaiter.WaitOne();
                
            }
        }

        public static void Run()
        {
            Application.Run(ServerConsoleForm);
            //Now the window has been closed, we need to shutdown.
            Environment.Exit(0);
        }

        public static void Write(string Input)
        {
            if (ServerConsoleForm == null)
            {
                return;
            }
            if (ServerConsoleForm.InvokeRequired)
            {
                MethodInvoker action = delegate
                {
                    ColorHandling.GUIHandler(ServerConsoleForm.ConsoleOutput, Input);
                    //ServerConsoleForm.ConsoleOutput.Text += Input;
                };
                ServerConsoleForm.ConsoleOutput.BeginInvoke(action);
            }
            else
            {
                ColorHandling.GUIHandler(ServerConsoleForm.ConsoleOutput, Input);
                //ServerConsoleForm.ConsoleOutput.Text += Input;
            }
        }

        public static void PasswordMode(bool passwordon)
        {
            if (ServerConsoleForm == null)
            {
                return;
            }
            if (ServerConsoleForm.InvokeRequired)
            {
                MethodInvoker action = delegate
                {
                    if (passwordon)
                    {
                        ServerConsoleForm.ConsoleInput.UseSystemPasswordChar = true;
                    }
                    else
                    {
                        ServerConsoleForm.ConsoleInput.UseSystemPasswordChar = false;
                    }
                };
                ServerConsoleForm.ConsoleOutput.BeginInvoke(action);
            }
            else
            {
                if (passwordon)
                {
                    ServerConsoleForm.ConsoleInput.UseSystemPasswordChar = true;
                }
                else
                {
                    ServerConsoleForm.ConsoleInput.UseSystemPasswordChar = false;
                }
            }
        }

        public static void LockInput(bool Locked)
        {
            if (ServerConsoleForm == null)
            {
                return;
            }
            if (ServerConsoleForm.InvokeRequired)
            {
                MethodInvoker action = delegate
                {
                    if (Locked)
                    {
                        ServerConsoleForm.ConsoleInput.Enabled = false;
                    }
                    else
                    {
                        ServerConsoleForm.ConsoleInput.Enabled = true;
                        ServerGUI.ServerConsoleForm.ConsoleInput.Focus();
                    }
                };
                ServerConsoleForm.ConsoleOutput.BeginInvoke(action);
            }
            else
            {
                if (Locked)
                {
                    ServerConsoleForm.ConsoleInput.Enabled = false;
                }
                else
                {
                    ServerConsoleForm.ConsoleInput.Enabled = true;
                    ServerGUI.ServerConsoleForm.ConsoleInput.Focus();
                }
            }
        }

        public static void ClearLog()
        {
            if (ServerConsoleForm == null)
            {
                return;
            }
            if (ServerConsoleForm.InvokeRequired)
            {
                MethodInvoker action = delegate
                {
                    ServerConsoleForm.ConsoleOutput.Text = "";
                };
                ServerConsoleForm.ConsoleOutput.BeginInvoke(action);
            }
            else
            {
                ServerConsoleForm.ConsoleOutput.Text = "";
            }
        }

        public static void RefreshUsers()
        {
            //DO NOT DO THIS IF IN CONSOLE MODE!
            if (!(Database.Settings.GUIMode))
            {
                return;
            }
            if (ServerConsoleForm.InvokeRequired)
            {
                MethodInvoker action = delegate
                {
                    ServerConsoleForm.ConsoleUserList.Text = "";
                    List<Server.NetObject> ClientListCache = Server.ClientList.ToArray().ToList();
                    foreach (Server.NetObject ThisClient in ClientListCache)
                    {
                        ServerConsoleForm.ConsoleUserList.Text += ThisClient.Username + "\n";
                    }
                };
                ServerConsoleForm.ConsoleOutput.BeginInvoke(action);
            }
            else
            {
                ServerConsoleForm.ConsoleUserList.Text = "";
                List<Server.NetObject> ClientListCache = Server.ClientList.ToArray().ToList();
                foreach (Server.NetObject ThisClient in ClientListCache)
                {
                    ServerConsoleForm.ConsoleUserList.Text += ThisClient.Username + "\n";
                }
            }
        }
    }
}
