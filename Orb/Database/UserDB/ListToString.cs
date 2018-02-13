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
            public static string ListToString(User User)
            {
                User[] UserToArray = { User };
                return ListToString(UserToArray);
            }
            public static string ListToString(List<User> UserList)
            {
                return ListToString(UserList.ToArray());
            }
            public static string ListToString(User[] UserList)
            {
                string outlist = "";
                if (UserList.Length == 0)
                {
                    outlist = "No Users.";
                    return outlist;
                }
                else
                {
                    foreach (User userobject in UserDB.Sort.Alphabetically(UserList))
                    {
                        outlist += userobject.Name + ", ";
                    }
                }
                return outlist.FinaliseStringList();
            }
        }
    }
}
