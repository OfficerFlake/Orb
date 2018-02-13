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
        public static readonly CommandDescriptor Orb_Command_Moderation_User_Mute = new CommandDescriptor
        {
            _Name = "Mute",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Mutes a user on the server.",
            _Usage = "Usage: /User.<Name>.Mute <Duration> <Reason>",
            _Commands = new string[] { "/User.*.Mute" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Moderation_User_Mute_Method,
        };

        public static bool Orb_Command_Moderation_User_Mute_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.UserDB.User TargetUser = Database.UserDB.Nobody;
            DateTime MuteEnds = new DateTime();
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
            #region GetMutexpiry
            if (Command._CmdArguments.Count() < 1)
            {
                //Mute is Permanent by default
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
                    //New Date Time == Permanent Mute.
                    //This is already set, we don't need to do anything.
                }
                else
                {
                    if (!Utilities.DateTimeUtilities.TryParseMiniTimespan(Command._CmdArguments[0], out Duration))
                    {
                        NetObj.ClientObject.SendMessage("Mute Duration Format Invalid. Acceptable Format/s: \"*w*d*h*m*s\" eg: \"1w2d5h3m2s\" or \"7w2m\" or even \"5s\".");
                        return false;
                    }
                    MuteEnds = DateTime.Now + Duration;
                }
            }
            #endregion
            #region GetMuteReason
            if (Command._CmdArguments.Count() < 2)
            {
                //Mute reason not given.
            }
            else
            {
                Reason = Command._CmdRawArguments.Split(new string[] {" "}, 2, StringSplitOptions.None)[1];
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject.Can(Database.PermissionDB.Strings.Mute))
            {
                //continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("You do not have permission to mute users on the server.");
                return false;
            }
            #endregion

            if (DateTime.Now - TimeSpan.FromSeconds(30) > MuteEnds && MuteEnds != new DateTime())
            {
                NetObj.ClientObject.SendMessage("MuteTime is in the past! Unable to mute the target!");
                return false;
            }
            if (TargetUser == NetObj.UserObject)
            {
                NetObj.ClientObject.SendMessage("You are not able to mute yourself!");
                return false;
            }
            if (TargetUser.Muted)
            {
                NetObj.ClientObject.SendMessage("User: \"" + TargetUser.Name + "\" is already muted.");
                return false;
            }
            TargetUser.TimesMuted++;
            TargetUser.MuteExpires = MuteEnds;
            TargetUser.Muted = true;
            TargetUser.MutedBy = NetObj.UserObject;
            TargetUser.MuteReason = Reason;
            TargetUser.SaveAll();
            if (TargetUser.MuteExpires == new DateTime())
            {
                Server.EmptyClientList.Include(TargetUser).SendMessage("&cYou have been PERMANTENTLY MUTED by \"" + NetObj.UserObject.Name + "\".");
                Server.AllClients.Except(TargetUser).SendMessage("&cUser: \"" + TargetUser.Name + "\" was PERMANTENTLY MUTED by \"" + NetObj.UserObject.Name + "\".");
                if (TargetUser.MuteReason != "") Server.AllClients.SendMessage("&cMute Reason: \"" + TargetUser.MuteReason + "\".");
            }
            else
            {
                Server.EmptyClientList.Include(TargetUser).SendMessage("&cYou have been MUTED by \"" + NetObj.UserObject.Name + "\" until " + Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(TargetUser.MuteExpires)) + ".");
                Server.AllClients.Except(TargetUser).SendMessage("&cUser: \"" + TargetUser.Name + "\" was MUTED by \"" + NetObj.UserObject.Name + "\" until " + Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(TargetUser.MuteExpires)) + ".");
                if (TargetUser.MuteReason != "") Server.AllClients.SendMessage("&cMute Reason: \"" + TargetUser.MuteReason + "\".");
            }
            return true;
        }
    }
}