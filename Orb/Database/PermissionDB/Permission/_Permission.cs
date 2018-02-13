using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Orb
{
    public static partial class Database
    {
        public static partial class PermissionDB
        {
            public partial class Permission
            {
                #region Variables
                //Name
                //public double Default = 0;
                public double Say = 0;
                public double ManageServer = 0;
                public double Ban = 0;
                public double Mute = 0;
                public double Kick = 0;
                public double Group_ManageMembers = 0;
                public double Group_DemoteableRank = 0;
                public double Group_PromoteableRank = 0;
                #endregion

                /// <summary>
                /// Change all permissions to Positive Infintity, Granting this user override power.
                /// </summary>
                public void MakeSuper()
                {
                    foreach (var ThisField in this.GetType().GetFields())
                    {
                        ThisField.SetValue(this, double.PositiveInfinity);
                    }
                }

                public void MakeNeutral()
                {
                    foreach (var ThisField in this.GetType().GetFields())
                    {
                        ThisField.SetValue(this, 0);
                    }
                }

                public void MakeModerator()
                {
                    MakeNeutral();
                    Ban = 1;
                    Mute = 1;
                    Kick = 1;
                }
            }
            
            public static class Strings
            {
                #region Strings
                //public const string Default = "DEFAULT";
                public const string Say = "SAY";
                public const string ManageServer = "MANAGESERVER";
                public const string Ban = "BAN";
                public const string Mute = "MUTE";
                public const string Kick = "KICK";
                public const string Group_ManageMembers = "GROUP_MANAGEMEMBERS";
                public const string Group_DemoteableRank = "GROUP_DEMOTEABLERANK";
                public const string Group_PromoteableRank = "GROUP_PROMOTEABLERANK";
                #endregion
            }

            
        }
    }
}
