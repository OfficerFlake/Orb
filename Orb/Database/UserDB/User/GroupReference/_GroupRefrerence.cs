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
                public partial class GroupReference
                {
                    public GroupDB.Group Group = GroupDB.NoGroup;
                    public GroupDB.Group.Rank Rank = GroupDB.NoRank;
                    public string RankReason = "";
                    public User RankedBy = UserDB.Nobody;
                    public GroupDB.Group.Rank PreviousRank = GroupDB.NoRank;
                    public DateTime RankDate = DateTime.Now;

                    public User Parent = UserDB.Nobody;

                    public partial class Strings
                    {
                        public const string Group = "GROUPNAME";
                        public const string Rank = "RANKNAME";
                        public const string RankReason = "RANKREASON";
                        public const string RankedBy = "RANKEDBY";
                        public const string PreviousRank = "PREVIOUSRANK";
                        public const string RankDate = "RANKDATE";
                    }
                }
                
            }   
        }
    }
}
