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
            public static User Find(string Name)
            {
                List<User> outlist = new List<User>();
                List<User> UserListCache = UserDB._List.ToList();
                if (Name.ToUpperInvariant() == "NULL")
                {
                    return UserDB.Nobody;
                }
                foreach (User userobject in UserListCache)
                {
                    if (userobject.Name == Name || userobject.Name.Replace('_', ' ') == Name.Replace('_', ' '))
                    {
                        outlist.Add(userobject);
                    }
                }
                if (outlist.Count == 0)
                {
                    return _FindCaseInsensitive(Name);
                }
                else if (outlist.Count == 1)
                {
                    return outlist[0];
                }
                else if (outlist.Count > 1)
                {
                    Logger.Log.SystemMessage(String.Format("Mutliple Users Found: {0}", outlist.ToString()));
                    return UserDB.Nobody;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Unknown Error in the User.Find() Function. outlist.Count: {0}", outlist.Count));
                    return UserDB.Nobody;
                }
            }

            private static User _FindCaseInsensitive(string Name)
            {
                List<User> outlist = new List<User>();
                List<User> UserListCache = UserDB._List.ToList();
                if (Name.ToUpperInvariant() == "NULL")
                {
                    return UserDB.Nobody;
                }
                foreach (User userobject in UserListCache)
                {
                    if (userobject.Name.ToUpperInvariant() == Name.ToUpperInvariant() || userobject.Name.ToUpperInvariant().Replace('_', ' ') == Name.Replace('_', ' ').ToUpperInvariant())
                    {
                        outlist.Add(userobject);
                    }
                }
                if (outlist.Count == 0)
                {
                    return _FindCaseInsensitiveContaining(Name);
                }
                else if (outlist.Count == 1)
                {
                    return outlist[0];
                }
                else if (outlist.Count > 1)
                {
                    Logger.Log.SystemMessage(String.Format("Mutliple Users Found: {0}", outlist.ToString()));
                    return UserDB.Nobody;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Unknown Error in the User.Find() Function. outlist.Count: {0}", outlist.Count));
                    return UserDB.Nobody;
                }
            }

            private static User _FindCaseInsensitiveContaining(string Name)
            {
                List<User> outlist = new List<User>();
                List<User> UserListCache = UserDB._List.ToList();
                if (Name.ToUpperInvariant() == "NULL")
                {
                    return UserDB.Nobody;
                }
                foreach (User userobject in UserListCache)
                {
                    if (userobject.Name.ToUpperInvariant().Contains(Name.ToUpperInvariant()) || userobject.Name.ToUpperInvariant().Replace('_', ' ').Contains(Name.Replace('_', ' ').ToUpperInvariant()))
                    {
                        outlist.Add(userobject);
                    }
                }
                if (outlist.Count == 0)
                {
                    Logger.Log.SystemMessage(String.Format("No User Found: \"{0}\"", Name));
                    return UserDB.Nobody;
                }
                else if (outlist.Count == 1)
                {
                    return outlist[0];
                }
                else if (outlist.Count > 1)
                {
                    Logger.Log.SystemMessage(String.Format("Mutliple Users Found: {0}", outlist.ToString()));
                    return UserDB.Nobody;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Unknown Error in the User.Find() Function. outlist.Count: {0}", outlist.Count));
                    return UserDB.Nobody;
                }
            }
        }
    }
}
