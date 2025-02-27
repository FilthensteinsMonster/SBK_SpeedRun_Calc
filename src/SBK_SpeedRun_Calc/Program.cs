using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SBK_SpeedRun_Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                Assembly assembly = Assembly.GetExecutingAssembly();
                string exePath = Path.GetDirectoryName(assembly.Location);
                string dir = Path.Combine(exePath, "UserInput"); 

                string configFileCon = Path.Combine(dir, "KeyBind.txt");
                string seedFileCon = Path.Combine(dir,"SeedData.txt");

                string escapeCondition = "1";

                if(!Directory.Exists(dir)){
                    throw new Exception("Error, Folder not found: " + dir);  
                } 

                ItemBox[] seedData = Parse.SeedData(seedFileCon);
                Dictionary<string, string[]> keyBinds = Parse.KeyBindConfig(configFileCon);

                Console.WriteLine(Display.KeyBinds(keyBinds));
                List<int> userInput = new List<int>();
                List<int> matches = new List<int>();

                string inputSelected = "";
                Console.WriteLine("Enter Items in the order obtained: ");
                do{
                    inputSelected = Console.ReadKey().KeyChar.ToString().ToLower();

                    if(keyBinds.Keys.Contains(inputSelected)){
                        userInput.Add(int.Parse(keyBinds[inputSelected][1]));
                        matches = Logic.SearchEntireArray(seedData, userInput.ToArray());     
                    }     

                }while(matches.Count > 1 && inputSelected != escapeCondition);

                Console.WriteLine("");
                if(matches.Count == 0){
                    Console.WriteLine("Invalid item collection, either user input error or seed data is wrong.");
                }

                int seedIndex = matches[0];
                
                Console.WriteLine("");
                Console.WriteLine("***   RNG Seed identified!   ***");
                Console.WriteLine("Press any key to incriment next value");
                Console.WriteLine("");

                do{
                    Console.WriteLine("Seed Value is: " + seedIndex);
                    Console.WriteLine("Next Red Item Is: " + Display.RedItem(seedData[seedIndex].Red));
                    Console.WriteLine("Next Blue Item Is: " + Display.BlueItem(seedData[seedIndex].Blue));

                    inputSelected = Console.ReadKey().KeyChar.ToString().ToLower();
                    seedIndex = Logic.IncrimentSeedIndex(seedIndex, seedData);
                }while(inputSelected != escapeCondition);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            } 
            finally{
                Console.WriteLine("");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}