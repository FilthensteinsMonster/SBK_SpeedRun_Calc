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
            try{
                Assembly assembly = Assembly.GetExecutingAssembly();
                string exePath = Path.GetDirectoryName(assembly.Location);
                string dir = Path.Combine(exePath, "UserInput"); 

                string configFileCon = Path.Combine(dir, "KeyBind.txt");
                string seedFileCon = Path.Combine(dir,"SeedData.txt");

                string escapeCondition = ".";

                if(!Directory.Exists(dir)){
                    throw new Exception("Error, Folder not found: " + dir);  
                } 

                int[] seedData = ParseSeedData(seedFileCon);

                List<int> userInput = new List<int>();
                List<int> matches = new List<int>();
                string inputSelected = "";
                do{
                    Console.WriteLine("Enter number: ");
                    inputSelected = Console.ReadKey().KeyChar.ToString();
                    userInput.Add(int.Parse(inputSelected));
                    matches = SearchEntireArray(seedData, userInput.ToArray());
                }while(matches.Count > 1 && inputSelected != escapeCondition);

                int seedIndex = matches[0];

                do{
                    Console.WriteLine("Next Seed Value Is: " + seedData[seedIndex]);
                    Console.WriteLine("Press any key to incriment next value");
                    Console.ReadKey();
                    seedIndex += 1;
                }while(inputSelected != escapeCondition);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            } 
            finally{
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// search the array to see if it contains another, return the index of next elements. Bug exists, doesn't include looping
        /// </summary>
        /// <param name="seed">data set of expected values</param>
        /// <param name="input">real time values</param>
        /// <returns></returns>
        static List<int> SearchEntireArray(int[] seed, int[] input){
            List<int> seedIndexs = new List<int>();

            int start = input.Length;

            for(int n = start; n < seed.Length - start; n++){
                int match = -1;
                bool found = TrySearchArrayInstance(seed, input, n, out match);

                if(found && match != -1){
                    seedIndexs.Add(match);
                }
            }

            return seedIndexs;
        }

        static bool TrySearchArrayInstance(int[] seed, int[] input, int index, out int match){
            match = -1;

            for(int n = 0; n < input.Length; n++){
                if(seed[index + n] != input[n]){
                    return false;
                }
            }

            match = index + input.Length;
            return true;
        }

        static int[] ParseSeedData(string filePath){
            List<int> result = new List<int>();

            if(!File.Exists(filePath)){
                throw new Exception("Error: Seed file not found: " + filePath);
            }

            using(StreamReader reader = new StreamReader(filePath)){
                while(!reader.EndOfStream){
                    string line = reader.ReadLine().Trim();

                    if(!string.IsNullOrWhiteSpace(line)){
                        result.Add(int.Parse(line));
                    }
                }
            }

            return result.ToArray();
        }
    }
}