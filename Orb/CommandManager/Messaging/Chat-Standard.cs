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
        public static void StandardChatMessage(Server.NetObject ThisNetObj, Commands.CommandReader Cmd)
        {
            if (ThisNetObj.TextWaiters.Count > 0)
            {
                ThisNetObj.TextInput = Cmd._CmdComplete;
                foreach (AutoResetEvent ThisEvent in ThisNetObj.TextWaiters.ToArray())
                {
                    ThisEvent.Set();
                    ThisNetObj.TextWaiters.RemoveAll(x => x == ThisEvent);
                }
                return;
            }
            string output = FormatChatMessage(Cmd._CmdComplete, ThisNetObj);
            Server.AllClients.Except(Database.UserDB.Find("PHP bot")).SendMessage(output);
            //ThisNetObj.HostObject.SendMessage(output);
            Logger.Log.Chat(output);
            return;
        }
    }
}