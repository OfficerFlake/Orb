using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Orb
{
    public static partial class Database
    {
        public static partial class GroupDB
        {
            public partial class Group
            {
                public string ListRanksToString()
                {
                    string outlist = "";
                    if (Ranks.Count == 0)
                    {
                        outlist = "No Ranks.";
                    }
                    else
                    {
                        foreach (Rank RankReference in Ranks)
                        {
                            outlist += RankReference.DisplayedName + ", ";
                        }
                    }
                    return outlist.FinaliseStringList();
                }
            }   
        }
    }
}
