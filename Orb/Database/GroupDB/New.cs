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
            public static Group New(string Name)
            {
                Group NewGroup;
                List<Group> MatchedGroups = new List<Group>();
                List<Group> GroupListCache = GroupDB.List.ToArray().ToList();
                foreach (Group GroupObject in GroupListCache)
                {
                    if (GroupObject.Name == Name)
                    {
                        MatchedGroups.Add(GroupObject);
                    }
                }
                if (MatchedGroups.Count == 0)
                {
                    try
                    {
                        NewGroup = new Group();
                        NewGroup.Name = Name;
                        NewGroup.DisplayedName = Name;
                        if (NewGroup.Name.ToUpperInvariant() == "NULL") return NewGroup;
                        List.Add(NewGroup);
                        Logger.Log.SystemMessage("    Created Group: " + NewGroup.Name);
                        return NewGroup;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Error Adding a new group to the database.");
                        return GroupDB.NoGroup;
                    }
                }
                else if (MatchedGroups.Count == 1)
                {
                    Logger.Log.SystemMessage(String.Format("    There is already a group by the name of \"{0}\"", Name));
                    return MatchedGroups[0];
                }
                else if (MatchedGroups.Count > 1)
                {
                    return GroupDB.NoGroup;
                }
                else {
                    Logger.Log.SystemMessage(String.Format("    Unknown Error in the User.New() Function. outlist.Count: {0}", MatchedGroups.Count));
                    return GroupDB.NoGroup;
                }
            }
        }
    }
}
