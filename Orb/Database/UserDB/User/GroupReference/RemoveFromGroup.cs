using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Orb
{
    public static partial class Database
    {
        public static partial class UserDB
        {
            public partial class User
            {
                public bool RemoveFromGroup(GroupDB.Group ThisGroup)
                {
                    try
                    {
                        List<GroupReference> CachedGroupList = Groups.ToArray().ToList();
                        foreach (GroupReference ThisGroupReference in CachedGroupList)
                        {
                            if (ThisGroupReference.Group == ThisGroup)
                            {
                                Groups.Remove(ThisGroupReference);
                            }
                        }
                        if (GroupRepresented == ThisGroup)
                        {
                            GroupRepresented = GroupDB.NoGroup;
                        }
                        return true;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Tried to remove user \"" + Name + "\" from a group");
                        return false;
                    }
                }   
            }   
        }
    }
}
