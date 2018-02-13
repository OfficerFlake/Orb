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
            public static User New(string Name)
            {
                User NewUser;
                List<User> MatchedUsers = new List<User>();
                List<User> UserListCache = List.ToArray().ToList();
                foreach (User userobject in UserListCache)
                {
                    if (userobject.Name.ToUpper() == Name.ToUpper())
                    {
                        MatchedUsers.Add(userobject);
                    }
                }
                if (MatchedUsers.Count == 0)
                {
                    try
                    {
                        NewUser = new User();
                        NewUser.Name = Name;
                        NewUser.DisplayedName = Name;
                        if (NewUser.Name.ToUpperInvariant() == "NULL") return NewUser;
                        if (NewUser.Name.ToUpperInvariant() == "CONNECTING...") return NewUser;
                        if (NewUser.Name.ToUpperInvariant() == "ORB") return NewUser;
                        List.Add(NewUser);
                        Logger.Log.SystemMessage("    Created User: " + NewUser.Name);
                        return NewUser;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Error Adding a new user to the database.");
                        return UserDB.Nobody;
                    }
                }
                else if (MatchedUsers.Count == 1)
                {
                    Logger.Log.SystemMessage(String.Format("    There is already a user by the name of \"{0}\"", Name));
                    return MatchedUsers[0];
                }
                else if (MatchedUsers.Count > 1)
                {
                    return UserDB.Nobody;
                }
                else {
                    Logger.Log.SystemMessage(String.Format("    Unknown Error in the User.New() Function. outlist.Count: {0}", MatchedUsers.Count));
                    return UserDB.Nobody;
                }
            }
        }
    }
}
