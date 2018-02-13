using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Logger
    {
        public static partial class Console
        {
            public static void Write(string MessageIn)
            {
                if (Database.Settings.GUIMode)
                {
                    try
                    {
                        ServerGUI.Write(MessageIn);
                    }
                    catch
                    {
                        //GUI not loaded...
                        ColorHandling.ConsoleHandler(MessageIn);
                        if (Logger.Console._LockInput) return;
                        ColorHandling.ConsoleHandler(Logger.Console.ConsolePrompt + Logger.Console.Input);
                    }
                    return;
                }
                else
                {
                    ColorHandling.ConsoleHandler(MessageIn);
                    if (Logger.Console._LockInput) return;
                    ColorHandling.ConsoleHandler(Logger.Console.ConsolePrompt + Logger.Console.Input);
                }
            }
            public static void Write()
            {
                Write("");
            }
        }
    }
}