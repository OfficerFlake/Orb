using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Net;
using System.Security.Cryptography;
using System.Threading;

namespace Orb
{
    public class SaltGenerator
    {
        public static string NewSalt()
        {
            string OutSalt = "";

            Random thisrandom = new Random();

            for (int i = 0; i < 16; i++)
            {
                OutSalt += "((char)" + thisrandom.Next(0, 255).ToString() + ").ToString() + ";
            }
            OutSalt = OutSalt.Remove(OutSalt.Length - 3);
            string OutputDLL = "public static partial class Database { public static string PasswordSalt = " + OutSalt + ";}";


            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "Orb.dll";

            CompilerResults r = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromSource(parameters, OutputDLL);

            //verify generation
            foreach(CompilerError ThisError in r.Errors) {
                Logger.Console.WriteLine("&c" + ThisError.ErrorText);
            }
            return OutSalt;
        }

        public static string Encrypt(string ThisString)
        {
            MD5 md5Hash = MD5.Create();
            string Output = MD5Hasher.GetMd5Hash(md5Hash, ThisString);
            if (MD5Hasher.VerifyMd5Hash(md5Hash, ThisString, Output)) return Output;
            else return null;
        }
    }

    public class MD5Hasher
    {
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        // Verify a hash against a string. 
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
