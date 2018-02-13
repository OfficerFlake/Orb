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
        public static readonly CommandDescriptor Orb_Command_Maintenence_Group_Ranks_Add = new CommandDescriptor
        {
            _Name = "Add Rank To Group",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Add a rank to a group on the server.",
            _Usage = "Usage: /Group.<Name>.Ranks.Add <RankName>",
            _Commands = new string[] { "/Groups.*.Ranks.Add" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_Group_Ranks_Add_Method,
        };

        public static bool Orb_Command_Maintenence_Group_Ranks_Add_Method(Server.NetObject NetObj, CommandReader Command)
        {
            Database.GroupDB.Group TargetGroup = Database.GroupDB.NoGroup;
            string TargetRankName = "";
            #region FindTargetGroup
            if (Command._CmdElements()[1] == "-")
            {
                if (NetObj.CommandHandling.PreviousGroup == Database.GroupDB.NoGroup)
                {
                    NetObj.ClientObject.SendMessage("No previous groups iterated over.");
                    return false;
                }
                else TargetGroup = NetObj.CommandHandling.PreviousGroup;
            }
            else
            {
                if (Database.GroupDB.FindGroup(Command._CmdElements()[1]) == Database.GroupDB.NoGroup)
                {
                    NetObj.ClientObject.SendMessage("Group not found: \"" + Command._CmdElements()[1] + "\".");
                    return false;
                }
                TargetGroup = Database.GroupDB.FindGroup(Command._CmdElements()[1]);
            }
            #endregion
            #region FindTargetRank
            if (Command._CmdArguments.Count() < 1)
            {
                NetObj.ClientObject.SendMessage("No rank specified to be added to the group.");
                return false;
            }
            else
            {
                TargetRankName = Command._CmdArguments[1].ToUpperInvariant();
                if (TargetGroup.Ranks.Count() == 0) { }
                else if (TargetGroup.Ranks.Select(x => x.Name.ToUpperInvariant()).Contains(TargetRankName))
                {
                    NetObj.ClientObject.SendMessage("Rank: \"" + TargetRankName + "\" already exists in Group: \"" + TargetGroup.Name + "\".");
                    return false;
                }
            }
            #endregion
            #region SuperUser Override
            if (NetObj.UserObject == Database.UserDB.SuperUser || NetObj.UserObject == TargetGroup.Founder)
            {
                //continue
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
                //continue
            }
            else
            {
                NetObj.ClientObject.SendMessage("You need to be a member of the group yourself in order to add ranks. (Group founders are excepted from this rule.)");
                return false;
            }
            #endregion

            if (TargetGroup.Ranks.Select(x => x.Name).Contains(TargetRankName))
            {
                NetObj.ClientObject.SendMessage("Rank: \"" + TargetRankName + "\" is already in Group: \"" + TargetGroup.Name + "\".");
                return false;
            }
            TargetGroup.NewRank(TargetRankName);
            TargetGroup.SaveAll();
            Server.EmptyClientList.Include(NetObj).SendMessage("You added a new Rank: \"" + TargetRankName + "\" to Group: \"" + TargetGroup.Name + "\".");
            Server.AllClients.Except(NetObj).SendMessage("New Rank: \"" + TargetRankName + "\" was added to Group: \"" + TargetGroup.Name + "\" by \"" + NetObj.UserObject.Name + "\".");
            return true;
        }
    }
}