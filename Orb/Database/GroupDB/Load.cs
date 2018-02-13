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
        public static partial class GroupDB
        {
            public static void LoadAll()
            {
                string GroupsDir = "./Database/Groups/";
                Utilities.IO.PrepareDirectory("./Database/");
                Utilities.IO.PrepareDirectory("./Database/Groups");
                DirectoryInfo[] GroupsDirContents = new DirectoryInfo(GroupsDir).GetDirectories();
                Logger.Log.SystemMessage("Loading All Groups...");
                if (GroupsDirContents.Length == 0)
                {
                    Logger.Log.SystemMessage("    No Groups are in the Database.");
                    return;
                }
                foreach (DirectoryInfo ThisDir in GroupsDirContents)
                {
                    GroupDB.Load(ThisDir.Name);
                }
                Logger.Log.SystemMessage("    All Groups Loaded.");
            }
            public static void Load(string GroupName)
            {
                Group ThisGroup = GroupDB.New(GroupName);
                ThisGroup.LoadAll();
            }
        }
    }
}