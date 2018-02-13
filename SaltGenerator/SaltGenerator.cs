using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Net;
using System;
using System.Threading;

namespace SaltGenerator
{
    class SaltGenerator
    {
        static void Main(string[] args)
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
                Console.WriteLine(ThisError.ErrorText);
            }
            Console.WriteLine("New Salt Generated: {0}", Assembly.LoadFrom("Orb.dll").GetType("Database").GetField("PasswordSalt").GetValue(null));
            Thread.Sleep(10000);
            Console.ReadKey();
        }
    }
}
