using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Database
    {
        public static partial class PermissionDB {
            public static partial class PermissionsCore
            {
                public static double CheckBasePermission(PermissionDB.Permission PermissionObject, string Key)
                {
                    return (Double)PermissionObject.GetType().GetField(Key).GetValue(PermissionObject);
                }

                public static bool CheckPermission(UserDB.User UserObject, string Key)
                {
                    //Logger.Console.WriteLine("Checking Permissions of User \"" + UserObject.Name + "\"...");
                    try
                    {
                        double Power = 0;
                        //Logger.Console.WriteLine("    User: " + ((Double)UserObject.Permissions.GetType().GetField(Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).GetValue(UserObject.Permissions)).ToString());
                        //Get Users BASE Permission (Specific to this user only)
                        Power += (Double)UserObject.Permissions.GetType().GetField(Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).GetValue(UserObject.Permissions);

                        //Sort through EVERY group the user is in...:
                        foreach (UserDB.User.GroupReference ThisGroup in UserObject.Groups)
                        {
                            //... And get the entire GROUPs permission.
                            //Logger.Console.WriteLine("    Group(" + ThisGroup.Group.Name + "): " + ((Double)ThisGroup.Group.Permissions.GetType().GetField(Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).GetValue(ThisGroup.Group.Permissions)).ToString());
                            Power += (Double)ThisGroup.Group.Permissions.GetType().GetField(Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).GetValue(ThisGroup.Group.Permissions);

                            //... As well as the RANKs specific permission.
                            //Logger.Console.WriteLine("    Rank(" + ThisGroup.Group.Name + "|" + ThisGroup.Rank.Name + "): " + ((Double)ThisGroup.Rank.Permissions.GetType().GetField(Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).GetValue(ThisGroup.Rank.Permissions)).ToString());
                            Power += (Double)ThisGroup.Rank.Permissions.GetType().GetField(Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase).GetValue(ThisGroup.Rank.Permissions);
                        }


                        //Logger.Console.WriteLine("");
                        if (double.IsNaN(Power))
                        {
                            Logger.Log.SystemMessage("    PermissionsCore performed a calculation for user: " + UserObject.Name + " for the permission: " + Key + " and encountered a \"NaN\" result. possibly, the user has a false and true permission override, which are conflicting.");
                            //Logger.Console.WriteLine("    OVERALL PERMISSION: FALSE(CONFLICT)");
                            return false; //abnormal...
                        }
                        if (double.IsNegativeInfinity(Power))
                        {
                            //Logger.Console.WriteLine("    OVERALL PERMISSION: FALSE(OVERRIDE)");
                            return false; //normal
                        }
                        if (double.IsPositiveInfinity(Power))
                        {
                            //Logger.Console.WriteLine("    OVERALL PERMISSION: TRUE(OVERRIDE)");
                            return true; //normal
                        }
                        if (Power >= 1)
                        {
                            //Logger.Console.WriteLine("    OVERALL PERMISSION: TRUE");
                            return true; //normal
                        }
                        if (Power < 1)
                        {
                            //Logger.Console.WriteLine("    OVERALL PERMISSION: FALSE");
                            return false; //normal
                        }

                        //Logger.Console.WriteLine("    OVERALL PERMISSION: FALSE(FAILSAFE)");
                        Logger.Log.SystemMessage("    PermissionsCore performed a calculation for user: " + UserObject.Name + " for the permission: " + Key + " and has somehow not resolved the calculation. As a failsafe, permission has been denied.");
                        return false; //HOW did the code end up here?
                    }
                    catch (Exception e)
                    {
                        //Logger.Console.WriteLine("    OVERALL PERMISSION: FALSE(ERROR!)");
                        Logger.Log.Bug(e, "    PermissionsCore performed a calculation for user: " + UserObject.Name + " for the permission: " + Key + " and has somehow encountered an excpetion. As a failsafe, permission has been denied.");
                        return false; //Failsafe.
                    }
                }
            }
        }
    }
}