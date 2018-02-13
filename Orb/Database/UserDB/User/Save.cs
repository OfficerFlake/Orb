using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Orb
{
    public static partial class Database
    {
        public static partial class UserDB
        {
            public partial class User
            {
                public void SaveAll()
                {
                    string OutputFile = "./Database/Users/" + Name + "/Info.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Users/");
                    Utilities.IO.PrepareDirectory("./Database/Users/" + Name);
                    Utilities.IO.PrepareFile(OutputFile);
                    string OutTime = Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(DateTime.Now));
                    List<String> Output = new List<String>();
                    Output.Add("REM AutoCreated by Orb Database Engine, " + OutTime + ".");

                    foreach (var ThisData in this.GetType().GetFields().ToList())
                    {
                        try
                        {
                            string OutString = "";
                            Type temp = ThisData.FieldType;
                            if (temp == typeof(Database.GroupDB.Group)) OutString = GroupDB._List.First(x => x == ThisData.GetValue(this)).Name;
                            else if (temp == typeof(Database.UserDB.User)) OutString = UserDB._List.First(x => x == ThisData.GetValue(this)).Name;
                            else if (temp == typeof(List<GroupReference>))
                            {
                                foreach (Database.UserDB.User.GroupReference ThisGroupReference in this.Groups)
                                {
                                    Output.Add("GROUP".SuffixTabs(4) + ThisGroupReference.Group.Name);
                                    //Logger.Console.WriteLine("RANK".SuffixTabs(4) + ThisRank.Name);
                                    ThisGroupReference.SaveAll();
                                }
                                continue;
                            }
                            else if (temp == typeof(Database.GroupDB.Group.Rank)) continue;
                            else if (temp == typeof(Database.PermissionDB.Permission)) continue; //Saved Later.
                            else if (temp == typeof(DateTime)) OutString = ((DateTime)(ThisData.GetValue(this))).ToCommonString();
                            else if (temp == typeof(TimeSpan)) OutString = ThisData.GetValue(this).ToString();
                            else
                            {
                                OutString = ThisData.GetValue(this).ToString();
                            }
                            //Logger.Console.WriteLine(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                            Output.Add(ThisData.Name.ToString().SuffixTabs(4) + OutString);
                        }
                        catch (Exception e)
                        {
                            Logger.Console.WriteLine("&cError&e " + e);
                            Logger.Console.WriteLine("&eError Saving Value for User: \"" + Name + "\" Value: \"" + ThisData.Name + "\".");
                        }
                    }
                    File.WriteAllLines(OutputFile, Output.ToList());
                    //Save All Permissions for this user.
                    //  As permissions in it's own right is a suboject,
                    //  Capabable of it's own specific Save() one var only,
                    //  we can do that instead if a specific permission needs to be saved.
                    OutputFile = "./Database/Users/" + Name + "/Permissions.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Users/");
                    Utilities.IO.PrepareDirectory("./Database/Users/" + Name);
                    Utilities.IO.PrepareFile(OutputFile);
                    this.Permissions.SaveAll(OutputFile);
                }
                public void Save(string Key)
                {
                    bool found = false;
                    string OutputFile = "./Database/Users/" + Name + "/Info.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Users/");
                    Utilities.IO.PrepareDirectory("./Database/Users/" + Name);
                    Utilities.IO.PrepareFile(OutputFile);
                    string[] OutputFileContents = Utilities.IO.ReadAllLines(OutputFile);
                    string OutTime = Utilities.DateTimeUtilities.ToYearTimeDescending(Utilities.DateTimeUtilities.FormatDateTime(DateTime.Now));
                    List<String> Output = new List<String>();
                    foreach (string line in OutputFileContents)
                    {
                        #region GetDataCouplets
                        string ThisLine = line;
                        if (!(ThisLine.Contains("\t")))
                        {
                            Output.Add(line);
                            continue;
                        }
                        while (ThisLine.Contains("\t\t"))
                        {
                            ThisLine = ThisLine.Replace("\t\t", "\t");
                        }
                        string Header = ThisLine.Split('\t')[0];
                        string Data = ThisLine.Remove(0, ((Header.Length) + 1));
                        var Converted = Utilities.IO.StringToVariable(Data);
                        //NOTE: These WILL Bypass GUI mode if enabled, as the GUI setting is turned on until it is read.
                        //if (Converted is Boolean) Logger.Console.WriteLine(String.Format("Bool: {0}", Data));
                        //if (Converted is IPAddress) Logger.Console.WriteLine(String.Format("IPAddress: {0}", Data));
                        //if (Converted is Decimal) Logger.Console.WriteLine(String.Format("Decimal: {0}", Data));
                        //if (Converted is Int32) Logger.Console.WriteLine(String.Format("Int32: {0}", Data));
                        //if (Converted is String) Logger.Console.WriteLine(String.Format("String: {0}", Data));
                        #endregion
                        if (Key.ToUpperInvariant() != Header.ToUpperInvariant())
                        {
                            Output.Add(line);
                            continue;
                        }
                        switch (Header.ToUpperInvariant())
                        {
                            #region Switches
                            case Strings.Name:
                                Output.Add(Strings.Name + "\t\t" + Name);
                                found = true;
                                break;
                            case Strings.DisplayedName:
                                Output.Add(Strings.DisplayedName + "\t\t" + DisplayedName.ToString());
                                found = true;
                                break;
                            case Strings.LastIP:
                                Output.Add(Strings.LastIP + "\t\t\t" + LastIP.ToString());
                                found = true;
                                break;
                            case Strings.LoginCount:
                                Output.Add(Strings.LoginCount + "\t\t" + LoginCount.ToString());
                                found = true;
                                break;
                            case Strings.MessagesTyped:
                                Output.Add(Strings.MessagesTyped + "\t\t" + MessagesTyped.ToString());
                                found = true;
                                break;
                            case Strings.DateJoined:
                                Output.Add(Strings.DateJoined + "\t\t" + DateJoined.ToString());
                                found = true;
                                break;
                            case Strings.DateLastVisited:
                                Output.Add(Strings.DateLastVisited + "\t\t" + DateLastVisited.ToString());
                                found = true;
                                break;
                            case Strings.PlayTime:
                                Output.Add(Strings.PlayTime + "\t\t" + PlayTime.ToString());
                                found = true;
                                break;
                            case Strings.Kills:
                                Output.Add(Strings.Kills + "\t\t\t" + Kills.ToString());
                                found = true;
                                break;
                            case Strings.Deaths:
                                Output.Add(Strings.Deaths + "\t\t\t" + Deaths.ToString());
                                found = true;
                                break;
                            case Strings.FlightsFlown:
                                Output.Add(Strings.FlightsFlown + "\t\t" + FlightsFlown.ToString());
                                found = true;
                                break;
                            case Strings.FlightHours:
                                Output.Add(Strings.FlightHours + "\t\t" + FlightHours.ToString());
                                found = true;
                                break;
                            case Strings.Banned:
                                Output.Add(Strings.Banned + "\t\t\t" + Banned.ToString());
                                found = true;
                                break;
                            case Strings.BannedBy:
                                Output.Add(Strings.BannedBy + "\t\t" + BannedBy.ToString());
                                found = true;
                                break;
                            case Strings.DateBanned:
                                Output.Add(Strings.DateBanned + "\t\t" + DateBanned.ToString());
                                found = true;
                                break;
                            case Strings.BanExpires:
                                Output.Add(Strings.BanExpires + "\t\t" + BanExpires.ToString());
                                found = true;
                                break;
                            case Strings.BanReason:
                                Output.Add(Strings.BanReason + "\t\t" + BanReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesBanned:
                                Output.Add(Strings.TimesBanned + "\t\t" + TimesBanned.ToString());
                                found = true;
                                break;
                            case Strings.Frozen:
                                Output.Add(Strings.Frozen + "\t\t\t" + Frozen.ToString());
                                found = true;
                                break;
                            case Strings.FrozenBy:
                                Output.Add(Strings.FrozenBy + "\t\t" + FrozenBy.ToString());
                                found = true;
                                break;
                            case Strings.DateFrozen:
                                Output.Add(Strings.DateFrozen + "\t\t" + DateFrozen.ToString());
                                found = true;
                                break;
                            case Strings.FreezeExpires:
                                Output.Add(Strings.FreezeExpires + "\t\t" + FreezeExpires.ToString());
                                found = true;
                                break;
                            case Strings.FreezeReason:
                                Output.Add(Strings.FreezeReason + "\t\t" + FreezeReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesFrozen:
                                Output.Add(Strings.TimesFrozen + "\t\t" + TimesFrozen.ToString());
                                found = true;
                                break;
                            case Strings.Muted:
                                Output.Add(Strings.Muted + "\t\t\t" + Muted.ToString());
                                found = true;
                                break;
                            case Strings.MutedBy:
                                Output.Add(Strings.MutedBy + "\t\t\t" + MutedBy.ToString());
                                found = true;
                                break;
                            case Strings.DateMuted:
                                Output.Add(Strings.DateMuted + "\t\t" + DateMuted.ToString());
                                found = true;
                                break;
                            case Strings.MuteExpires:
                                Output.Add(Strings.MuteExpires + "\t\t" + MuteExpires.ToString());
                                found = true;
                                break;
                            case Strings.MuteReason:
                                Output.Add(Strings.MuteReason + "\t\t" + MuteReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesMuted:
                                Output.Add(Strings.TimesMuted + "\t\t" + TimesMuted.ToString());
                                found = true;
                                break;
                            case Strings.Kicked:
                                Output.Add(Strings.Kicked + "\t\t\t" + Kicked.ToString());
                                found = true;
                                break;
                            case Strings.KickedBy:
                                Output.Add(Strings.KickedBy + "\t\t" + KickedBy.ToString());
                                found = true;
                                break;
                            case Strings.DateKicked:
                                Output.Add(Strings.DateKicked + "\t\t" + DateKicked.ToString());
                                found = true;
                                break;
                            case Strings.KickReason:
                                Output.Add(Strings.KickReason + "\t\t" + KickReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesKicked:
                                Output.Add(Strings.TimesKicked + "\t\t" + TimesKicked.ToString());
                                found = true;
                                break;
                            case Strings.OthersBanned:
                                Output.Add(Strings.OthersBanned + "\t\t" + OthersBanned.ToString());
                                found = true;
                                break;
                            case Strings.OthersFrozen:
                                Output.Add(Strings.OthersFrozen + "\t\t" + OthersFrozen.ToString());
                                found = true;
                                break;
                            case Strings.OthersMuted:
                                Output.Add(Strings.OthersMuted + "\t\t" + OthersMuted.ToString());
                                found = true;
                                break;
                            case Strings.OthersKicked:
                                Output.Add(Strings.OthersKicked + "\t\t" + OthersKicked.ToString());
                                found = true;
                                break;
                            case Strings.GroupRepresented:
                                Output.Add(Strings.GroupRepresented + "\t" + GroupRepresented.ToString());
                                found = true;
                                break;
                            #endregion
                            default:
                                Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header));
                                //Unrecognised Value.
                                break;
                        }
                    }
                    if (!found)
                    {
                        switch (Key)
                        {
                            #region Switches
                            case Strings.Name:
                                Output.Add(Strings.Name + "\t\t" + Name);
                                found = true;
                                break;
                            case Strings.DisplayedName:
                                Output.Add(Strings.DisplayedName + "\t\t" + DisplayedName.ToString());
                                found = true;
                                break;
                            case Strings.LastIP:
                                Output.Add(Strings.LastIP + "\t\t\t" + LastIP.ToString());
                                found = true;
                                break;
                            case Strings.LoginCount:
                                Output.Add(Strings.LoginCount + "\t\t" + LoginCount.ToString());
                                found = true;
                                break;
                            case Strings.MessagesTyped:
                                Output.Add(Strings.MessagesTyped + "\t\t" + MessagesTyped.ToString());
                                found = true;
                                break;
                            case Strings.DateJoined:
                                Output.Add(Strings.DateJoined + "\t\t" + DateJoined.ToString());
                                found = true;
                                break;
                            case Strings.DateLastVisited:
                                Output.Add(Strings.DateLastVisited + "\t\t" + DateLastVisited.ToString());
                                found = true;
                                break;
                            case Strings.PlayTime:
                                Output.Add(Strings.PlayTime + "\t\t" + PlayTime.ToString());
                                found = true;
                                break;
                            case Strings.Kills:
                                Output.Add(Strings.Kills + "\t\t\t" + Kills.ToString());
                                found = true;
                                break;
                            case Strings.Deaths:
                                Output.Add(Strings.Deaths + "\t\t\t" + Deaths.ToString());
                                found = true;
                                break;
                            case Strings.FlightsFlown:
                                Output.Add(Strings.FlightsFlown + "\t\t" + FlightsFlown.ToString());
                                found = true;
                                break;
                            case Strings.FlightHours:
                                Output.Add(Strings.FlightHours + "\t\t" + FlightHours.ToString());
                                found = true;
                                break;
                            case Strings.Banned:
                                Output.Add(Strings.Banned + "\t\t\t" + Banned.ToString());
                                found = true;
                                break;
                            case Strings.BannedBy:
                                Output.Add(Strings.BannedBy + "\t\t" + BannedBy.ToString());
                                found = true;
                                break;
                            case Strings.DateBanned:
                                Output.Add(Strings.DateBanned + "\t\t" + DateBanned.ToString());
                                found = true;
                                break;
                            case Strings.BanExpires:
                                Output.Add(Strings.BanExpires + "\t\t" + BanExpires.ToString());
                                found = true;
                                break;
                            case Strings.BanReason:
                                Output.Add(Strings.BanReason + "\t\t" + BanReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesBanned:
                                Output.Add(Strings.TimesBanned + "\t\t" + TimesBanned.ToString());
                                found = true;
                                break;
                            case Strings.Frozen:
                                Output.Add(Strings.Frozen + "\t\t\t" + Frozen.ToString());
                                found = true;
                                break;
                            case Strings.FrozenBy:
                                Output.Add(Strings.FrozenBy + "\t\t" + FrozenBy.ToString());
                                found = true;
                                break;
                            case Strings.DateFrozen:
                                Output.Add(Strings.DateFrozen + "\t\t" + DateFrozen.ToString());
                                found = true;
                                break;
                            case Strings.FreezeExpires:
                                Output.Add(Strings.FreezeExpires + "\t\t" + FreezeExpires.ToString());
                                found = true;
                                break;
                            case Strings.FreezeReason:
                                Output.Add(Strings.FreezeReason + "\t\t" + FreezeReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesFrozen:
                                Output.Add(Strings.TimesFrozen + "\t\t" + TimesFrozen.ToString());
                                found = true;
                                break;
                            case Strings.Muted:
                                Output.Add(Strings.Muted + "\t\t\t" + Muted.ToString());
                                found = true;
                                break;
                            case Strings.MutedBy:
                                Output.Add(Strings.MutedBy + "\t\t\t" + MutedBy.ToString());
                                found = true;
                                break;
                            case Strings.DateMuted:
                                Output.Add(Strings.DateMuted + "\t\t" + DateMuted.ToString());
                                found = true;
                                break;
                            case Strings.MuteExpires:
                                Output.Add(Strings.MuteExpires + "\t\t" + MuteExpires.ToString());
                                found = true;
                                break;
                            case Strings.MuteReason:
                                Output.Add(Strings.MuteReason + "\t\t" + MuteReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesMuted:
                                Output.Add(Strings.TimesMuted + "\t\t" + TimesMuted.ToString());
                                found = true;
                                break;
                            case Strings.Kicked:
                                Output.Add(Strings.Kicked + "\t\t\t" + Kicked.ToString());
                                found = true;
                                break;
                            case Strings.KickedBy:
                                Output.Add(Strings.KickedBy + "\t\t" + KickedBy.ToString());
                                found = true;
                                break;
                            case Strings.DateKicked:
                                Output.Add(Strings.DateKicked + "\t\t" + DateKicked.ToString());
                                found = true;
                                break;
                            case Strings.KickReason:
                                Output.Add(Strings.KickReason + "\t\t" + KickReason.ToString());
                                found = true;
                                break;
                            case Strings.TimesKicked:
                                Output.Add(Strings.TimesKicked + "\t\t" + TimesKicked.ToString());
                                found = true;
                                break;
                            case Strings.OthersBanned:
                                Output.Add(Strings.OthersBanned + "\t\t" + OthersBanned.ToString());
                                found = true;
                                break;
                            case Strings.OthersFrozen:
                                Output.Add(Strings.OthersFrozen + "\t\t" + OthersFrozen.ToString());
                                found = true;
                                break;
                            case Strings.OthersMuted:
                                Output.Add(Strings.OthersMuted + "\t\t" + OthersMuted.ToString());
                                found = true;
                                break;
                            case Strings.OthersKicked:
                                Output.Add(Strings.OthersKicked + "\t\t" + OthersKicked.ToString());
                                found = true;
                                break;
                            case Strings.GroupRepresented:
                                Output.Add(Strings.GroupRepresented + "\t" + GroupRepresented.ToString());
                                found = true;
                                break;
                            #endregion
                            default:
                                Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                                //Unrecognised Value.
                                break;
                        }
                    }
                    try
                    {
                        File.WriteAllLines(OutputFile, Output.ToList());
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}