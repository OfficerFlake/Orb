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
            public static partial class Sort
            {
                public static Group[] Alphabetically(Group[] GroupArray)
                {
                    return GroupArray.OrderBy(x => x.DisplayedName).ToArray();
                }
                public static List<Group> Alphabetically(List<Group> GroupList)
                {
                    return GroupList.OrderBy(x => x.DisplayedName).ToList();
                }
            }
        }
    }
}
