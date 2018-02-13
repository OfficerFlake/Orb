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
            public static partial class ServerOptions
            {
                public static object Get(string Header)
                {
                    switch (Header.ToUpperInvariant())
                    {
                        //Network
                        #region ServerPort
                        case Settings.Strings.ServerPort:
                            return ServerPort;
                        #endregion
                        #region OrbPort
                        case Settings.Strings.OrbPort:
                            return OrbPort;
                        #endregion
                        #region ServerIP
                        case Settings.Strings.ServerIP:
                            return ServerIP;
                        #endregion

                        //Debug
                        #region LocalTestMode
                        case Settings.Strings.LocalTestMode:
                            return LocalTestMode;
                        #endregion
                        #region VerboseDebug
                        case Settings.Strings.VerboseDebug:
                            return VerboseDebug;
                        #endregion

                        //Authentication
                        #region AlwaysAllowLocalHost
                        case Settings.Strings.AlwaysAllowLocalHost:
                            return AlwaysAllowLocalHost;
                        #endregion
                        #region UseYSFHQAuthentication
                        case Settings.Strings.UseYSFHQAuthentication:
                            return UseYSFHQAuthentication;
                        #endregion

                        //Security
                        #region StartLock
                        case Settings.Strings.StartLock:
                            return StartLock;
                        #endregion
                        #region StartPassword
                        case Settings.Strings.StartPass:
                            return StartPass;
                        #endregion

                        //Console
                        #region Console
                        case Settings.Strings.ConsoleName:
                            return ConsoleName;
                        #endregion
                        #region GUIMode
                        case Settings.Strings.GUIMode:
                            return GUIMode;
                        #endregion
                        default:
                            Logger.Log.SystemMessage(String.Format("Unrecognised Value: {0}", Header));
                            //Unrecognised Value.
                            return null;
                    }
                }
            }
        }
    }
}