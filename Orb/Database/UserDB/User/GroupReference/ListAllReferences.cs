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
                public string ListGroupsToString()
                {
                    string outlist = "";
                    if (Groups.Count == 0)
                    {
                        outlist = "No Groups.";
                    }
                    else
                    {
                        foreach (User.GroupReference GroupReference in Groups)
                        {
                            outlist += GroupReference.Group.DisplayedName + ", ";
                        }
                    }
                    return outlist.FinaliseStringList();
                }
            }   
        }
    }
}
