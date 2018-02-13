using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Messaging
    {
        public static void GroupChatMessage(Server.NetObject ThisNetObj, Commands.CommandReader Cmd)
        {
            foreach (string ThisCommandElement in Cmd._CmdElements())
            {
                if (Database.GroupDB.FindGroup(ThisCommandElement) != Database.GroupDB.NoGroup)
                {
                    Server.EmptyClientList.Include(Database.GroupDB.FindGroup(ThisCommandElement)).Except(Database.UserDB.Find("PHP bot")).Except(ThisNetObj).SendMessage("(" + Database.GroupDB.FindGroup(ThisCommandElement).Name + ")" + ThisNetObj.UserObject.Name + ": " + Cmd._CmdRawArguments);
                    ThisNetObj.ClientObject.SendMessage("(" + Database.GroupDB.FindGroup(ThisCommandElement).Name + ")" + ThisNetObj.UserObject.Name + ": " + Cmd._CmdRawArguments);
                }
                else
                {
                    ThisNetObj.ClientObject.SendMessage("&eGroup not found: \"" + ThisCommandElement + "\".");
                }
            }
            return;
        }
    }
}