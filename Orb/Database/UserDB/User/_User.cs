using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Orb
{
    public static partial class Database
    {
        public static partial class UserDB
        {
            public partial class User
            {
                #region Variables
                //Name
                public string Name = "";
                public string DisplayedName = "";

                public string Password = "";
                public bool UsePassword = false;

                public IPAddress LastIP         = IPAddress.Any;
                public int LoginCount           = 0;
                public int MessagesTyped        = 0;
                public DateTime DateJoined      = DateTime.Now;
                public DateTime DateLastVisited = DateTime.Now;
                public TimeSpan PlayTime        = TimeSpan.Zero; //We can grab values like "TotalHours" with this, most interchangable.
                public int Kills                = 0;
                public int Deaths               = 0;
                public int FlightsFlown         = 0;
                public TimeSpan FlightHours     = TimeSpan.Zero;

                public bool Banned              = false;
                public User BannedBy            = UserDB.Nobody;
                public DateTime DateBanned      = new DateTime();
                public DateTime BanExpires      = new DateTime();
                public string BanReason         = "";
                public int TimesBanned          = 0;

                public bool Frozen;
                public User FrozenBy            = UserDB.Nobody;
                public DateTime DateFrozen      = new DateTime();
                public DateTime FreezeExpires   = new DateTime();
                public string FreezeReason      = "";
                public int TimesFrozen          = 0;

                public bool Muted               = false;
                public User MutedBy             = UserDB.Nobody;
                public DateTime DateMuted       = new DateTime();
                public DateTime MuteExpires     = new DateTime();
                public string MuteReason        = "";
                public int TimesMuted           = 0;

                public bool Kicked              = false; //Should never have a purpose.
                public User KickedBy            = UserDB.Nobody;
                public DateTime DateKicked      = new DateTime();
                public DateTime KickExpires     = new DateTime();
                public string KickReason        = "";
                public int TimesKicked          = 0;

                public int OthersBanned         = 0;
                public int OthersFrozen         = 0;
                public int OthersMuted          = 0;
                public int OthersKicked         = 0;

                public GroupDB.Group GroupRepresented  = GroupDB.NoGroup; //Need to replace this with a Group Object.

                public List<GroupReference> Groups = new List<GroupReference>();

                public bool IsUtilityUser()
                {
                    if (this == SuperUser) return true;
                    if (this == Nobody) return true;
                    return false;
                }

                //User specific Permissions object.
                public Database.PermissionDB.Permission Permissions = new Database.PermissionDB.Permission();
                #endregion

                public bool Can(string Key) {
                    return PermissionDB.PermissionsCore.CheckPermission(this, Key);
                }

                public bool CanNot(string Key)
                {
                    return !PermissionDB.PermissionsCore.CheckPermission(this, Key);
                }

                public bool IsMuted {
                    get
                    {
                        if (Muted)
                        {
                            if (MuteExpires == new DateTime()) return true;
                            else if (MuteExpires < DateTime.Now)
                            {
                                Server.ClientList.Where(x => x.UserObject == this).ToList().SendMessage("Your Mute has expired, and you are able to talk again.");
                                Logger.Console.WriteLine("&cUSER \"" + Name + "\" WAS UNMUTED AS THEIR MUTE EXPIRY HAS PASSED!");
                                Logger.Console.WriteLine("&c    " + MuteExpires.ToString() + " < " + DateTime.Now.ToString());
                                Muted = false;
                                SaveAll();
                                return false;
                            }
                            else
                            {
                                Logger.Log.Bug("Unknown Error in \"Is Muted\" test for User: \"" + Name + "\". Returned True as a safeguard.");
                                return true;
                            }
                        }
                        return false;
                    }
                }

                public bool IsBanned
                {
                    get
                    {
                        if (Banned)
                        {
                            if (BanExpires == new DateTime()) return true;
                            else if (BanExpires < DateTime.Now)
                            {
                                Server.ClientList.Where(x => x.UserObject == this).ToList().SendMessage("Your Ban has expired, and you are able to connect again.");
                                    //^^^ This is a bit weird, as all users should be disconnected if they are banned. Nonetheless, we send it just incase...
                                Logger.Console.WriteLine("&cUSER \"" + Name + "\" WAS UNBANNED AS THEIR BAN EXPIRY HAS PASSED!");
                                Logger.Console.WriteLine("&c    " + BanExpires.ToString() + " < " + DateTime.Now.ToString());
                                Banned = false;
                                SaveAll();
                                return false;
                            }
                            else
                            {
                                Logger.Log.Bug("Unknown Error in \"Is Banned\" test for User: \"" + Name + "\". Returned True as a safeguard.");
                                return true;
                            }
                        }
                        return false;
                    }
                }

                public void MuteNotifier()
                {
                    if (IsMuted)
                    {
                        #region EchoMessages
                        string[] EchoMessages = {   "Your words echo far, but nothing happens.",
                                                    "Your words echo far, but nobody hears you.",
                                                    "Your words echo far, but nobody cares.",
                                                    "Your words disappear, like tears in the rain.",
                                                    "You go to speak, but no sound comes out.",
                                                    "OAK: Now is not the time to talk.",
                                                    Name + " used \"Mute Talk!\"... It's not very effective.",
                                                    "Nuh uh uh! You didn't say the magic word!",
                                                    "\"If I could talk, I could be some sort of company for myself...\"",
                                                    "S...O...S... Help... Me...",
                                                    "Every action has a consequence, " + Name + ", You may want to check your mute reason.",
                                                    "Keep trying to talk. I'm sure someone will hear you eventually.",
                                                    "If a Muted Player talks on an empty server, do they still not make noise?",
                                                    "SHH! This is a Library!",
                                                    "SHH! This is a Tennis Match!",
                                                    "SHH! This is a Nickleback Concert!",
                                                    "SILENCE EARTHLING!",
                                                    "Most people stop trying to talk by now. You are something special, " + Name + "... Something VERY \"Special\".",
                                                    "OOO! I think someone MAY have heard that one! DO IT AGAIN!",
                                                    Name + ", Wat R U Doin. " + Name + ", STAPH!",
                                                    "If you scream when you muted, no-one can hear you. It's like space really, without the weightlessness.",
                                                    "Error sending message: Mayonaise low",
                                                    "Error sending message: Insufficient Credits",
                                                    "Error sending message: Please insert cat to continue.",
                                                    "Error sending message: Spoon not found.",
                                                    "Error sending message: The Monkey that lives in the program ate it.",
                                                    "Error sending message: Not enough bananas in stock.",
                                                    "Error sending message: Couldn't go far even as decided to use go want to do look more like.",
                                                    "It was the best of messages, it was the blurst of messages.",
                                                    "I'd like to send that message for you, but FSDFSJ.",
                                                    "You message failed to send: you did not attach a post stamp, it was rejected by the post service.",
                                                    "Don't forget, you are muted FOREVER.",
                                                    "Arnold Schwarzenegger: \"SHADDDDDDDAAAAAAAAAAAAAP!!!!!\"",
                                                };
                        #endregion
                        Random ThisRandom = new Random();
                        Server.ClientList.Where(x => x.UserObject == this).ToList().SendMessage(EchoMessages[ThisRandom.Next(EchoMessages.Length)]);
                    }
                }
            }
            public static class Strings
            {
                #region Strings
                //Name
                public const string Name = "USERNAME";
                public const string DisplayedName = "DISPLAYEDNAME";

                public const string LastIP = "LASTIP";
                public const string LoginCount = "LOGINCOUNT";
                public const string MessagesTyped = "MESSAGESTYPED";
                public const string DateJoined = "DATEJOINED";
                public const string DateLastVisited = "DATELASTVISITED";
                public const string PlayTime = "PLAYTIME";
                public const string Kills = "KILLS";
                public const string Deaths = "DEATHS";
                public const string FlightsFlown = "FLIGHTSFLOWN";
                public const string FlightHours = "FLIGHTHOURS";

                public const string Banned = "BANNED";
                public const string BannedBy = "BANNEDBY";
                public const string DateBanned = "DATEBANNED";
                public const string BanExpires = "BANEXPIRES";
                public const string BanReason = "BANREASON";
                public const string TimesBanned = "TIMESBANNED";

                public const string Frozen = "FROZEN";
                public const string FrozenBy = "FROZENBY";
                public const string DateFrozen = "DATEFROZEN";
                public const string FreezeExpires = "FREEZEEXPIRES";
                public const string FreezeReason = "FREEZEREASON";
                public const string TimesFrozen = "TIMESFROZEN";

                public const string Muted = "MUTED";
                public const string MutedBy = "MUTEDBY";
                public const string DateMuted = "DATEMUTED";
                public const string MuteExpires = "MUTEEXPIRES";
                public const string MuteReason = "MUTEREASON";
                public const string TimesMuted = "TIMESMUTED";

                public const string Kicked = "KICKED"; //Will Never have a use?
                public const string KickedBy = "KICKEDBY";
                public const string DateKicked = "DATEKICKED";
                public const string KickExpires = "KICKEXPIRES"; //Will Never have a use?
                public const string KickReason = "KICKREASON";
                public const string TimesKicked = "TIMESKICKED";

                public const string OthersBanned = "OTHERSBANNED";
                public const string OthersFrozen = "OTHERSFROZEN";
                public const string OthersMuted = "OTHERSMUTED";
                public const string OthersKicked = "OTHERSKICKED";

                public const string Group = "GROUP";
                public const string GroupRepresented = "GROUPREPRESENTED";

                public const string UsePassword = "USEPASS";
                public const string Password = "PASSWORD";
                #endregion
            }
        }
    }
}
