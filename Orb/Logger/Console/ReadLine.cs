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
            
            public static string ReadLine()
            {
                if (Database.Settings.GUIMode)
                {
                    ConsoleInputWaiter.WaitOne();
                    return CommandsTyped[CommandsTyped.Count - 1]; //need to shift back due to array size.
                }
                else
                {
                    #region Console
                    string output = "";
                    // Backspace Should Not Work
                    ConsoleKeyInfo key = System.Console.ReadKey(true);
                    while (key.Key != ConsoleKey.Enter)
                    {
                        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                        {
                            output += key.KeyChar;
                            Console.Write(key.KeyChar.ToString());
                        }
                        else
                        {
                            if (key.Key == ConsoleKey.Backspace && output.Length > 0)
                            {
                                output = output.Substring(0, (output.Length - 1));
                                Console.Write("\b \b");
                            }
                        }
                        if (key.Key == ConsoleKey.Enter)
                        {
                            Console.WriteLine("");
                            return output;
                        }
                        key = System.Console.ReadKey(true);
                    }
                    Console.WriteLine("");
                    return output;
                    #endregion
                }
            }
        }
    }
}
