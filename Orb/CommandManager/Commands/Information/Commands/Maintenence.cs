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
        public static readonly CommandDescriptor Orb_Command_Information_Maintenence = new CommandDescriptor
        {
            _Name = "Maintenence Command List",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "List the Maintenence Commands available on the server.",
            _Usage = "Usage: /Commands.Maintenence",
            _Commands = new string[] { "/Commands.Maintenence" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Information_Maintenence_Method,
        };

        public static bool Orb_Command_Information_Maintenence_Method(Server.NetObject NetObj, CommandReader Command)
        {
            NetObj.ClientObject.SendMessage("&eMaintenence Commands:");
            NetObj.ClientObject.SendMessage("&a    /Groups&e                               Shows the Groups on the server.");
            NetObj.ClientObject.SendMessage("&a    /Groups.Add (Name)&e                    Adds a new group to the server.");
            NetObj.ClientObject.SendMessage("&a    /Groups.Remove (Name)&e                 Removes a group from the server.");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Add (Name)&e             Adds a user to a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Remove (Name)&e          Removes a user from a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Founder (Name)&e         Transfers Ownership of a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Ranks.List&e             Shows Ranks of a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Ranks.Add (Name)&e       Adds a new Rank to a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Ranks.Remove (Name)&e    Removes a Rank from a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Ranks.<Name>.MoveUp&e    Orders a Rank Higher in a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Ranks.<Name>.MoveDown&e  Orders a Rank Lower in a group");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Promote (Name)&e         Increases the rank of a user by one.");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Demote (Name)&e          Decreases the rank of a user by one.");
            NetObj.ClientObject.SendMessage("&a    /Groups.<NAME>.Rank (Name) (NewRank)&e  Shows the rank of, or re-ranks, a user.");
            return true;
        }
    }
}