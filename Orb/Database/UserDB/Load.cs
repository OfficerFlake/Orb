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
            public static void LoadAll()
            {
                string UsersDir = "./Database/Users/";
                Utilities.IO.PrepareDirectory("./Database/");
                Utilities.IO.PrepareDirectory("./Database/Users");
                DirectoryInfo[] UsersDirContents = new DirectoryInfo(UsersDir).GetDirectories();
                Logger.Log.SystemMessage("Loading All Users...");
                if (UsersDirContents.Length == 0)
                {
                    Logger.Log.SystemMessage("    No Users are in the Database.");
                    return;
                }
                foreach(DirectoryInfo ThisDir in UsersDirContents) {
                    User ThisUser = UserDB.Load(ThisDir.Name);
                }
                Logger.Log.SystemMessage("    All Users Loaded.");
            }
            public static User Load(string UserName)
            {
                User ThisUser = UserDB.New(UserName);
                ThisUser.LoadAll();
                return ThisUser;
            }
        }
    }
}