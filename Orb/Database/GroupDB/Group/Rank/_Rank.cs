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
                public partial class Rank
                {
                    #region Variables
                    public string Name = "";
                    public string DisplayedName = "";

                    //Rank specific Permissions Object.
                    public Database.PermissionDB.Permission Permissions = new Database.PermissionDB.Permission();
                    #endregion

                    public partial class Strings
                    {
                        #region Strings
                        public const string Name = "RANKNAME";
                        public const string DisplayedName = "DISPLAYEDNAME";
                        #endregion
                    }
                }
            }
        }
    }
}
