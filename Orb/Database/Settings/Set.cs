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
        public static partial class Settings
        {
            public static void Set(string Key, bool Value)
            {
                switch (Key.ToUpperInvariant())
                {
                    //Network

                    //Debug
                    #region ProxyMode
                    case Settings.Strings.ProxyMode:
                        ProxyMode = Value;
                        break;
                    #endregion
                    #region LocalTestMode
                    case Settings.Strings.LocalTestMode:
                        LocalTestMode = Value;
                        break;
                    #endregion
                    #region VerboseDebug
                    case Settings.Strings.VerboseDebug:
                        VerboseDebug = Value;
                        break;
                    #endregion

                    //Authentication
                    #region AlwaysAllowLocalHost
                    case Settings.Strings.AlwaysAllowLocalHost:
                        AlwaysAllowLocalHost = Value;
                        break;
                    #endregion
                    #region UseYSFHQAuthentication
                    case Settings.Strings.UseYSFHQAuthentication:
                        UseYSFHQAuthentication = Value;
                        break;
                    #endregion

                    //Security
                    #region StartLock
                    case Settings.Strings.StartLock:
                        StartLock = Value;
                        break;
                    #endregion

                    //Console
                    #region GUIMode
                    case Settings.Strings.GUIMode:
                        GUIMode = Value;
                        break;
                    #endregion
                    default:
                        Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                        //Unrecognised Value.
                        return;
                }
            }
            public static void Set(string Key, IPAddress Value)
            {
                switch (Key.ToUpperInvariant())
                {
                    //Network
                    #region ServerIP
                    case Settings.Strings.ServerIP:
                        ServerIP = Value;
                        break;
                    #endregion

                    //Debug

                    //Authentication

                    //Security

                    //Console
                    default:
                        Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                        //Unrecognised Value.
                        return;
                }
            }
            public static void Set(string Key, decimal Value)
            {
                switch (Key.ToUpperInvariant())
                {
                    //Network

                    //Debug

                    //Authentication

                    //Security

                    //Console
                    default:
                        Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                        //Unrecognised Value.
                        return;
                }
            }
            public static void Set(string Key, Int32 Value)
            {
                switch (Key.ToUpperInvariant())
                {
                    //Network
                    #region ServerPort
                    case Settings.Strings.ServerPort:
                        ServerPort = Value;
                        break;
                    #endregion
                    #region OrbPort
                    case Settings.Strings.OrbPort:
                        ServerPort = Value;
                        break;
                    #endregion

                    //Debug

                    //Authentication

                    //Security

                    //Console
                    default:
                        Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                        //Unrecognised Value.
                        return;
                }
            }
            public static void Set(string Key, string Value)
            {
                switch (Key.ToUpperInvariant())
                {
                    //Network

                    //Debug

                    //Authentication

                    //Security
                    #region StartPassword
                    case Settings.Strings.StartPass:
                        StartPass = Value;
                        break;
                    #endregion

                    //Console
                    #region Console
                    case Settings.Strings.ConsoleName:
                        ConsoleName = Value;
                        break;
                    #endregion
                    default:
                        Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Key));
                        //Unrecognised Value.
                        return;
                }
            }
        }
    }
}