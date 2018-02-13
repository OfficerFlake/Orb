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
        public static readonly CommandDescriptor Orb_Command_Moderation_User_Ban = new CommandDescriptor
        {
            _Name = "Ban",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Bans a user from the server.",
            _Usage = "Usage: /User.<Name>.Ban <Duration> <Reason>",
            _Commands = new string[] { "/User.*.Ban" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Moderation_User_Ban_Method,
        };

        public static bool Orb_Command_Moderation_User_Ban_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.UserDB.User TargetUser = Database.UserDB.Nobody;
            DateTime BanEnds = new DateTime();
            TimeSpan Duration = new TimeSpan();
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
            #region GetBanExpiry
            if (Command._CmdArguments.Count() < 1)
            {
                //Ban is Permanent by default
            }
            else
            {
                if (Command._CmdArguments[0].ToUpperInvariant() == "INF" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "INFINITE" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "INFINITY" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "0" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "-" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "FOREVER" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "PERMANENT" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "PERM" ||
                    Command._CmdArguments[0].ToUpperInvariant() == "!")
                {
                    //New Date Time == Permanent Ban.
                    //This is already set, we don't need to do anything.
                }
                else
                {
                    if (!Utilities.DateTimeUtilities.TryParseMiniTimespan(Command._CmdArguments[0], out Duration))
                    {
                        NetObj.ClientObject.SendMessage("Ban Duration Format Invalid. Acceptable Format/s: \"*w*d*h*m*s\" eg: \"1w2d5h3m2s\" or \"7w2m\" or even \"5s\".");
                        return false;
                    }
                    BanEnds = DateTime.Now + Duration;
                }
            }
            #endregion
            #region GetBanReason
            if (Command._CmdArguments.Count() < 2)
            {
                //Ban reason not given.
            }
            else
            {
                Reason = Command._CmdRawArguments.Split(new string[] {" "}, 2, StringSplitOptions.None)[1];
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject.Can(Database.PermissionDB.Strings.Ban))
            {
                //continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("You do not have permission to ban users from the server.");
                return false;
            }
            #endregion

            if (DateTime.Now - TimeSpan.FromSeconds(30) > BanEnds && BanEnds != new DateTime())
            {
                NetObj.ClientObject.SendMessage("BanTime is in the past! Unable to ban the target!");
                return false;
            }
            if (TargetUser == NetObj.UserObject)
            {
                NetObj.ClientObject.SendMessage("You are not able to ban yourself!");
                return false;
            }
            if (TargetUser.Banned)
            {
                NetObj.ClientObject.SendMessage("User: \"" + TargetUser.Name + "\" is already banned.");
                return false;
            }
            TargetUser.TimesBanned++;
            TargetUser.BanExpires = BanEnds;
            TargetUser.Banned = true;
            TargetUser.BannedBy = NetObj.UserObject;
            TargetUser.BanReason = Reason;
            TargetUser.SaveAll();
            if (TargetUser.BanExpires == new DateTime())
            {
                Server.EmptyClientList.Include(TargetUser).SendMessage("&cYou have been PERMANTENTLY BANNED by \"" + NetObj.UserObject.Name + "\".");
                Server.AllClients.Except(TargetUser).SendMessage("&cUser: \"" + TargetUser.Name + "\" was PERMANTENTLY BANNED by \"" + NetObj.UserObject.Name + "\".");
                if (TargetUser.BanReason != "") Server.AllClients.SendMessage("&cBan Reason: \"" + TargetUser.BanReason + "\".");
            }
            else
            {
                Server.EmptyClientList.Include(TargetUser).SendMessage("&cYou have been BANNED by \"" + NetObj.UserObject.Name + "\" until " + Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(TargetUser.BanExpires)) + ".");
                Server.AllClients.Except(TargetUser).SendMessage("&cUser: \"" + TargetUser.Name + "\" was BANNED by \"" + NetObj.UserObject.Name + "\" until " + Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(TargetUser.BanExpires)) + ".");
                if (TargetUser.BanReason != "") Server.AllClients.SendMessage("&cBan Reason: \"" + TargetUser.BanReason + "\".");
            }
            foreach (Server.NetObject ThisClient in Server.ClientList.Where(x => x.UserObject == TargetUser).ToArray())
            {
                ThisClient.Close(); //Kicks the target user.
            }
            return true;
        }
    }
}