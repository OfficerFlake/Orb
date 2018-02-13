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
        public static readonly CommandDescriptor Orb_Command_Moderation_User_Kick = new CommandDescriptor
        {
            _Name = "Kick",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Kick a user from the server.",
            _Usage = "Usage: /User.<Name>.Kick <Reason>",
            _Commands = new string[] { "/User.*.Kick" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Moderation_User_Kick_Method,
        };

        public static bool Orb_Command_Moderation_User_Kick_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.UserDB.User TargetUser = Database.UserDB.Nobody;
            string Reason = "";
            #region FindTargetUser
            if (Command._CmdElements()[1] == "-")
            {
                if (NetObj.CommandHandling.PreviousUser == Database.UserDB.Nobody)
                {
                    NetObj.ClientObject.SendMessage("No previous users iterated over.");
                    return false;
                }
                else
                {
                    TargetUser = NetObj.CommandHandling.PreviousUser;
                }
            }
            else
            {
                if (Database.UserDB.Find(Command._CmdElements()[1]) != Database.UserDB.Nobody)
                {
                    TargetUser = Database.UserDB.Find(Command._CmdElements()[1]);
                }
                else
                {
                    NetObj.ClientObject.SendMessage("User not found: \"" + Command._CmdElements()[1] + "\".");
                    return false;
                }
            }
            #endregion
            #region GetKickReason
            if (Command._CmdArguments.Count() < 1)
            {
                //Ban reason not given.
            }
            else
            {
                Reason = Command._CmdRawArguments;
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject.Can(Database.PermissionDB.Strings.Kick))
            {
                //continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("You do not have permission to kick users from the server.");
                return false;
            }
            #endregion

            if (TargetUser == NetObj.UserObject)
            {
                NetObj.ClientObject.SendMessage("You are not able to kick yourself!");
                return false;
            }
            if (!Server.ClientList.Select(x => x.UserObject).Contains(TargetUser))
            {
                NetObj.ClientObject.SendMessage("User: \"" + TargetUser.Name + "\" is not online.");
                return false;
            }
            TargetUser.TimesKicked++;
            TargetUser.KickedBy = NetObj.UserObject;
            TargetUser.KickReason = Reason;
            TargetUser.SaveAll();
            Server.EmptyClientList.Include(TargetUser).SendMessage("&cYou have been KICKED by \"" + NetObj.UserObject.Name + "\".");
            Server.AllClients.Except(TargetUser).SendMessage("&cUser: \"" + TargetUser.Name + "\" was KICKED by \"" + NetObj.UserObject.Name + "\".");
            if (TargetUser.KickReason != "") Server.AllClients.SendMessage("&cKick Reason: \"" + TargetUser.KickReason + "\".");
            foreach (Server.NetObject ThisClient in Server.ClientList.Where(x => x.UserObject == TargetUser).ToArray())
            {
                ThisClient.Close(); //Kicks the target user.
            }
            return true;
        }
    }
}