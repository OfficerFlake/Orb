using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Orb
{
    public static partial class Database
    {
        public static partial class GroupDB
        {
            public partial class Group
            {
                public bool DeleteRank(Rank ThisRank)
                {
                    Logger.Log.SystemMessage(String.Format("Deleted Rank \"{0}\"", ThisRank.Name));
                        if (Ranks.Contains(ThisRank))
                        {
                            try
                            {
                                var deletable = new DirectoryInfo("./Database/Group/" + Name + "/" + ThisRank.Name);
                                deletable.Delete(true);
                                SaveAll(); //This will actually remove the group reference.
                            }
                            catch (Exception e)
                            {
                                Logger.Log.Bug(e, "Directory \"" + "./Database/Group/" + ThisRank.Name + "\" is in use or does not exist!");
                            }
                    }
                    //Now we get all users who referenced this rank and change their rank to NoRank.
                    foreach (UserDB.User ThisUser in UserDB.List)
                    {
                        foreach (UserDB.User.GroupReference ThisGroupReference in ThisUser.Groups)
                        {
                            if (ThisGroupReference.Rank == ThisRank)
                            {
                                ThisGroupReference.Rank = NoRank;
                            }
                        }
                    }
                    Ranks.Remove(ThisRank);
                    return true;
                }
                public bool DeleteAllRanks()
                {
                    List<Rank> ListCache = Ranks.ToArray().ToList();
                    Logger.Log.SystemMessage("Deleteing All Ranks for group: " + Name + "...");
                    foreach (Rank ThisRank in ListCache)
                    {
                        DeleteRank(ThisRank);
                    }
                    Logger.Log.SystemMessage("All Ranks Deleted for group: " + Name);
                    return true;
                }
            }
        }
    }
}