using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Orb
{
    public static partial class Utilities
    {
        public static partial class WelcomeFile
        {
            public static void WriteDefault()
            {
                string OutputFile = "./Welcome.txt";
                if (File.Exists(OutputFile)) return;
                Utilities.IO.PrepareFile(OutputFile);
                List<String> Output = new List<String>();
                
                string OutString =
@"=======================================================================================
|                               WELCOME TO ORB TEST SERVER!                           |
|                                                                                     |
| Please notify OfficerFlake of any bugs at YSFHQ.com or email officerflake@gmail.com |
|                                                                                     |
| This Test Server is for private use, and is NOT owned or manageable by OfficerFlake |
|                                                                                     |
| If you need help with anything server related (packs, bans, etc.) please speak to   |
| the Server owner, and NOT OfficerFlake. :D                                          |
=======================================================================================";

                Output.Add(OutString);
                File.WriteAllLines(OutputFile, Output.ToList());
            }
        }
    }
}