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

                public List<RankDB.Rank> Ranks = new List<RankDB.Rank>();
                //Permissions object...
                #endregion
            }
            public static class Strings
            {
                #region Strings
                public const string Name = "GROUPNAME";
                public const string DisplayedName = "DISPLAYEDNAME";

                public const string DateCreated = "DATECREATED";
                public const string DateLastModified = "DATELASTMODIFIED";
                public const string Joinable = "JOINABLE";
                public const string Leavable = "LEAVABLE";

                public const string Founder = "FOUNDER";

                //Ranks String
                //Permissions String
                #endregion
            }

            
        }
    }
}
