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
            public partial class User
            {
                public GroupReference FindGroupReference(string Name)
                {
                    List<GroupReference> outlist = new List<GroupReference>();
                    List<GroupReference> GroupListCache = this.Groups;
                    if (Name.ToUpperInvariant() == "NULL")
                    {
                        return UserDB.NoGroupReference;
                    }
                    foreach (GroupReference GroupReferenceObject in GroupListCache)
                    {
                        if (GroupReferenceObject.Group.Name == Name)
                        {
                            outlist.Add(GroupReferenceObject);
                        }
                    }
                    if (outlist.Count == 0)
                    {
                        Logger.Log.SystemMessage(String.Format("No Group Reference Found: \"{0}\"", Name));
                        return UserDB.NoGroupReference;
                    }
                    else if (outlist.Count == 1)
                    {
                        return outlist[0];
                    }
                    else if (outlist.Count > 1)
                    {
                        Logger.Log.SystemMessage(String.Format("Mutliple Group References Found: {0}", outlist.ToString()));
                        return UserDB.NoGroupReference;
                    }
                    else
                    {
                        Logger.Log.SystemMessage(String.Format("Unknown Error in the GroupReference.Find() Function. outlist.Count: {0}", outlist.Count));
                        return UserDB.NoGroupReference;
                    }
                }
            }
        }
    }
}
