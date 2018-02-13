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
                public void LoadAll()
                {
                    string InputFile = "./Database/Users/" + Name + "/Info.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Users/");
                    Utilities.IO.PrepareDirectory("./Database/Users/" + Name);
                    Utilities.IO.PrepareFile(InputFile);
                    string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                    Logger.Log.SystemMessage("Loading Values for User \"" + Name + "\"...");
                    foreach (string line in InfoFileContents)
                    {
                        #region GetDataCouplets
                        string ThisLine = line;
                        if (!(ThisLine.Contains("\t")))
                        {
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
                        #region SkipComments
                        if (Header.ToUpperInvariant().StartsWith("REM") || (Header.ToUpperInvariant().StartsWith("#")) || (Header.ToUpperInvariant().StartsWith("//")))
                        {
                            //We don't worry abbout Comments or remarks.
                            continue;
                        }
                        #endregion

                        object inputvalue = null;
                        //Logger.Console.WriteLine(Converted.ToString());
                        //Console.WriteLine(Converted);
                        try
                        {
                            if (Data == "") continue;
                            if (Header.ToUpperInvariant() == "GROUP")
                            {
                                //try
                                //{
                                    string GroupString = Data;
                                    GroupDB.Group ThisGroup = GroupDB.FindGroup(GroupString);
                                    if (ThisGroup == GroupDB.NoGroup) throw new System.ArgumentException("Group Not Found: \"" + GroupString + "\".");
                                    GroupReference ThisGroupReference = AddToGroup(ThisGroup);
                                    ThisGroupReference.LoadAll();
                                //}
                                //catch (Exception e) { Logger.Log.Bug(e, Strings.OthersKicked + " given is NOT a group reference."); }
                                continue;
                            }

                            Type temp = this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).FieldType;
                            if (temp == typeof(IPAddress)) inputvalue = IPAddress.Parse(Data);
                            else if (temp == typeof(Database.GroupDB.Group)) inputvalue = Database.GroupDB.FindGroup(Data);
                            else if (temp == typeof(Database.UserDB.User)) inputvalue = Database.UserDB.Find(Data);
                            else if (temp == typeof(DateTime))
                            {
                                try
                                {
                                    inputvalue = Data.ToDateTime();
                                }
                                catch (Exception e)
                                {
                                    //throw new System.ArgumentException(Header + ", " + Data, e);
                                }
                            }
                            else if (temp == typeof(TimeSpan)) inputvalue = TimeSpan.Parse(Data);
                            else inputvalue = Convert.ChangeType(Data, temp);
                            this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).SetValue(this, inputvalue);
                            //Logger.Console.WriteLine(this.GetType().GetField(Header.ToUpperInvariant(), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).Name.SuffixTabs(4) + Data);
                        }
                        catch (Exception e)
                        {
                            ///*
                            Logger.Console.WriteLine("&cERROR&e " + e);
                            Logger.Console.WriteLine(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                            Logger.Console.WriteLine(String.Format("Unrecognised Value: {0}", Data.ToUpperInvariant()));
                            //Thread.Sleep(50000);
                            //*/
                            //System.Console.WriteLine(e);
                            Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                            //Unrecognised Value.
                            continue;
                        }
                    }
                    //Load All Permissions for this user.
                    //  As permissions in it's own right is a suboject,
                    //  Capabable of it's own specific Load() one var only,
                    //  we can do that instead if a specific permission needs to be reloaded.
                    InputFile = "./Database/Users/" + Name + "/Permissions.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Users/");
                    Utilities.IO.PrepareDirectory("./Database/Users/" + Name);
                    Utilities.IO.PrepareFile(InputFile);
                    this.Permissions.LoadAll(InputFile);

                    Logger.Log.SystemMessage("Loaded Values for User \"" + Name + "\".");
                    //end of loading is here.
                }
                public void Load(string Key)
                {
                    string InputFile = "./Database/Users/" + Name + "/Info.Dat";
                    Utilities.IO.PrepareDirectory("./Database/");
                    Utilities.IO.PrepareDirectory("./Database/Users/");
                    Utilities.IO.PrepareDirectory("./Database/Users/" + Name);
                    Utilities.IO.PrepareFile(InputFile);
                    string[] InfoFileContents = Utilities.IO.ReadAllLines(InputFile);
                    foreach (string line in InfoFileContents)
                    {
                        #region GetDataCouplets
                        string ThisLine = line;
                        if (!(ThisLine.Contains("\t")))
                        {
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
                        #region SkipComments
                        if (Header.ToUpperInvariant().StartsWith("REM") || (Header.ToUpperInvariant().StartsWith("#")) || (Header.ToUpperInvariant().StartsWith("//")))
                        {
                            //We don't worry abbout Comments or remarks.
                            continue;
                        }
                        #endregion
                        if (Key.ToUpperInvariant() != Header.ToUpperInvariant())
                        {
                            //Do not load any variable except what we need.
                            continue;
                        }
                        switch (Header.ToUpperInvariant())
                        {
                            #region Switches
                            case Strings.Name:
                                try { Name = (string)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Name + " given is NOT a string."); }
                                break;
                            case Strings.DisplayedName:
                                try { DisplayedName = (string)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DisplayedName + " given is NOT a string."); }
                                break;
                            case Strings.LastIP:
                                try { LastIP = (IPAddress)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.LastIP + " given is NOT a string."); }
                                break;
                            case Strings.LoginCount:
                                try { LoginCount = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.LoginCount + " given is NOT a string."); }
                                break;
                            case Strings.MessagesTyped:
                                try { MessagesTyped = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.MessagesTyped + " given is NOT a string."); }
                                break;
                            case Strings.DateJoined:
                                try { DateJoined = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DateJoined + " given is NOT a string."); }
                                break;
                            case Strings.DateLastVisited:
                                try { DateLastVisited = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DateLastVisited + " given is NOT a string."); }
                                break;
                            case Strings.PlayTime:
                                try { PlayTime = TimeSpan.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.PlayTime + " given is NOT a string."); }
                                break;
                            case Strings.Kills:
                                try { Kills = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Kills + " given is NOT a string."); }
                                break;
                            case Strings.Deaths:
                                try { Deaths = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Deaths + " given is NOT a string."); }
                                break;
                            case Strings.FlightsFlown:
                                try { FlightsFlown = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.FlightsFlown + " given is NOT a string."); }
                                break;
                            case Strings.FlightHours:
                                try { FlightHours = TimeSpan.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.FlightHours + " given is NOT a string."); }
                                break;
                            case Strings.Banned:
                                try { Banned = (bool)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Banned + " given is NOT a string."); }
                                break;
                            case Strings.BannedBy:
                                try { BannedBy = UserDB.Find(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.BannedBy + " given is NOT a string."); }
                                break;
                            case Strings.DateBanned:
                                try { DateBanned = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DateBanned + " given is NOT a string."); }
                                break;
                            case Strings.BanExpires:
                                try { BanExpires = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.BanExpires + " given is NOT a string."); }
                                break;
                            case Strings.BanReason:
                                try { BanReason = (string)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.BanReason + " given is NOT a string."); }
                                break;
                            case Strings.TimesBanned:
                                try { TimesBanned = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.TimesBanned + " given is NOT a string."); }
                                break;
                            case Strings.Frozen:
                                try { Frozen = (bool)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Frozen + " given is NOT a string."); }
                                break;
                            case Strings.FrozenBy:
                                try { FrozenBy = UserDB.Find(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.FrozenBy + " given is NOT a string."); }
                                break;
                            case Strings.DateFrozen:
                                try { DateFrozen = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DateFrozen + " given is NOT a string."); }
                                break;
                            case Strings.FreezeExpires:
                                try { FreezeExpires = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.FreezeExpires + " given is NOT a string."); }
                                break;
                            case Strings.FreezeReason:
                                try { FreezeReason = (string)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.FreezeReason + " given is NOT a string."); }
                                break;
                            case Strings.TimesFrozen:
                                try { TimesFrozen = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.TimesFrozen + " given is NOT a string."); }
                                break;
                            case Strings.Muted:
                                try { Muted = (bool)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Muted + " given is NOT a string."); }
                                break;
                            case Strings.MutedBy:
                                try { MutedBy = UserDB.Find(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.MutedBy + " given is NOT a string."); }
                                break;
                            case Strings.DateMuted:
                                try { DateMuted = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DateMuted + " given is NOT a string."); }
                                break;
                            case Strings.MuteExpires:
                                try { MuteExpires = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.MuteExpires + " given is NOT a string."); }
                                break;
                            case Strings.MuteReason:
                                try { MuteReason = (string)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.MuteReason + " given is NOT a string."); }
                                break;
                            case Strings.TimesMuted:
                                try { TimesMuted = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.TimesMuted + " given is NOT a string."); }
                                break;
                            case Strings.Kicked:
                                try { Kicked = (bool)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.Kicked + " given is NOT a string."); }
                                break;
                            case Strings.KickedBy:
                                try { KickedBy = UserDB.Find(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.KickedBy + " given is NOT a string."); }
                                break;
                            case Strings.DateKicked:
                                try { DateKicked = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.DateKicked + " given is NOT a string."); }
                                break;
                            case Strings.KickExpires:
                                try { KickExpires = DateTime.Parse(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.KickExpires + " given is NOT a string."); }
                                break;
                            case Strings.KickReason:
                                try { KickReason = (string)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.KickReason + " given is NOT a string."); }
                                break;
                            case Strings.TimesKicked:
                                try { TimesKicked = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.TimesKicked + " given is NOT a string."); }
                                break;
                            case Strings.OthersBanned:
                                try { OthersBanned = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.OthersBanned + " given is NOT a string."); }
                                break;
                            case Strings.OthersFrozen:
                                try { OthersFrozen = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.OthersFrozen + " given is NOT a string."); }
                                break;
                            case Strings.OthersMuted:
                                try { OthersMuted = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.OthersMuted + " given is NOT a string."); }
                                break;
                            case Strings.OthersKicked:
                                try { OthersKicked = (int)Converted; }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.OthersKicked + " given is NOT a string."); }
                                break;
                            case Strings.GroupRepresented:
                                try { GroupRepresented = GroupDB.FindGroup(Converted.ToString()); }
                                catch (Exception e) { Logger.Log.Bug(e, Strings.GroupRepresented + " given is NOT a string."); }
                                break;
                            #endregion
                            default:
                                Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header.ToUpperInvariant()));
                                //Unrecognised Value.
                                break;
                        }
                    }
                    //end of loading is here.
                }
            }
        }
    }
}