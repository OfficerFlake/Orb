using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Net;
using System.IO;

namespace Orb
{
    public static partial class Database
    {
        public static string PasswordSalt = "1234567890ABCDEF";
        public static bool NewSaltGenerated = false; //if changed to true, ALL passwords in the database will be removed!

        public static void LoadSalt()
        {
            if (File.Exists("Orb.DLL"))
            {
                Logger.Console.WriteLine("&5    Salt Loaded.");
                PasswordSalt = Assembly.LoadFrom("Orb.dll").GetType("Database").GetField("PasswordSalt").GetValue(null).ToString();
            }
            else
            {
                Logger.Console.WriteLine("&d    Orb.DLL Not Found: New Salt Generated.");
                PasswordSalt = SaltGenerator.NewSalt();
                NewSaltGenerated = true;
            }
        }
    }
}
