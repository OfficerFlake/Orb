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
            public static void ClearLine()
            {
                if (Database.Settings.GUIMode)
                {
                    ServerGUI.Write("\r");
                }
                else
                {
                    Logger.Console.Write("\r" + new string(' ', System.Console.WindowWidth));
                    try
                    {
                        System.Console.SetCursorPosition(0, System.Console.CursorTop - 1);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
