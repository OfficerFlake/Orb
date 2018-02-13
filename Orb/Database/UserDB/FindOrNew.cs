using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orb
{
    public static partial class Database
    {
        public static partial class UserDB
        {
            public static User FindOrNew(string Name)
            {
                if (Find(Name) == Nobody) return New(Name);
                else return Find(Name);
            }
        }
    }
}