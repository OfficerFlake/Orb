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
        public static readonly CommandDescriptor Orb_Command_Flight_Form = new CommandDescriptor
        {
            _Name = "Form",
            _Version = 1.0,
            _Date = new DateTime(2014, 06, 01),
            _Author = "OfficerFlake",
            _Descrption = "Chooses A Formation Target based off Vehicle ID.",
            _Usage = "Usage: /Flight.Form <ID>",
            _Commands = new string[] { "/Flight.Form", "/Form" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Flight_Form_Method,
        };

        public static bool Orb_Command_Flight_Form_Method(Server.NetObject NetObj, CommandReader Command)
        {
            if (Command._CmdArguments.Count() < 1)
            {
                NetObj.ClientObject.SendMessage("&eFormat incorrect: Use instead: \"/Form ID\"!");
                return false;
            }

            uint ID = NetObj.Vehicle.ID;

            try
            {
                ID = UInt32.Parse(Command._CmdArguments[0]);
            }
            catch
            {
                NetObj.ClientObject.SendMessage("&eFormat incorrect: Be sure you are using an integer value!");
                return false;
            }
            if (NetObj.Vehicle.ID == 0)
            {
                NetObj.ClientObject.SendMessage("&eYou need to be flying in order to choose a formation target!");
                return false;
            }
            NetObj.Vehicle.FormTarget = ID;
            NetObj.ClientObject.SendMessage("&eSucessfully set Formation Target to \"" + NetObj.Vehicle.FormTarget.ToString() + "\".");
            return true;
        }
    }
}