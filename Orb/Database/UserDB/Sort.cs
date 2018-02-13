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
            public static partial class Sort
            {
                public static User[] Alphabetically(User[] UserArray)
                {
                    return UserArray.OrderBy(x => x.DisplayedName).ToArray();
                }
                public static List<User> Alphabetically(List<User> UserList)
                {
                    return UserList.OrderBy(x => x.DisplayedName).ToList();
                }
            }
        }
    }
}
