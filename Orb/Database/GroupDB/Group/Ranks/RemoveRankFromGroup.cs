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
                public bool RemoveRankFromGroup(Rank RankToRemove)
                {
                    try
                    {
                        List<Rank> RankCache = Ranks.ToArray().ToList();
                        Rank NewRank = NoRank;
                        if (RankCache.IndexOf(RankToRemove) > 0) NewRank = RankCache[RankCache.IndexOf(RankToRemove) - 1];
                        else if (RankCache.IndexOf(RankToRemove) < RankCache.Count() - 1) NewRank = RankCache[RankCache.IndexOf(RankToRemove) + 1];
                        else NewRank = NoRank;
                        string RankType = "Demoted";
                        if (RankCache.IndexOf(RankToRemove) == 0) RankType = "Promoted";
                        //We need to iterate over EVERY user and remove their reference to the rank we are trying to destory.

                        //Itereate over the userlist, look for a user who has this rank in thier groups:
                        List<UserDB.User> UserListCache = UserDB.List.ToArray().ToList();
                        foreach (UserDB.User ThisUser in UserListCache)
                        {
                            foreach (UserDB.User.GroupReference ThisGroupReference in ThisUser.Groups.ToArray())
                            {
                                if (ThisGroupReference.Group == this)
                                //This means that only the current group is rank adjusted. Though it shouldn't happen, groups COULD share the same object?
                                {
                                    if (ThisGroupReference.Rank == RankToRemove)
                                    {
                                        ThisGroupReference.PreviousRank = NoRank;
                                        ThisGroupReference.Rank = NewRank;
                                        if (NewRank != NoRank)
                                        {
                                            //only message online users.
                                            Server.ClientList.Where(x => x.UserObject == ThisUser).ToList().SendMessage("Your rank in Group: \"" + ThisGroupReference.Group.Name + "\" has been removed. You have been " + RankType + " to Rank: \"" + NewRank.Name + "\".");
                                        }
                                        else
                                        {
                                            //only message online users.
                                            Server.ClientList.Where(x => x.UserObject == ThisUser).ToList().SendMessage("Your rank in Group: \"" + ThisGroupReference.Group.Name + "\" has been removed. As it was the last rank, You have been removed from the group.");
                                            ThisUser.RemoveFromGroup(this);
                                            ThisUser.SaveAll();
                                            continue;
                                        }
                                        ThisGroupReference.RankDate = DateTime.Now;
                                        ThisGroupReference.RankedBy = UserDB.SuperUser;
                                        ThisGroupReference.RankReason = "Previous Rank Deleted.";
                                        ThisUser.SaveAll();
                                    }
                                }
                            }
                        }
                        Ranks.Remove(RankToRemove);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Bug(e, "Error removing a rank from a group");
                        return false;
                    }
                }
            }
        }
    }
}
