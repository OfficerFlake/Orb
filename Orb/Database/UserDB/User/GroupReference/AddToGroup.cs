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
                public GroupReference AddToGroup(GroupDB.Group ThisGroup)
                {
                    if (Groups.Select(x => x.Group).Contains(ThisGroup))
                    {
                        foreach (GroupReference ThisGroupReference in Groups)
                        {
                            if (ThisGroupReference.Group == ThisGroup) {
                                return ThisGroupReference;
                            }
                        }
                    }
                    GroupReference output = new GroupReference();
                    if (ThisGroup == GroupDB.NoGroup)
                    {
                        Logger.Log.SystemMessage("Tried to add user \"" + Name + "\" to the NULL Group placeholder.");
                        return NoGroupReference;
                    }
                    output.Group = ThisGroup;
                    if (ThisGroup.Ranks.Count() >= 1)
                    {
                        output.Rank = ThisGroup.Ranks[0];
                    }
                    else
                    {
                        output.Rank = Database.GroupDB.NoRank;
                    }
                    output.RankDate = DateTime.Now;
                    output.Parent = this;
                    Groups.Add(output);
                    return output;
                }   
            }   
        }
    }
}
