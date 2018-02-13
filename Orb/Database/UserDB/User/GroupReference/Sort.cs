using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Database
    {
        public static partial class UserDB
        {
            public static partial class GroupReferences
            {
                public static partial class Sort
                {
                    public static User.GroupReference[] Alphabetically(User.GroupReference[] GroupArray)
                    {
                        return GroupArray.OrderBy(x => x.Group.DisplayedName).ToArray();
                    }
                    public static List<User.GroupReference> Alphabetically(List<User.GroupReference> GroupList)
                    {
                        return GroupList.OrderBy(x => x.Group.DisplayedName).ToList();
                    }
                }
            }
        }
    }
}
