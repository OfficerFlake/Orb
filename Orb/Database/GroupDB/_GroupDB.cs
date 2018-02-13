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
            public static List<Group> List = new List<Group>();
            public static List<Group> _List
            {
                get
                {
                    List<Group> output = List.ToArray().ToList();
                    output.Add(NoGroup);
                    return output;
                }
            }
            public static Group NoGroup = GroupDB.New("Null");
            public static Group.Rank NoRank = NoGroup.NewRank("Null");
        }
    }
}
