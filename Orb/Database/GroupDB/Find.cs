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
            public static Group FindGroup(string Name)
            {
                List<Group> outlist = new List<Group>();
                List<Group> GroupListCache = GroupDB.List;
                if (Name.ToUpperInvariant() == "NULL")
                {
                    return GroupDB.NoGroup;
                }
                foreach (Group groupobject in GroupListCache)
                {
                    if (groupobject.Name.Replace('_', ' ') == Name.Replace('_', ' '))
                    {
                        outlist.Add(groupobject);
                    }
                }
                if (outlist.Count == 0)
                {
                    return _FindGroupCaseInsensitive(Name);
                }
                else if (outlist.Count == 1)
                {
                    return outlist[0];
                }
                else if (outlist.Count > 1)
                {
                    Logger.Log.SystemMessage(String.Format("Mutliple Groups Found: {0}", outlist.ToString()));
                    return GroupDB.NoGroup;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Unknown Error in the Group.Find() Function. outlist.Count: {0}", outlist.Count));
                    return GroupDB.NoGroup;
                }
            }

            private static Group _FindGroupCaseInsensitive(string Name)
            {
                List<Group> outlist = new List<Group>();
                List<Group> GroupListCache = GroupDB.List;
                if (Name.ToUpperInvariant() == "NULL")
                {
                    return GroupDB.NoGroup;
                }
                foreach (Group groupobject in GroupListCache)
                {
                    if (groupobject.Name.ToUpperInvariant().Replace('_', ' ') == Name.ToUpperInvariant().Replace('_', ' '))
                    {
                        outlist.Add(groupobject);
                    }
                }
                if (outlist.Count == 0)
                {
                    return _FindGroupCaseInsensitiveContaining(Name);
                }
                else if (outlist.Count == 1)
                {
                    return outlist[0];
                }
                else if (outlist.Count > 1)
                {
                    Logger.Log.SystemMessage(String.Format("Mutliple Groups Found: {0}", outlist.ToString()));
                    return GroupDB.NoGroup;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Unknown Error in the Group.Find() Function. outlist.Count: {0}", outlist.Count));
                    return GroupDB.NoGroup;
                }
            }

            private static Group _FindGroupCaseInsensitiveContaining(string Name)
            {
                List<Group> outlist = new List<Group>();
                List<Group> GroupListCache = GroupDB.List;
                if (Name.ToUpperInvariant() == "NULL")
                {
                    return GroupDB.NoGroup;
                }
                foreach (Group userobject in GroupListCache)
                {
                    if (userobject.Name.ToUpperInvariant().Replace('_', ' ').Contains(Name.ToUpperInvariant().Replace('_', ' ')))
                    {
                        outlist.Add(userobject);
                    }
                }
                if (outlist.Count == 0)
                {
                    Logger.Log.SystemMessage(String.Format("No Group Found: \"{0}\"", Name));
                    return GroupDB.NoGroup;
                }
                else if (outlist.Count == 1)
                {
                    return outlist[0];
                }
                else if (outlist.Count > 1)
                {
                    Logger.Log.SystemMessage(String.Format("Mutliple Groups Found: {0}", outlist.ToString()));
                    return GroupDB.NoGroup;
                }
                else
                {
                    Logger.Log.SystemMessage(String.Format("Unknown Error in the Group.Find() Function. outlist.Count: {0}", outlist.Count));
                    return GroupDB.NoGroup;
                }
            }
        }
    }
}
