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
                public Rank FindRank(string Name)
                {
                    List<Rank> outlist = new List<Rank>();
                    List<Rank> RankListCache = this.Ranks.ToArray().ToList();
                    if (Name.ToUpperInvariant() == "NULL")
                    {
                        return Database.GroupDB.NoRank;
                    }
                    foreach (Rank Rank in RankListCache)
                    {
                        if (Rank.Name == Name || Rank.Name.Replace('_', ' ') == Name.Replace('_', ' '))
                        {
                            outlist.Add(Rank);
                        }
                    }
                    if (outlist.Count == 0)
                    {
                        return _FindRankCaseInsensitive(Name);
                    }
                    else if (outlist.Count == 1)
                    {
                        return outlist[0];
                    }
                    else if (outlist.Count > 1)
                    {
                        Logger.Log.SystemMessage(String.Format("Mutliple Ranks Found: {0}", outlist.ToString()));
                        return Database.GroupDB.NoRank;
                    }
                    else
                    {
                        Logger.Log.SystemMessage(String.Format("Unknown Error in the Group.Rank.Find() Function. outlist.Count: {0}", outlist.Count));
                        return Database.GroupDB.NoRank;
                    }
                }

                private Rank _FindRankCaseInsensitive(string Name)
                {
                    List<Rank> outlist = new List<Rank>();
                    List<Rank> RankListCache = this.Ranks.ToArray().ToList();
                    if (Name.ToUpperInvariant() == "NULL")
                    {
                        return Database.GroupDB.NoRank;
                    }
                    foreach (Rank Rank in RankListCache)
                    {
                        if (Rank.Name.ToUpperInvariant() == Name.ToUpperInvariant() || Rank.Name.ToUpperInvariant().Replace('_', ' ') == Name.ToUpperInvariant().Replace('_', ' '))
                        {
                            outlist.Add(Rank);
                        }
                    }
                    if (outlist.Count == 0)
                    {
                        return _FindRankCaseInsensitiveContaining(Name);
                    }
                    else if (outlist.Count == 1)
                    {
                        return outlist[0];
                    }
                    else if (outlist.Count > 1)
                    {
                        Logger.Log.SystemMessage(String.Format("Mutliple Ranks Found: {0}", outlist.ToString()));
                        return Database.GroupDB.NoRank;
                    }
                    else
                    {
                        Logger.Log.SystemMessage(String.Format("Unknown Error in the Group.Rank.Find() Function. outlist.Count: {0}", outlist.Count));
                        return Database.GroupDB.NoRank;
                    }
                }

                private Rank _FindRankCaseInsensitiveContaining(string Name)
                {
                    List<Rank> outlist = new List<Rank>();
                    List<Rank> RankListCache = this.Ranks.ToArray().ToList();
                    if (Name.ToUpperInvariant() == "NULL")
                    {
                        return Database.GroupDB.NoRank;
                    }
                    foreach (Rank Rank in RankListCache)
                    {
                        if (Rank.Name.ToUpperInvariant().Replace('_', ' ').Contains(Name.ToUpperInvariant().Replace('_', ' ')))
                        {
                            outlist.Add(Rank);
                        }
                    }
                    if (outlist.Count == 0)
                    {
                        Logger.Log.SystemMessage(String.Format("No Rank Found: \"{0}\"", Name));
                        return Database.GroupDB.NoRank;
                    }
                    else if (outlist.Count == 1)
                    {
                        return outlist[0];
                    }
                    else if (outlist.Count > 1)
                    {
                        Logger.Log.SystemMessage(String.Format("Mutliple Ranks Found: {0}", outlist.ToString()));
                        return Database.GroupDB.NoRank;
                    }
                    else
                    {
                        Logger.Log.SystemMessage(String.Format("Unknown Error in the Group.Rank.Find() Function. outlist.Count: {0}", outlist.Count));
                        return Database.GroupDB.NoRank;
                    }
                }
            }
        }
    }
}
