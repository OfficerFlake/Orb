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
            #region Variables
            //Network Addresses.
            public static bool ProxyMode = true;
            public static int ServerPort = 7914;
            public static int OrbPort = 7915;
            public static IPAddress ServerIP = IPAddress.Parse("127.0.0.1");
            public static IPAddress OrbIP = IPAddress.Parse("127.0.0.1");
            
            //Debug Options
            public static bool LocalTestMode = false;
            public static bool VerboseDebug = false;

            //Authentication Options
            public static bool AlwaysAllowLocalHost = true;
            public static bool UseYSFHQAuthentication = true;

            //Security
            public static bool StartLock = false;
            public static string StartPass = "";

            //Console
            public static string ConsoleName = "";
            public static bool GUIMode = true;

            #endregion
            #region Strings
            public static class Strings
            {
                public const string ProxyMode = "PROXYMODE";
                public const string ServerPort = "SERVERPORT";
                public const string OrbPort = "ORBPORT";
                public const string ServerIP = "SERVERIP";
                public const string OrbIP = "ORBIP";
                public const string LocalTestMode = "LOCALTESTMODE";
                public const string VerboseDebug = "VERBOSEDEBUG";
                public const string AlwaysAllowLocalHost = "ALWAYSALLOWLOCALHOST";
                public const string UseYSFHQAuthentication = "USEYSFHQAUTHENTICATION";
                public const string StartLock = "STARTLOCK";
                public const string StartPass = "STARTPASS";
                public const string ConsoleName = "CONSOLENAME";
                public const string GUIMode = "GUIMODE";
            }
            #endregion
        }
    }
}