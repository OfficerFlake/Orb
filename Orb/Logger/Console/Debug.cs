using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Orb
{
    public static partial class Logger
    {
        public static partial class Console
        {
            public static void Debug(string MessageIn)
            {
                #if DEBUG
                Logger.Console.WriteLine(MessageIn);
                #endif
            }
            public static void Debug()
            {
                #if DEBUG
                Logger.Console.WriteLine();
                #endif
            }
        }
    }
}
