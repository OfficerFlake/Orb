using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Orb
{
    public static partial class Database
    {
        public static partial class UserDB
        {
            public static bool Delete(string Name)
            {
                User Target = UserDB.Find(Name);
                if (Target == UserDB.Nobody)
                {
                    Logger.Log.SystemMessage(String.Format("Could Not Delete User \"{0}\"", Name));
                    try
                    {
                        var deletable = new DirectoryInfo("./Database/Users/" + Name);
                        deletable.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Directory \"" + "./Database/Users/" + Name + "\" is in use or does not exist!");
                    }
                    return false;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Deleted User \"{0}\"", Name));
                    UserDB.List.Remove(Target);
                    try
                    {
                        var deletable = new DirectoryInfo("./Database/Users/" + Name);
                        deletable.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Directory \"" + "./Database/Users/" + Name + "\" is NOT empty!");
                    }
                    return true;
                }
            }
            public static bool DeleteAll()
            {
                List<User> ListCache = List.ToArray().ToList();
                Logger.Log.SystemMessage("Deleteing All Users...");
                foreach (User ThisUser in ListCache)
                {
                    UserDB.Delete(ThisUser.Name);
                }
                string UsersDir = "./Database/Users/";
                Utilities.IO.PrepareDirectory("./Database/");
                Utilities.IO.PrepareDirectory("./Database/Users");
                DirectoryInfo[] UsersDirContents = new DirectoryInfo(UsersDir).GetDirectories();
                foreach (DirectoryInfo ThisDir in UsersDirContents)
                {
                    try
                    {
                        ThisDir.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, String.Format("Directory In Use: {0}", ThisDir.FullName));
                    }
                }
                Logger.Log.SystemMessage("All Users Deleted.");
                return true;
            }
        }
    }
}
