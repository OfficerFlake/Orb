using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Database
    {
        public static partial class GroupDB
        {
            public static string ListToString(Group ThisGroup)
            {
                Group[] GroupToArray = { ThisGroup };
                return ListToString(GroupToArray);
            }
            public static string ListToString(List<Group> GroupList)
            {
                return ListToString(GroupList.ToArray());
            }
            public static string ListToString(Group[] GroupList)
            {
                string outlist = "";
                if (GroupList.Length == 0)
                {
                    outlist = "No Groups.";
                    return outlist;
                }
                else
                {
                    foreach (Group GroupObject in GroupDB.Sort.Alphabetically(GroupList))
                    {
                        outlist += GroupObject.DisplayedName + ", ";
                    }
                }
                return outlist.FinaliseStringList();
            }
        }
    }
}
