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
        public static readonly CommandDescriptor Orb_Command_Maintenence_User_Password_Change = new CommandDescriptor
        {
            _Name = "Change Password",
            _Version = 1.0,
            _Date = new DateTime(2013, 10, 01),
            _Author = "OfficerFlake",
            _Descrption = "Changes your log in password",
            _Usage = "Usage: /Password.Change",
            _Commands = new string[] { "/Password.Change" },

            //The method naming format should follow the standard packaging protocal!
            //This is to ensure no methods are overwritten by other users!
            //Please use a Namespace like method!
            //Namespace: <YourName/Repository>_<MethodType>_<MethodName>
            //The Handler should be similar, but end in "_Method"!
            _Handler = Orb_Command_Maintenence_User_Password_Change_Method,
        };

        public static bool Orb_Command_Maintenence_User_Password_Change_Method(Server.NetObject NetObj, CommandReader Command)
        {
            if (NetObj.UserObject.IsUtilityUser())
            {
                NetObj.ClientObject.SendMessage("This Command is not availabile from the Server Console.");
                return false;
            }
            NetObj.ClientObject.SendMessage("Password Change Mode Initialised. Please type \"/EXIT\" at any time to exit this interface.");
            if (NetObj.UserObject.Password == "")
            {
                NetObj.ClientObject.SendMessage("You do not have a password set at the moment.");
            }
            else
            {
                #region OldPass
                NetObj.ClientObject.SendMessage("Please Verify Your Old Password: ");
                while (true)
                {
                    AutoResetEvent WaitForMessage = new AutoResetEvent(false);
                    NetObj.TextWaiters.Add(WaitForMessage);
                    if (!WaitForMessage.WaitOne(30000))
                    {
                        NetObj.ClientObject.SendMessage("Still waiting for a response for the Password Changing Module.");
                        NetObj.ClientObject.SendMessage("Please Enter a Password, or type \"/EXIT\" to leave the Module.");
                        NetObj.ClientObject.SendMessage("You will be automatically removed in 30 Seconds.");
                        if (!WaitForMessage.WaitOne(30000))
                        {
                            NetObj.ClientObject.SendMessage("Leaving the Password Changing Module...");
                            NetObj.TextWaiters.RemoveAll(x => x == WaitForMessage);
                            return false;
                        }
                    }
                    string ThisMessage = NetObj.TextInput;
                    if (ThisMessage.ToUpper() == "/EXIT")
                    {
                        NetObj.ClientObject.SendMessage("Leaving the Password Change Mode...");
                        return false;
                    }
                    if (SaltGenerator.Encrypt(ThisMessage) == NetObj.UserObject.Password)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000); //This is to stop brute force bots from breaking or lagging the server out!
                        NetObj.ClientObject.SendMessage("Password incorrect.");
                        NetObj.ClientObject.SendMessage("Leaving the Password Change Mode...");
                        return false;
                    }
                }
                #endregion
            }
            string NewPass = "";
            #region NewPass1
            NetObj.ClientObject.SendMessage("Please Enter Your New Password: ");
            while (true)
            {
                AutoResetEvent WaitForMessage = new AutoResetEvent(false);
                NetObj.TextWaiters.Add(WaitForMessage);
                if (!WaitForMessage.WaitOne(30000))
                {
                    NetObj.ClientObject.SendMessage("Still waiting for a response for the Password Changing Module.");
                    NetObj.ClientObject.SendMessage("Please Enter a Password, or type \"/EXIT\" to leave the Module.");
                    NetObj.ClientObject.SendMessage("You will be automatically removed in 30 Seconds.");
                    if (!WaitForMessage.WaitOne(30000))
                    {
                        NetObj.ClientObject.SendMessage("Leaving the Password Changing Module...");
                        NetObj.TextWaiters.RemoveAll(x => x == WaitForMessage);
                        return false;
                    }
                }
                string ThisMessage = NetObj.TextInput;
                if (ThisMessage.ToUpper() == "/EXIT")
                {
                    NetObj.ClientObject.SendMessage("Leaving the Password Change Mode...");
                    return false;
                }
                if (ThisMessage == "")
                {
                    NewPass = "";
                }
                else
                {
                    NewPass = SaltGenerator.Encrypt(ThisMessage);
                }
                break;
            }
            #endregion
            #region NewPass2
            NetObj.ClientObject.SendMessage("Please Verify Your New Password: ");
            while (true)
            {
                AutoResetEvent WaitForMessage = new AutoResetEvent(false);
                NetObj.TextWaiters.Add(WaitForMessage);
                if (!WaitForMessage.WaitOne(30000))
                {
                    NetObj.ClientObject.SendMessage("Still waiting for a response for the Password Changing Module.");
                    NetObj.ClientObject.SendMessage("Please Enter a Password, or type \"/EXIT\" to leave the Module.");
                    NetObj.ClientObject.SendMessage("You will be automatically removed in 30 Seconds.");
                    if (!WaitForMessage.WaitOne(30000))
                    {
                        NetObj.ClientObject.SendMessage("Leaving the Password Changing Module...");
                        NetObj.TextWaiters.RemoveAll(x => x == WaitForMessage);
                        return false;
                    }
                }
                string ThisMessage = NetObj.TextInput;
                if (ThisMessage.ToUpper() == "/EXIT")
                {
                    NetObj.ClientObject.SendMessage("Leaving the Password Change Mode...");
                    return false;
                }
                if (ThisMessage == "" && NewPass == "")
                {
                    NetObj.UserObject.Password = "";
                    break;
                }
                else if (SaltGenerator.Encrypt(ThisMessage).ToUpper() == NewPass.ToUpper())
                {
                    NetObj.UserObject.Password = SaltGenerator.Encrypt(ThisMessage);
                    break;
                }
                else
                {
                    NetObj.ClientObject.SendMessage("Passwords do not match.");
                    NetObj.ClientObject.SendMessage("Leaving the Password Change Mode...");
                    return false;
                }
            }
            #endregion
            #region PasswordUpdated
            NetObj.UserObject.SaveAll();
            NetObj.ClientObject.SendMessage("Your password has been changed successfully.");
            if (!NetObj.UserObject.UsePassword) NetObj.ClientObject.SendMessage("Remember to use \"/ENABLEPASSWORD\" to allow password authentication fallback!");
            NetObj.ClientObject.SendMessage("Leaving the Password Changing Module...");
            return true;
            #endregion
        }
    }
}