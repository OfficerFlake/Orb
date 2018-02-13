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
            public static void WriteLine(string MessageIn)
            {
                if (Database.Settings.GUIMode)
                {
                    try
                    {
                        ServerGUI.Write(MessageIn + "\n");
                    }
                    catch
                    {
                        //GUI not loaded...
                        Console.Write(MessageIn + "\n");
                    }
                    return;
                }
                else
                {
                    Console.Write(MessageIn + "\n");
                }
            }
            public static void WriteLine()
            {
                WriteLine("");
            }
        }
    }
}
