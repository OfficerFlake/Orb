using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Orb
{
    //public static partial class GUI
    //{
        public partial class ServerConsole : Form
        {
            public static List<String> CommandsTyped = new List<String>();
            public static int CommandHighlighted = 0;

            public ServerConsole()
            {
                InitializeComponent();
                CommandsTyped.Add("<No More Previous Commands>");
                CommandsTyped.Add(ConsoleInput.Text);
                CommandHighlighted = 1;
                Activate();
            }

            private void SubmitLine(object sender, KeyEventArgs e)
            {
                switch (e.KeyCode)
                {
                    case Keys.Back:
                        if (ConsoleInput.Text.Length > 0)
                        {
                            CommandsTyped[CommandsTyped.Count - 1] = ConsoleInput.Text;
                        }
                        break;
                    case Keys.Escape:
                        ConsoleInput.Text = "";
                        CommandsTyped[CommandsTyped.Count - 1] = ConsoleInput.Text;
                        break;
                    case Keys.Enter:
                        if (ConsoleInput.Text != "")
                        {
                            CommandsTyped[CommandsTyped.Count - 1] = ConsoleInput.Text;
                            CommandsTyped.Add(ConsoleInput.Text);
                            CommandHighlighted = CommandsTyped.Count() - 1;
                            Thread CommandHandle = new Thread(() => CommandManager.Process(Server.OrbConsole, CommandsTyped[CommandsTyped.Count - 1]));
                            CommandHandle.Start();
                            ConsoleInput.Text = "";

                            Logger.Console.ConsoleInputWaiter.Set();
                        }
                        break;
                    case Keys.Up:
                        if (CommandHighlighted > 0)
                        {
                            CommandHighlighted--;
                            CommandsTyped[CommandsTyped.Count - 1] = ConsoleInput.Text;
                            ConsoleInput.Text = CommandsTyped[CommandHighlighted];
                        }
                        break;
                    case Keys.Down:
                        if (CommandHighlighted == CommandsTyped.Count - 1)
                        {
                            ConsoleInput.Text = "";
                            break;
                        }
                        CommandHighlighted++;
                        ConsoleInput.Text = CommandsTyped[CommandHighlighted];
                        if (CommandHighlighted == CommandsTyped.Count - 1)
                        {
                            ConsoleInput.Text = "";
                        }
                        break;
                    default:
                        //ConsoleInput.Text += e.KeyChar;
                        CommandsTyped[CommandsTyped.Count - 1] = ConsoleInput.Text;
                        break;
                //EndSwitch Block.
                }
            }

            private void ResumeMainThread(object sender, EventArgs e)
            {
                //Tell the program that the form is loaded!
                ServerGUI.FormLoadingWaiter.Set();
                Activate(); //SWAP to this window.
            }

            private void TextChangedActions(object sender, EventArgs e)
            {
            }

            private void ArrowKeyOverrides(object sender, PreviewKeyDownEventArgs e)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                        e.IsInputKey = true;
                        break;
                }
            }
        }
    //}
}
