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
            public static AutoResetEvent ConsoleInputWaiter = new AutoResetEvent(false);
            public static List<AutoResetEvent> ConsoleWriteWait = new List<AutoResetEvent>();
            public static List<String> CommandsTyped = new List<String>();
            public static int CommandHighlighted = 0;

            public static bool _LockInput = false;

            public static void LockInput(bool Locked)
            {
                if (Locked)
                {
                    Logger.Console.ClearLine();
                    _LockInput = true;
                }
                else
                {
                    ColorHandling.ConsoleHandler(ConsolePrompt + Input);
                    _LockInput = false;
                }
            }

            public static string Input = ""; //The command currently being typed into the console.
            public static string ConsolePrompt = "&5Ørb#> &f";
            

            public static void Initialise()
            {
                if (Server.ConsoleMode == false) return;
                Logger.Console.Write("");//The prompt is added automatically.
                Thread ListeningThread = new Thread(new ThreadStart(Listener));
                ListeningThread.Start();
            }


            public static void Listener()
            {
                CommandsTyped.Add("<No More Previous Commands>");
                CommandsTyped.Add(Input);
                CommandHighlighted = 1;
                while (true)
                {
                    System.ConsoleKeyInfo temp = System.Console.ReadKey(true);
                    if (_LockInput)
                    {
                        continue;
                    }
                    switch (temp.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (Input.Length > 0)
                            {
                                Input = Input.Remove(Input.Length - 1);
                                CommandsTyped[CommandsTyped.Count - 1] = Input;
                            }
                            break;
                        case ConsoleKey.Escape:
                            Input = "";
                            CommandsTyped[CommandsTyped.Count - 1] = Input;
                            break;
                        case ConsoleKey.Enter:
                            if (Input != "")
                            {
                                CommandsTyped[CommandsTyped.Count - 1] = Input;
                                CommandsTyped.Add(Input);
                                CommandHighlighted = CommandsTyped.Count() - 1;
                                Thread CommandHandle = new Thread(() => CommandManager.Process(Server.OrbConsole, CommandsTyped[CommandsTyped.Count - 1]));
                                CommandHandle.Start();
                                Input = "";
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (CommandHighlighted > 0)
                            {
                                CommandHighlighted--;
                                Input = CommandsTyped[CommandHighlighted];
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (CommandHighlighted == CommandsTyped.Count - 1)
                            {
                                Input = "";
                                break;
                            }
                            CommandHighlighted++;
                            Input = CommandsTyped[CommandHighlighted];
                            if (CommandHighlighted == CommandsTyped.Count - 1)
                            {
                                Input = "";
                            }
                            break;
                        default:
                            if (char.IsControl(temp.KeyChar) && temp.Key != ConsoleKey.UpArrow && temp.Key != ConsoleKey.UpArrow) break;
                            else
                            {
                                Input += temp.KeyChar;
                                CommandsTyped[CommandsTyped.Count - 1] = Input;
                            }
                            break;
                    }
                    ColorHandling.ConsoleHandler(ConsolePrompt + Input);
                }
            }

        }
    }
}
