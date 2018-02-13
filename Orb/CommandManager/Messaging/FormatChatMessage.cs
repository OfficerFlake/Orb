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
        public static string FormatChatMessage(string Command, Server.NetObject ThisNetObj)
        {
            if (ThisNetObj.UserObject.GroupRepresented != Database.GroupDB.NoGroup)
            {
                string output = "";
                foreach (char ThisChar in ThisNetObj.UserObject.GroupRepresented.RepresentationFormat.ToUpperInvariant())
                {
                    switch (ThisChar)
                    {
                        case 'G':
                            output += "&5[" + ThisNetObj.UserObject.GroupRepresented.Name + "&5]";
                            break;
                        case 'U':
                            output += "&8" + ThisNetObj.UserObject.DisplayedName;
                            break;
                        case 'R':
                            try
                            {
                                if (ThisNetObj.UserObject.Groups.First(x => x.Group == ThisNetObj.UserObject.GroupRepresented).Rank == Database.GroupDB.NoRank)
                                {
                                    continue;
                                }
                                output += "&9[" + ThisNetObj.UserObject.Groups.First(x => x.Group == ThisNetObj.UserObject.GroupRepresented).Rank.Name + "&9]";
                            }
                            catch
                            {
                            }
                            break;
                        default:
                            output += ThisChar;
                            break;
                    }
                }
                return output + "&f: " + Command;
            }
            else
            {
                return ThisNetObj.UserObject.DisplayedName + "&f: " + Command;
            }
        }
    }
}