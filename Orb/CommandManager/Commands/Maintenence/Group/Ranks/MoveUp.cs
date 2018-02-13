using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Commands
    {
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Ranks_Increment = new CommandDescriptor
        {
            _Name = "Increment Rank In Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Increment a ranks position in a group on the server.",
            _Usage = "Usage: /Group.<Name>.Ranks.Increment <RankName>",
            _Commands = new string[] { "/Groups.*.Ranks.Increment", "/Groups.*.Ranks.MoveUp" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Ranks_Increment_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Ranks_Increment_Method(Server.NetObject NetObj, CommandReader Command)
        {
            string TargetRankName = "";
            Database.GroupDB.Group.Rank TargetRank = Database.GroupDB.NoRank;
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            #region FindTargetGroup
            if (Database.GroupDB.FindGroup(Command._CmdElements()[1]) == Database.GroupDB.NoGroup)
            {
                NetObj.ClientObject.SendMessage("Group not found: \"" + Command._CmdElements()[1] + "\".");
                return false;
            }
            TargetGroup = Database.GroupDB.FindGroup(Command._CmdElements()[1]);
            #endregion
            #region FindTargetRank
            if (Command._CmdArguments.Count() < 1)
            {
                NetObj.ClientObject.SendMessage("No rank specified to be incremented in the group.");
                return false;
            }
            else
            {
                TargetRankName = Command._CmdArguments[0].ToUpperInvariant();
                if (!(TargetGroup.Ranks.Select(x => x.Name.ToUpperInvariant()).Contains(TargetRankName)))
                {
                    NetObj.ClientObject.SendMessage("Rank: \"" + TargetRankName + "\" not found in Group: \"" + TargetGroup.Name + "\".");
                    return false;
                }
                else
                {
                    TargetRank = TargetGroup.Ranks.First(x => x.Name.ToUpperInvariant() == TargetRankName);
                }
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject == TargetGroup.Founder)
            {
                //Continue
            }
            #endregion
            #region Standard Testing
            if (NetObj.UserObject.Groups.Select(x => x.Group).Contains(TargetGroup))
            {
                //THIS User is a member of the group. (Required to be able to modify the group.
                Database.UserDB.User.GroupReference ThisGroupReference = NetObj.UserObject.Groups.Where(x => x.Group == TargetGroup).ToArray()[0];
                if (ThisGroupReference.Rank.Permissions.Group_ManageMembers <= 0)
                {
                    NetObj.ClientObject.SendMessage("Your rank does not have permission to manage group ranks in this group.");
                    return false;
                }
                //Continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("You need to be a member of the group yourself in order to move ranks. (Group founders are excepted from this rule.)");
                return false;
            }
            #endregion

            if (TargetGroup.Ranks.IndexOf(TargetRank) == TargetGroup.Ranks.Count() - 1)
            {
                NetObj.ClientObject.SendMessage("Unable to increment Rank: \"" + TargetRank.Name + "\" in Group: \"" + TargetGroup.Name + "\" any higher. It is the highest rank already.");
                return false;
            }
            int CurrentPosition = TargetGroup.Ranks.IndexOf(TargetRank);
            int NextPosition = CurrentPosition + 1;
            Database.GroupDB.Group.Rank SwapContainer = TargetGroup.Ranks[NextPosition];
            TargetGroup.Ranks[NextPosition] = TargetRank;
            TargetGroup.Ranks[CurrentPosition] = SwapContainer;
            TargetGroup.SaveAll();
            Server.EmptyClientList.Include(NetObj).SendMessage("Rank: \"" + TargetRank.Name + "\" in Group: \"" + TargetGroup.Name + "\" incremented to position " + NextPosition.ToString() + ".");
            Server.AllClients.Except(NetObj).SendMessage("User: \"" + NetObj.UserObject.Name + "\" incremented Rank: \"" + TargetRank.Name + "\" in Group: \"" + TargetGroup.Name + "\" to position " + NextPosition.ToString() + ".");
            return true;
        }
    }
}