using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Orb
{
    public static partial class Commands
    {
        public static void LoadAll() 
        {
            //Logger.Console.WriteLine("&bLoadingCommands...");
            foreach (var ThisCmdDesciptorRaw in typeof(Commands).GetFields().Where(x => x.FieldType == typeof(CommandDescriptor)))
            {
                try
                {
                    CommandDescriptor ThisCmdDescriptor = (CommandDescriptor)ThisCmdDesciptorRaw.GetValue(ThisCmdDesciptorRaw.FieldType); //(CommandDescriptor)Convert.ChangeType(ThisCmdDesciptorRaw.FieldType, typeof(CommandDescriptor));
                    //Logger.Console.WriteLine("Found: " + ThisCmdDescriptor._Name);
                    Register(ThisCmdDescriptor);
                }
                catch
                {
                    Logger.Console.WriteLine("        Failed: " + ThisCmdDesciptorRaw.Name);
                    continue;
                }
            }
            //Logger.Console.WriteLine("&bLoadingComplete.");
        }
    }
}