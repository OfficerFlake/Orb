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
            public static string ReadPass()
            {
                if (Database.Settings.GUIMode)
                {
                    ServerGUI.PasswordMode(true);
                    ConsoleInputWaiter.WaitOne();
                    ServerGUI.PasswordMode(false);
                    string output = ServerConsole.CommandsTyped[CommandsTyped.Count - 1];

                    //Remove the password from command history, for security.
                    ServerConsole.CommandsTyped.RemoveAt(ServerConsole.CommandsTyped.Count - 1); 
                    ServerConsole.CommandHighlighted--;

                    return output;
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
                            Console.Write("*");
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
