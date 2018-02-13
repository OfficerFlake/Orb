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
        public static readonly CommandDescriptor Orb_Command_Moderation_User_UnBan = new CommandDescriptor
        {
            _Name = "UnBan",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "UnBans a user from the server.",
            _Usage = "Usage: /User.<Name>.UnBan <Reason>",
            _Commands = new string[] { "/User.*.UnBan" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Moderation_User_UnBan_Method,
        };

        public static bool Orb_Command_Moderation_User_UnBan_Method(Server.NetObject NetObj, CommandReader Command)
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
            #region GetUnBanReason
            if (Command._CmdArguments.Count() < 1)
            {
                //Ban reason not given.
            }
            else
            {
                Reason = Command._CmdArguments[0];
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject.Can(Database.PermissionDB.Strings.Ban))
            {
                //continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("You do not have permission to unban users from the server.");
                return false;
            }
            #endregion

            if (TargetUser == NetObj.UserObject)
            {
                NetObj.ClientObject.SendMessage("You are not able to unban yourself!");
                return false;
            }
            if (!TargetUser.Banned)
            {
                NetObj.ClientObject.SendMessage("User: \"" + TargetUser.Name + "\" is not banned.");
                return false;
            }
            TargetUser.BanExpires = new DateTime();
            TargetUser.Banned = false;
            TargetUser.BannedBy = NetObj.UserObject;
            TargetUser.BanReason = Reason;
            TargetUser.SaveAll();
            Server.EmptyClientList.Include(TargetUser).SendMessage("&eYou have been &aUNBANNED&e by \"" + NetObj.UserObject.Name + "\".");
            Server.AllClients.Except(TargetUser).SendMessage("&eUser: \"" + TargetUser.Name + "\" was &aUNBANNED&e by \"" + NetObj.UserObject.Name + "\".");
            if (TargetUser.BanReason != "") Server.AllClients.SendMessage("&aUnBan &eReason: \"" + TargetUser.BanReason + "\".");
            return true;
        }
    }
}