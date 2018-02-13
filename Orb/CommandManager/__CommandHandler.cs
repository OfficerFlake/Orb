using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Commands
    {
        public delegate bool CommandHandler(Server.NetObject source, CommandReader cmd); //return true if command worked, false if it failed.
        public static List<CommandDescriptor> List = new List<CommandDescriptor>();

        public sealed class CommandDescriptor
        {
            public string _Name = null;
            public double _Version = 0;
            public DateTime _Date = new DateTime();
            public string _Author = null;
            public string _Descrption = null;
            public string _Usage = null;
            public string[] _Commands = null; //multiple commands, supporting aliases.
            public CommandHandler _Handler = null;
        }

        public static string[] CmdWildCards = { "*" };
        public static string[] CmdAliasesReserver = { "/Help" };

        public static bool Register(CommandDescriptor ThisCmdDescriptor)
        {
            //This is called WHEN initialised, NOT automatically! hooray for static class huh? :/
            //if (_Name == null | _Version == 0 | _Date == null | _Author == null | _Descrption == null | _Usage == null | _Commands == null | _Handler == null)
            //{
            //    return false;
            //}
            foreach (var ThisFieldVar in ThisCmdDescriptor.GetType().GetFields(System.Reflection.BindingFlags.Public))
            {
                if (ThisFieldVar.GetValue(ThisCmdDescriptor) == null)
                {
                    Logger.Console.WriteLine("&cERROR: &eFailure to register command: \"" + ThisCmdDescriptor._Name + "\". The Field " + ThisFieldVar.Name + " is left Null.");
                    return false;
                }
            }
            foreach (string ThisString in ThisCmdDescriptor._Commands)
            {
                if(ThisString.StartsWith("//") | ThisString.StartsWith("@") | ThisString.StartsWith("@@") | ThisString.EndsWith("."))
                {
                    Logger.Console.WriteLine("&cERROR: &eFailure to register command: \"" + ThisCmdDescriptor._Name + "\". There is an alias that uses illegal characters, that would conflict with the way Orb handles user chat (for example, stating with \"@\").");
                    return false;
                }
                if(!ThisString.StartsWith("/"))
                {
                    Logger.Console.WriteLine("&cERROR: &eFailure to register command: \"" + ThisCmdDescriptor._Name + "\". There is an alias that does not start with the Command designator character: \"/\".");
                    return false;
                }
            }

            if (Commands.List.Select(x => x._Name).Contains(ThisCmdDescriptor._Name))
            {
                foreach (double ThisDouble in Commands.List.Where(x => x._Name == ThisCmdDescriptor._Name).Select(y => y._Version))
                {
                    if (ThisDouble > ThisCmdDescriptor._Version)
                    {
                        Logger.Console.WriteLine("&cERROR: &eFailure to register command: \"" + ThisCmdDescriptor._Name + "\". There is already a command by this name and it is of a newer version.");
                        return false;
                    }
                }
                Logger.Console.WriteLine("&aSucessfully updated registered command: \"" + ThisCmdDescriptor._Name + "\".");
            }
            //Logger.Console.WriteLine("&eSucessfully registered command: \"" + ThisCmdDescriptor._Name + "\".");
            Commands.List.RemoveAll(x => x._Name == ThisCmdDescriptor._Name);
            Commands.List.Add(ThisCmdDescriptor);
            return true;
        }

        public static List<CommandDescriptor> FindCommand(string Cmd)
        {
            List<Commands.CommandDescriptor> MatchingCommands = new List<Commands.CommandDescriptor>();
            #region GetMatchingCommands
            foreach (Commands.CommandDescriptor ThisCmd in Commands.List)
            {
                //get all the command descriptors, and find matches one by one.
                foreach (string CmdString in ThisCmd._Commands)
                {
                    //get all the aliases for this indiviual cmd, and check if it's length == length of command typed.
                    if (CmdString.Split('.').Count() != Cmd.Split('.').Count()) continue;
                    else
                    {
                        //iterate over each cmd part, find matches.
                        //USE THE "FOR" LUKE
                        bool CmdMatches = true;
                        for (int i = 0; i <= (CmdString.Split('.').Count() - 1); i++)
                        {
                            if (!(CmdString.Split('.')[i].ToUpperInvariant() == Cmd.Split('.')[i].ToUpperInvariant() || CmdWildCards.Contains(CmdString.Split('.')[i].ToUpperInvariant())))
                            {
                                CmdMatches = false;
                                break;
                            }
                        }
                        if (CmdMatches)
                        {
                            if (!(MatchingCommands.Contains(ThisCmd))) MatchingCommands.Add(ThisCmd);
                        }
                    }
                }
            }
            #endregion

            return MatchingCommands;
        }

        public static bool OneMatchingCommand(this List<CommandDescriptor> MatchingCommands)
        {
            if (MatchingCommands.Count() < 1)
            {
                return false;
            }
            if (MatchingCommands.Count() == 1)
            {
                return true;
            }
            if (MatchingCommands.Count() > 1)
            {
                return false;
            }
            return false;
        }

        public class CommandReader {
            public string _CmdComplete = null;
            public string _CmdString
            {
                get
                {
                    return _CmdComplete.Split(new string[] { " " }, 2, StringSplitOptions.None)[0];
                }
                set
                {
                    try
                    {
                        _CmdComplete = value + " " + _CmdComplete.Split(new string[] { " " }, 2, StringSplitOptions.None)[1];
                    }
                    catch
                    {
                        _CmdComplete = value;
                    }
                }
            }
            public string _CmdRawArguments
            {
                get
                {
                    try
                    {
                        return _CmdComplete.Split(new string[] { " " }, 2, StringSplitOptions.None)[1];
                    }
                    catch
                    {
                        return null;
                    }
                }
                set
                {
                    _CmdComplete = _CmdComplete.Split(new string[] { " " }, 2, StringSplitOptions.None)[0] + " " + value;
                }
            }
            public string[] _CmdArguments
            {
                get
                {
                    try
                    {
                        return _CmdRawArguments.Split(' ');
                    }
                    catch
                    {
                        return new string[] {};
                    }
                }
            }

            public int _CmdPosition = 0;
            public string _CmdCurrent()
            {
                if ((_CmdString.Replace(',', '.').Split('.').Count() - 1) < _CmdPosition || _CmdPosition < 0)
                {
                    return "NULL";
                }
                else
                {
                    return _CmdString.Replace(',', '.').Split('.')[_CmdPosition];
                }
            }
            public string _CmdPrev()
            {
                if ((_CmdString.Replace(',', '.').Split('.').Count() - 2) < _CmdPosition - 1 || _CmdPosition - 1 < 0)
                {
                    return "NULL";
                }
                else
                {
                    return _CmdString.Replace(',', '.').Split('.')[_CmdPosition - 1];
                }
            }
            public string _CmdNext()
            {
                if ((_CmdString.Replace(',', '.').Split('.').Count()) < _CmdPosition + 1 || _CmdPosition + 1 < 0)
                {
                    return "NULL";
                }
                else
                {
                    return _CmdString.Replace(',', '.').Split('.')[_CmdPosition + 1];
                }
            }
            public string[] _CmdElements()
            {
                return _CmdString.Remove(0, 1).Replace(',', '.').Split('.');
            }

            public void _CmdGotoPrev()
            {
                if ((_CmdString.Replace(',', '.').Split('.').Count() - 2) < _CmdPosition - 1 || _CmdPosition - 1 < 0)
                {
                    //Do Nothing
                }
                else
                {
                    _CmdPosition--;
                }
            }
            public void _CmdGotoNext()
            {
                if ((_CmdString.Replace(',', '.').Split('.').Count()) < _CmdPosition + 1 || _CmdPosition + 1 < 0)
                {
                    //Do Nothing
                }
                else
                {
                    _CmdPosition++;
                }
            }

            public CommandReader(string RawCommand) {
                _CmdComplete = RawCommand;
                if (RawCommand.Split(new string[] { " " }, 2, StringSplitOptions.None).Count() < 2)
                {
                    RawCommand += " ";
                }
            }
        }
    }

    public static partial class CommandManager
    {
        public static void Process(Server.NetObject ThisNetObj, string RawCommand)
        {
            if (RawCommand == "/")
            {
                //Repeat Last Command.
                RawCommand = ThisNetObj.LastTypedCommand;
            }

            ThisNetObj.LastTypedCommand = RawCommand;

            Commands.CommandReader Cmd = new Commands.CommandReader(RawCommand);


            if (Cmd._CmdString.ToUpperInvariant().StartsWith("@@"))
            {
                Cmd._CmdString = Cmd._CmdString.Remove(0, 1);
                Messaging.GroupChatMessage(ThisNetObj, Cmd);
                return;
            }
            if (Cmd._CmdString.ToUpperInvariant().StartsWith("@"))
            {
                Cmd._CmdString = Cmd._CmdString.Remove(0, 1);
                Messaging.PrivateChatMessage(ThisNetObj, Cmd);
                return;
            }
            if (Cmd._CmdString.ToUpperInvariant().StartsWith("//"))
            {
                Cmd._CmdComplete = Cmd._CmdComplete.Remove(0, 1);
                Messaging.StandardChatMessage(ThisNetObj, Cmd);
                return;
            }
            if (Cmd._CmdString.ToUpperInvariant() == "/HELP")
            {
                if (Cmd._CmdArguments.Count() < 1)
                {
                    //show help usage
                    ThisNetObj.ClientObject.SendMessage("Welcome To Orb For YSFlight!");
                    ThisNetObj.ClientObject.SendMessage("    Orb Servers are designed to extend YSFlights Functionality, by use of commands and a database.");
                    ThisNetObj.ClientObject.SendMessage("    For more information on commands available, type \"/Commands\".");
                    return;
                }
                List<Commands.CommandDescriptor> MatchingCommands = Commands.FindCommand("/" + Cmd._CmdArguments[0]);
                if (MatchingCommands.Count() == 0)
                {
                    ThisNetObj.ClientObject.SendMessage("&cCommand not found: \"" + Cmd._CmdArguments[0] + "\".");
                    return;
                }
                if (MatchingCommands.OneMatchingCommand())
                {
                    ThisNetObj.ClientObject.SendMessage("&a" + MatchingCommands[0]._Usage);
                    ThisNetObj.ClientObject.SendMessage("&e" + "    " + MatchingCommands[0]._Descrption);
                    ThisNetObj.ClientObject.SendMessage("&a" + "    Aliases: " + MatchingCommands[0]._Commands.Take(3).ToList().ToStringList());
                }
                else
                {
                    ThisNetObj.ClientObject.SendMessage("&cERROR: &eMore then one matching command found: \"" + Cmd._CmdArguments[0] + "\".");
                }
                return;
            }
            if (Cmd._CmdString.ToUpperInvariant().StartsWith("/")) 
            {
                List<Commands.CommandDescriptor> MatchingCommands = Commands.FindCommand(Cmd._CmdString);
                if (MatchingCommands.Count() == 0)
                {
                    ThisNetObj.ClientObject.SendMessage("&eCommand not found: \"" + Cmd._CmdString + "\".");
                    return;
                }
                if (MatchingCommands.OneMatchingCommand())
                {
                    MatchingCommands[0]._Handler(ThisNetObj, Cmd);
                }
                else
                {
                    ThisNetObj.ClientObject.SendMessage("&cERROR: &eMore then one matching command found: \"" + Cmd._CmdString + "\".");
                }
                return;
            }
            Messaging.StandardChatMessage(ThisNetObj, Cmd);
            return;
            //ThisNetObj.ClientObject.SendMessage("TEST COMPLETE");
        }
    }
}