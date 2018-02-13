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
            public static List<User> List = new List<User>();
            public static List<User> _List
            {
                get
                {
                    List<User> output = List.ToArray().ToList();
                    output.Add(Nobody);
                    output.Add(Connecting);
                    output.Add(SuperUser);
                    return output;
                }
            }
            public static User Nobody = UserDB.New("Null");
            public static User SuperUser = UserDB.New("Orb");
            public static User Connecting = UserDB.New("Connecting...");
            public static User.GroupReference NoGroupReference = new User.GroupReference();
        }
    }
}
