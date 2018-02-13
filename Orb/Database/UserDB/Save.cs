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
            public static void SaveAll()
            {
                foreach(User ThisUser in List) {
                    if (ThisUser == UserDB.Nobody) continue;
                    if (ThisUser == UserDB.Connecting) continue;
                    if (ThisUser == UserDB.SuperUser) continue;
                    ThisUser.SaveAll();
                }
            }
            public static void Save(string UserName)
            {
                User ThisUser = UserDB.Find(UserName);
                if (ThisUser == UserDB.Nobody) return;
                if (ThisUser == UserDB.Connecting) return;
                if (ThisUser == UserDB.SuperUser) return;
                ThisUser.SaveAll();
            }
        }
    }
}