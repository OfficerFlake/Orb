using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Orb
{
    public static class ColorHandling
    {
        /// <summary> List of color names indexed by their id. </summary>
        #region Colours
        public static readonly SortedList<char, string> ColorNames = new SortedList<char, string> {
            { '0', "black" },
            { '1', "navy" },
            { '2', "green" },
            { '3', "teal" },
            { '4', "maroon" },
            { '5', "purple" },
            { '6', "olive" },
            { '7', "silver" },
            { '8', "gray" },
            { '9', "blue" },
            { 'a', "lime" },
            { 'b', "aqua" },
            { 'c', "red" },
            { 'd', "magenta" },
            { 'e', "yellow" },
            { 'f', "white" }
        };
        #endregion

        public static string[] Converter(String input) {
            
            List<string> output = new List<string>();
            string nextoutput = "";
            char nextcolor = 'f';
            if (input == null) return output.ToArray();
            if (input.IndexOf('&') == -1)
            {
                output.Add("&f" + input);
                return output.ToArray();
            }
            else
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '&')
                    {
                        if (input.Length > i)
                        {
                            foreach (char colorid in ColorHandling.ColorNames.Keys)
                            {
                                if (colorid == input[i + 1])
                                {
                                    output.Add("&" + nextcolor + nextoutput);
                                    nextoutput = "";
                                    nextcolor = colorid;
                                    continue;
                                }
                            }
                            foreach (char colorid in "syprhwmi")
                            {
                                if (colorid == input.ToLower()[i + 1])
                                {
                                    output.Add("&" + nextcolor + nextoutput);
                                    nextoutput = "";
                                    nextcolor = colorid;
                                    continue;
                                }
                            }
                            continue;
                        }
                    }
                    else if (i > 0)
                    {
                        if (input[i - 1] == '&')
                        {
                            foreach (char colorid in ColorHandling.ColorNames.Keys)
                            {
                                if (colorid == input[i])
                                {
                                    continue;
                                }
                            }
                            foreach (char colorid in "syprhwmi")
                            {
                                if (colorid == input.ToLower()[i])
                                {
                                    output.Add("&" + nextcolor + nextoutput);
                                    nextoutput = "";
                                    nextcolor = colorid;
                                    continue;
                                }
                            }
                            continue;
                        }
                    }
                    if (input[i] == '&') nextoutput += "<3>";
                    nextoutput += input[i];
                }
                output.Add("&" + nextcolor + nextoutput);
                return output.ToArray();
            }
        }

        public static void GUIHandler(System.Windows.Forms.RichTextBox ConsoleOutput, String ThisMessage) {
            AutoResetEvent ReadyToResume = new AutoResetEvent(false);
            ServerGUI.ConsoleWriteWait.Add(ReadyToResume);
            while (ServerGUI.ConsoleWriteWait.Count() > 1)
            {
                if (!(ServerGUI.ConsoleWriteWait.Contains(ReadyToResume))) ServerGUI.ConsoleWriteWait.Add(ReadyToResume);
                ReadyToResume.WaitOne(1000);
                ServerGUI.ConsoleWriteWait.RemoveAll(x => x == ReadyToResume);
            }
            #region AddMessages
            if (ThisMessage.StartsWith("\r"))
            {
                List<string> myList = ConsoleOutput.Lines.ToList();
                if (myList.Count > 0)
                {
                    ConsoleOutput.Select(ConsoleOutput.GetFirstCharIndexFromLine(ConsoleOutput.Lines.Count() - 2), ConsoleOutput.GetFirstCharIndexFromLine(ConsoleOutput.Lines.Count() - 1));
                        //^^ -1 == actual length, -2, previous line. -1  is EQUAL to the end of the line, which crashes it!
                    //ConsoleOutput.SelectionBackColor = System.Drawing.Color.FromArgb(240, 240, 0);
                    ConsoleOutput.ReadOnly = false; //Pain in the mcarse this damned limitation...
                    ConsoleOutput.SelectedText = "";
                    ConsoleOutput.ReadOnly = true;
                    ConsoleOutput.Select(ConsoleOutput.Text.Length, 0); //Move the cursor to the end.
                }
                ThisMessage = ThisMessage.Remove(0, 1); // "/r" is one char, not two!
            }
            foreach (string msgToAppend in Converter(ThisMessage))
            {
                int oldLength = ConsoleOutput.Text.Length;
                /*
                if (msgToAppend.Remove(0, 2).Length + oldLength > ConsoleOutput.MaxLength)
                {
                    ConsoleOutput.Clear();
                }
                */
                try
                {
                    ConsoleOutput.AppendText(msgToAppend.Remove(0, 2));
                    ConsoleOutput.Select(oldLength, msgToAppend.Length - 2);
                    switch (msgToAppend.ToLower()[1])
                    {
                        case '0':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(32, 32, 32);
                            break;
                        case '1':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(0, 0, 127);
                            break;
                        case '2':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(0, 127, 0);
                            break;
                        case '3':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(0, 127, 127);
                            break;
                        case 'y':
                        case '4':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(127, 48, 0);
                            break;
                        case 'm':
                        case 'i':
                        case '5':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(127, 0, 127);
                            break;
                        case '6':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(127, 127, 0);
                            break;
                        case '7':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(186, 186, 186);
                            break;
                        case '8':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(117, 117, 117);
                            break;
                        case '9':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(96, 96, 240);
                            break;
                        case 'h':
                        case 'a':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(0, 240, 0);
                            break;
                        case 'p':
                        case 'b':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(0, 240, 240);
                            break;
                        case 'w':
                        case 'c':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(240, 0, 0);
                            break;
                        case 'd':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(240, 0, 240);
                            break;
                        case 's':
                        case 'r':
                        case 'e':
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(240, 240, 0);
                            break;
                        default:
                            ConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(240, 240, 240);
                            break;
                    }
                    oldLength = ConsoleOutput.Text.Length;
                }
                catch
                {
                }
            }
            #endregion;
            ServerGUI.ConsoleWriteWait.RemoveAll(x=>x == ReadyToResume);
            ConsoleOutput.Refresh(); //Refresh The Console
            if (ServerGUI.ConsoleOutputAutoScrollDown)
            {
                ConsoleOutput.Select(ConsoleOutput.Text.Length, 0);
                ConsoleOutput.ScrollToCaret();
            }
            if (ServerGUI.ConsoleWriteWait.Count() >= 1)
            {
                ServerGUI.ConsoleWriteWait[0].Set(); //Let the next console handler write.
            }
        }

        public static void ConsoleHandler(String ThisMessage)
        {
            AutoResetEvent ReadyToResume = new AutoResetEvent(false);
            Logger.Console.ConsoleWriteWait.Add(ReadyToResume);
            while (Logger.Console.ConsoleWriteWait.Count() > 1)
            {
                if (!(Logger.Console.ConsoleWriteWait.Contains(ReadyToResume))) Logger.Console.ConsoleWriteWait.Add(ReadyToResume);
                ReadyToResume.WaitOne(1000);
                Logger.Console.ConsoleWriteWait.RemoveAll(x => x == ReadyToResume);
            }
            if (ThisMessage.StartsWith("\r"))
            {
                try
                {
                    System.Console.SetCursorPosition(0, System.Console.CursorTop - 1);
                }
                catch
                {
                }
                ThisMessage = ThisMessage.Remove(0, 1); // "\r" is ONE character, not two.
            }
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write("\r" + new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, System.Console.CursorTop -1);
            //System.Console.WriteLine("1");
            foreach (string msgToAppend in Converter(ThisMessage))
            {
                switch (msgToAppend.ToLower()[1])
                {
                    case '0':
                        System.Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case '1':
                        System.Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;
                    case '2':
                        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case '3':
                        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case 'y':
                    case '4':
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case 'm':
                    case 'i':
                    case '5':
                        System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;
                    case '6':
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case '7':
                        System.Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case '8':
                        System.Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                    case '9':
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case 'h':
                    case 'a':
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 'p':
                    case 'b':
                        System.Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case 'w':
                    case 'c':
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case 'd':
                        System.Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case 's':
                    case 'r':
                    case 'e':
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        System.Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                System.Console.Write(msgToAppend.Remove(0, 2));
            }
            //System.Console.WriteLine("2");
            Logger.Console.ConsoleWriteWait.RemoveAll(x => x == ReadyToResume);
            if (Logger.Console.ConsoleWriteWait.Count() >= 1)
            {
                try
                {
                    Logger.Console.ConsoleWriteWait[0].Set(); //Let the next console handler write.
                }
                catch
                {
                }
            }
        }

        public static string StripColors(string input ) {
            if (input == null) return "";
            if( input.IndexOf( '&' ) == -1 ) {
                return input;
            } else {
                StringBuilder output = new StringBuilder( input.Length );
                for( int i = 0; i < input.Length; i++ ) {
                    if( input[i] == '&' ) {
                        if( i == input.Length - 1 ) {
                            break;
                        }
                        i++;
                        if( input[i] == 'n' || input[i] == 'N' ) {
                            output.Append( '\n' );
                        }
                    } else {
                        output.Append( input[i] );
                    }
                }
                return output.ToString();
            }
        }
        
    }
}
