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
        public static void PrivateChatMessage(Server.NetObject ThisNetObj, Commands.CommandReader Cmd)
        {
            foreach (string ThisCommandElement in Cmd._CmdElements())
            {
                if (Database.UserDB.Find(ThisCommandElement) != Database.UserDB.Nobody)
                {
                    if (ThisNetObj.UserObject == Database.UserDB.Find(ThisCommandElement))
                    {
                        ThisNetObj.ClientObject.SendMessage("&eYou can't PM yourself!");
                        continue;
                    }
                    Server.EmptyClientList.Include(Database.UserDB.Find(ThisCommandElement)).Except(Database.UserDB.Find("PHP bot")).Except(ThisNetObj).SendMessage("&bPM From " + ThisNetObj.UserObject.Name + ": " + Cmd._CmdRawArguments);
                    ThisNetObj.ClientObject.SendMessage("&bPM To " + Database.UserDB.Find(ThisCommandElement).Name + ": " + Cmd._CmdRawArguments);
                }
                else
                {
                    ThisNetObj.ClientObject.SendMessage("&eUser not found: \"" + ThisCommandElement + "\".");
                }
            }
            return;
        }
    }
}