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
        public static readonly CommandDescriptor Orb_Command_Chat_Spam = new CommandDescriptor
        {
            _Name = "Spam",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Spams the server with a /Say style chat message 10 times.",
            _Usage = "Usage: /Spam <Message>",
            _Commands = new string[] { "/Spam" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Chat_Spam_Method,
        };

        public static bool Orb_Command_Chat_Spam_Method(Server.NetObject NetObj, CommandReader Command)
        {
            #region Spam
            if (NetObj.UserObject.Muted)
            {
                NetObj.UserObject.MuteNotifier();
                return false;
            }
            if (NetObj.UserObject.Can(Database.PermissionDB.Strings.Say))
            {
                Network.Packets.Type32_ChatMessage SpamThis = new Network.Packets.Type32_ChatMessage();
                SpamThis.Message = Command._CmdRawArguments;
                Network.Packet ThisPacket = SpamThis.Serialise();
                Logger.Console.WriteLine(NetObj.UserObject.DisplayedName + "&b(Spam)&f: " + Command._CmdRawArguments);
                for (int i = 0; i < 10; i++)
                {
                    Server.AllClients.Except(Server.OrbConsole).Send(ThisPacket);
                }
                return true;
            }
            else
            {
                NetObj.ClientObject.SendMessage("You do not have enough permission to \"Spam\".");
                return false;
            }
            #endregion
        }
    }
}