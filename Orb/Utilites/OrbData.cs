using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Orb
{
    public static class Version
    {
        public static string OfficerFlakeRainbowName = "&e=&6=&4= &cO&ef&af&2i&3c&1e&9r&bF&dl&5a&4k&ce &4=&6=&e=";
        public static string OfficerFlakeGoldenName = "&e=&6=&4= &6OfficerFlake &4=&6=&e=";
        public static string OfficerFlakeTealName = "&3OfficerFlake";
        public static int MainVersion = 1;
        public static double SubVersion = 0.0;
        public static bool TestVersion = true;
        public static string TestVersionID = "(Alpha-Test_29Oct13)";
        public static string FullName
        {
            get
            {
                string Output = "Orb " + (string.Format("{0:0.0}",(Math.Truncate((double)MainVersion + (double)SubVersion)*10)/10));
                if (TestVersion)
                {
                    Output += TestVersionID;
                }
                return Output;
            }
        }

        public static void WriteNewToOrbHelpFile()
        {
            #region OutPut
            string OutString =

@"Welcome to Orb for YSFlight!
============================

Thanks very much for taking your time to download this server software!

This short guide will take you through the quick steps needed to set up your orb server:

================

1) Be sure your vanilla server works to begin with. Run your normal YSFlight server, add it to the YSFHQ server list if wanted and confirm that users can connect to it.

2) Set your vanilla server to run on a different port then what you want Orb server to run on. for example, if you want people to connect to orb on 7915 (standard YSF port) then you need to have vanilla YSF run on a different port. This is done by changing the YSFlight Server Options port, then starting the server.

3) Edit Settings.Dat with a text editor. find the line that says 'ServerPort' and change the number to the port you chose for YSFlight Server. the default orb will use is 7914.

4) Ensure your chosen orb port is the same port that the vanilla ysflight server would have used. by default, this is 7915.

5) That's it! start the orb server, which should run on the orb port, and be sure your vanilla server is running on the server port. Now, just connect a ysfclient to 127.0.0.1 (your local internal ip address) and to the orb server port you specified. Orb will do the rest!

================

Troubleshooting:

Q: My Orb window has nothing in it! It's just black!

A: Your resolution or environment may not support the GUI mode. Change GUI Mode in the Settings.Dat to 'False'.

Q: I keep getting a 'Can't connect to the server' error in ysflight!

A: Be sure that orb has started correctly, and that your orb port in the Settings.Dat is the port you are connecting to in YSF. Also be sure your OS doesn't block used ports by nature.

Q: I get a message 'Could not reach the server' and then get disconnected / Orb shows a message 'Connecting client was disconnected as the host service is unreachable!'.

A: Is your vanilla YSFlight server running? And is it running on the port specified by the server port setting in the Settings.Dat?

Q: I can't log in! Orb checks with YSFHQ but fails, and I get denied!

A: The YSFHQ authentication procedure is still experimental at this stage. Any user with less then 10 logins will be allowed connection to the server regardless of the approval. It is thus reccommended users set a password fallback should their IP address ever not match the YSFHQ records. If you still encounter errors, please disable the module.

Q: No commands are working for my YSF client!

A: Which YSF version are you using? Older versions do not support the sending of long usernames (over 15 char). Please upgrade to the latest version as old YSF builds are not supported!

Q: I found a bug!

A: Is it accurately reproducable? tell OfficerFlake @ YSFHQ.com what happens and how. He'll be able to fix it quickly. Also  check your log files, they may provide a better insight.

Q: So the server is set up, now what?

A: Type /help or /commands in either Orb or YSFlight, and take it from there!";

            #endregion
            string OutFile = "./README_New_Server.txt";
            List<String> Output = new List<String>();
            Output.Add(OutString);
            Utilities.IO.PrepareFile(OutFile);
            File.WriteAllLines(OutFile, Output);
        }

        public static void TestVersionWarning()
        {
            Logger.Console.WriteLine("Welcome to Orb Server!");
            Logger.Console.WriteLine("======================");
            Logger.Console.WriteLine("You are using a TEST Version of Orb [" + Orb.Version.FullName + "].");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("You may encounter errors, including but not limited to errors that could CRASH YOUR USER DATABASE!");
            Logger.Console.WriteLine("Make frequent backups, and, prefereably, do NOT run test version on a servers primary installation.");
            Logger.Console.WriteLine();
        }

        public static void TestVersionWarning42s()
        {
            Logger.Console.WriteLine("Welcome to Orb Server!");
            Logger.Console.WriteLine("======================");
            Logger.Console.WriteLine("You are using a TEST Version of Orb [" + Orb.Version.FullName + "].");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("You may encounter errors or disconnects, but for the most part, This program is quite stable.");
            Logger.Console.WriteLine("Please only use a username of 15 characters or less. Nothing relys on this at the moment, but you will encounter issues in later versions.");
            Logger.Console.WriteLine("The Service will run on 127.0.0.1:7915. Please just connect your ysflight client, like normal to 127.0.0.1 and this service will connect you to 42south server through orb.");
            Logger.Console.WriteLine("Please note: The only advantage of using this test version is to get a new chat message type :)");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("Thanks for Your Support!");
            Logger.Console.WriteLine();
        }

        public static void TestVersionWarningADE()
        {
            Logger.Console.WriteLine("&5Welcome to Orb Server!");
            Logger.Console.WriteLine("&5======================");
            Logger.Console.WriteLine("&9You are using a Development Version of Orb");
            Logger.Console.WriteLine("&9[" + Orb.Version.FullName + "].");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&9This Development Version is for the EXCLUSIVE use of OfficerFlake");
            Logger.Console.WriteLine("&9    Orb Founder/Administrator/Developer ONLY.");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&cWARNING: &eDO NOT RELEASE DEVELOPMENT ENVIRONMENT APPS TO THE PUBLIC!");
            Logger.Console.WriteLine();
        }

        public static void TestVersionWarningOrbTestGroup()
        {
            Logger.Console.WriteLine("&5Welcome to Orb Server!");
            Logger.Console.WriteLine("&5======================");
            Logger.Console.WriteLine("&9You are using a &dPre-Alpha &9Test Version of Orb");
            Logger.Console.WriteLine("&9\t [" + Orb.Version.FullName + "&9].");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&9This Development Version is for the EXCLUSIVE use of ");
            Logger.Console.WriteLine("&9\t Privelleged Orb Developers ONLY.");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&cWARNING: &eDO &cNOT&e RELEASE DEVELOPMENT ENVIRONMENT APPS TO THE PUBLIC!");
            Logger.Console.WriteLine("&e         &eDOING SO WILL SEE YOU &cREMOVED &eFROM THE TEST TEAM!");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&bGot any questions? Please speak to " + OfficerFlakeGoldenName + "&b and he'll help you out!");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&dTHANK YOU &bFor testing &5Ørb# &bServer for &fYS&9Flight&b! I hope you enjoy it! :D");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&5======================");
        }

        public static void TestVersionWarningAlpha()
        {
            Logger.Console.WriteLine("&5Welcome to Orb Server!");
            Logger.Console.WriteLine("&5======================");
            Logger.Console.WriteLine("&9You are using a &dAlpha &9Test Version of Orb");
            Logger.Console.WriteLine("&9\t [" + Orb.Version.FullName + "&9].");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&9This Version &dWILL&9 have bugs in it!");
            Logger.Console.WriteLine("&9Your perserverance in testing despite these issues really helps me!");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&bGot any questions? Please speak to " + OfficerFlakeTealName + "&b and he'll help you out!");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&dTHANK YOU &bFor testing &5Ørb# &bServer for &fYS&9Flight&b! I hope you enjoy it! :D");
            Logger.Console.WriteLine("");
            Logger.Console.WriteLine("&5======================");
        }
    }
}