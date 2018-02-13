using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Orb
{
    public static partial class Server
    {
        public static partial class Shutdown
        {
            public static void MasterClose(string Message, int Seconds)
            {
                ServerGUI.LockInput(true);
                Logger.Console.LockInput(true);
                Logger.Log.SystemMessage("MASTERCLOSE: " + Message + "\n");
                Logger.Console.WriteLine();
                Logger.Console.WriteLine("&c" + Message + "\n");
                Logger.Console.WriteLine("");
                for (int i = Seconds -1; i > 0; i--)
                {
                    Logger.Console.WriteLine(String.Format("&cOrb Will Close In &f{0}&c Seconds.", i + 1));
                    Thread.Sleep(1000);
                    Logger.Console.ClearLine();
                }
                Logger.Console.WriteLine(String.Format("&cOrb Will Close In &f1&c Second."));
                Thread.Sleep(1000);
                Logger.Console.ClearLine();
                for (int i = 3; i > 0; i--)
                {
                    Logger.Console.WriteLine(String.Format("&c!!!Orb Closing!!!", i));
                    Thread.Sleep(500);
                    Logger.Console.ClearLine();
                    Thread.Sleep(500);
                }
                Environment.Exit(0);
            }
        }
    }
}
