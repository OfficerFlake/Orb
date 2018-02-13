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
            public partial class Group
            {
                public Rank NewRank(string Name)
                {
                    Rank NewRank;
                    try
                    {
                        NewRank = new Rank();
                        NewRank.Name = Name;
                        NewRank.DisplayedName = Name;
                        if (NewRank.Name.ToUpperInvariant() == "NULL")
                        {
                            if (NoRank == null) return NewRank;
                            else return NoRank;
                        }
                        Ranks.Insert(0, NewRank);
                        //RankDB.List.Add(NewRank);
                        Logger.Log.SystemMessage("Created Rank: " + NewRank.Name);
                        return NewRank;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Error Creating new rank.");
                        return NoRank;
                    }
                }
            }
        }
    }
}
