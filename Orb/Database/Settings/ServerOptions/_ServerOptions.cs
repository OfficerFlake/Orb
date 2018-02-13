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
                #region Variables
                public static bool ServerLock { get; private set; }
                public static int ServerVersion { get; private set; }
                public static string Map { get; private set; }
                #endregion
                // Add the rest as needed...
                #region Strings
                public static class Strings
                {
                    public const string ServerLocked = "SERVERLOCK";
                    public const string ServerVersion = "SERVERVERSION";
                    public const string ServerPort = "MAP";
                }
                #endregion
            }
        }
    }
}