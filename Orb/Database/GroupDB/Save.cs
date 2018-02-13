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
            public static void SaveAll()
            {
                foreach(Group ThisGroup in List) {
                    if (ThisGroup == GroupDB.NoGroup) continue;
                    ThisGroup.SaveAll();
                }
            }
            public static void Save(string GroupName)
            {
                Group ThisGroup = GroupDB.FindGroup(GroupName);
                if (ThisGroup == GroupDB.NoGroup) return;
                ThisGroup.SaveAll();
            }
        }
    }
}