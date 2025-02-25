using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace SBK_SpeedRun_Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string exePath = Path.GetDirectoryName(assembly.Location);
            string dir = Path.Combine(exePath, "UserInput"); 

            string configFile = "KeyBind.txt";
            string dataSeed = "SeedData.txt";

            if(!Directory.Exists(dir)){
                Console.WriteLine("Error, folder not found: " + dir);
            } else {
                Console.WriteLine("files found");
            }
            
            Console.WriteLine("Prog complete. press any key to exit");
            Console.ReadKey();
        }
    }
}