using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Orb
{
    public static partial class Logger
    {
        public static partial class Console
        {
            public static void Clear()
            {
                if (Database.Settings.GUIMode)
                {
                    ServerGUI.ClearLog();
                }
                else
                {
                    System.Console.Clear();
                }
            }
        }
    }
}
