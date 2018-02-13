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
                #region Variables
                public string Name = "";
                public string DisplayedName = "";

                public DateTime DateCreated     = DateTime.Now;
                public DateTime DateLastModified      = DateTime.Now;

                public bool Joinable = true;
                public bool Leavable = true;

                public UserDB.User Founder = UserDB.Nobody;

                public List<Rank> Ranks = new List<Rank>();
                public Rank NoRank = new Rank();

                public string RepresentationFormat = "GUR"; //G: GroupName, U: Username, R: Rank

                //Group Specific Permissions Object.
                public Database.PermissionDB.Permission Permissions = new Database.PermissionDB.Permission();
                #endregion
            }
            public static class Strings
            {
                #region Strings
                public const string Name = "NAME";
                public const string DisplayedName = "DISPLAYEDNAME";

                public const string DateCreated = "DATECREATED";
                public const string DateLastModified = "DATELASTMODIFIED";
                public const string Joinable = "JOINABLE";
                public const string Leavable = "LEAVABLE";

                public const string Founder = "FOUNDER";

                public const string Rank = "RANK";

                //Ranks String
                //Permissions String
                #endregion
            }

            
        }
    }
}
