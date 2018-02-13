using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Orb
{
    public static partial class Database
    {
        public static partial class GroupDB
        {
            public static bool Delete(Group Target)
            {
                string Name = Target.Name;
                if (Target == GroupDB.NoGroup)
                {
                    return false;
                }
                else
                {
                    foreach (UserDB.User ThisUser in UserDB.List)
                    {
                        if (ThisUser.Groups.Select(x=>x.Group).Contains(Target))
                        {
                            Server.ClientList.Where(x => x.UserObject == ThisUser).ToList().SendMessage("You were removed from Group: \"" + Target.Name + "\" because the group has been removed from the server.");
                            ThisUser.RemoveFromGroup(Target);
                            ThisUser.SaveAll();
                        }
                    }
                    Logger.Log.SystemMessage(String.Format("Deleted Group \"{0}\"", Name));
                    GroupDB.List.Remove(Target);
                    try
                    {
                        var deletable = new DirectoryInfo("./Database/Group/" + Name);
                        deletable.Delete(true);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Directory \"" + "./Database/Group/" + Name + "\" is in use or does not exist!");
                    }
                    return true;
                }
            }
            public static bool DeleteAll()
            {
                List<Group> ListCache = List.ToArray().ToList();
                Logger.Log.SystemMessage("Deleteing All Groups...");
                foreach (Group ThisGroup in ListCache)
                {
                    GroupDB.Delete(ThisGroup);
                }
                string GroupsDir = "./Database/Groups/";
                Utilities.IO.PrepareDirectory("./Database/");
                Utilities.IO.PrepareDirectory("./Database/Groups");
                DirectoryInfo[] GroupsDirContents = new DirectoryInfo(GroupsDir).GetDirectories();
                foreach (DirectoryInfo ThisDir in GroupsDirContents)
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
                Logger.Log.SystemMessage("All Groups Deleted.");
                return true;
            }
        }
    }
}
