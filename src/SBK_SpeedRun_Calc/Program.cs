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

                string escapeCondition = ".";

                if(!Directory.Exists(dir)){
                    throw new Exception("Error, Folder not found: " + dir);  
                } 

                int[] seedData = ParseSeedData(seedFileCon);
                Dictionary<string, string[]> keyBinds = ParseKeyBindConfig(configFileCon);

                Console.WriteLine(DisplayKeyBinds(keyBinds));
                
                // int[] Seed = new int[]{3,5,7,7,8,6,1,9,7,8,2,9,1};
                // int[] A = new int[]{9,1,3};
                // int[] B = new int[]{7,8};

                // var Bres = SearchEntireArray(Seed, B);
                // var Ares = SearchEntireArray(Seed, A);

                // var db = "Debug";

                List<int> userInput = new List<int>();
                List<int> matches = new List<int>();

                string inputSelected = "";
                Console.WriteLine("Enter Items in the order obtained: ");
                do{
                    inputSelected = Console.ReadKey().KeyChar.ToString();
                    userInput.Add(int.Parse(keyBinds[inputSelected][1]));
                    matches = SearchEntireArray(seedData, userInput.ToArray());                
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
                    Console.WriteLine("Next Item Is: " + MapBlueBox(seedData[seedIndex]));
                    Console.ReadKey();
                    seedIndex = IncrimentSeedIndex(seedIndex, seedData);
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

        static string DisplayKeyBinds(Dictionary<string, string[]> config){
            bool fistRed = false;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("  Blue Items");
            sb.AppendLine("  ------------------------");
            
            var keys = config.Keys;
            foreach(string key in keys){
              
                string type = config[key][0];
                if(type == "red" && !fistRed){
                    fistRed = true;
                    sb.AppendLine("");
                    sb.AppendLine("  Red Items");
                    sb.AppendLine("  ------------------------");
                }

                sb.Append("   " + key + "  ");
                if(type == "blue"){
                    sb.AppendLine(MapBlueBox(config[key][1]));
                } else if(type == "red"){
                    sb.AppendLine(MapRedBox(config[key][1]));
                }
            }

            sb.AppendLine("");
            sb.AppendLine("  ------------------------");

            return sb.ToString();
        }

        static string MapRedBox(int n){
            return MapRedBox(n.ToString());
        }
        static string MapRedBox(string n){
            switch(n){
                case "2": return "Hand";
                case "3": return "Umbrealla/Parachute";
                case "4": return "Ice";
                case "5": return "Snowman"; 
                case "9": return "Bomb";
                default: throw new Exception("Invalid item mapping id passed, value passed is: " + n + " expected were 2,3,4,5,9");
            }
        }

        static string MapBlueBox(int n){
            return MapBlueBox(n.ToString());
        }
        static string MapBlueBox(string n){
            switch(n){
                case "1": return "Fan";
                case "2": return "Ghost";
                case "3": return "Pan";
                case "4": return "Rock";
                case "5": return "Mouse"; 
                case "6": return "Board";
                default: throw new Exception("Invalid item mapping id passed, value passed is: " + n + " expected were 1,2,3,4,5,6");
            }
        }

        static int IncrimentSeedIndex(int current, int[] seed){
            int max = seed.Length - 1;
            int next = current + 1;
            if(next > max){
                next = 0;
            }
            return next;
        }
        static List<int> SearchEntireArray(int[] seed, int[] input){
            List<int> seedIndexs = new List<int>();

            int start = input.Length;

            for(int n = start; n < seed.Length; n++){
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

            int max = seed.Length;
            int adjIndex = -1;

            for(int n = 0; n < input.Length; n++){

                int idx = index + n;
                int maxL = max - 1;
                int overflow = maxL / idx;
                int loopIdx = (idx % maxL) - 1;

                bool hasLoop = (overflow == 0) ? true : false;
                adjIndex = (hasLoop == true) ? loopIdx : idx;
                
                // debug
                // Console.WriteLine("index: " + index +  " hasLoop: " + hasLoop + " idx,maxL,over,loopIdx: " + idx + "," + maxL + "," + overflow + "," + loopIdx +  " adj: " + adjIndex);

                if(seed[adjIndex] != input[n]){
                    return false;
                }
            }

            match = adjIndex;
            return true;
        }

        static Dictionary<string, string[]> ParseKeyBindConfig(string filePath){
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();

            if(!File.Exists(filePath)){
                throw new Exception("Error: Key Bind file not found: " + filePath);
            }

            using(StreamReader reader = new StreamReader(filePath)){
                while(!reader.EndOfStream){
                    string line = reader.ReadLine().Trim();

                    if(!string.IsNullOrWhiteSpace(line)){
                        string[] values = line.Split(',');
                        string boxType = values[0].Trim();
                        string key = values[1].Trim();
                        string index =  values[2].Trim();

                        if(!result.ContainsKey(key)){
                            result.Add(key, new string[]{boxType, index});
                        }
                    }
                }
            }

            return result;
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