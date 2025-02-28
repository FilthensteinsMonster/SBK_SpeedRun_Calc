using System.Text;

namespace SBK_SpeedRun_Calc{

    static class Display{
        
        public static void KeyBinds(Dictionary<string, string[]> config){
            bool fistRed = false;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("  App Keys");
            sb.AppendLine("  ------------------------");
            sb.AppendLine("   " + "1" + "  " + "Terminate Program");
            sb.AppendLine("   " + "0" + "  " + "Reset Program");
            
            sb.AppendLine("");
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
                    sb.AppendLine(BlueItem(config[key][1]));
                } else if(type == "red"){
                    sb.AppendLine(RedItem(config[key][1]));
                } else {
                    throw new Exception("Error, Item config was neither a red/blue box, please review keybind.txt file.");
                }
            }

            sb.AppendLine("");
            sb.AppendLine("  ------------------------");

            Console.WriteLine(sb.ToString());
        }

        public  static string RedItem(int n){
            return RedItem(n.ToString());
        }
        static string RedItem(string n){
            switch(n){
                case "2": return "Hand";
                case "3": return "Umbrealla/Parachute";
                case "4": return "Ice";
                case "5": return "Snowman"; 
                case "9": return "Bomb";
                default: throw new Exception("Invalid item mapping id passed, value passed is: " + n + " expected were 2,3,4,5,9");
            }
        }

        public static string BlueItem(int n){
            return BlueItem(n.ToString());
        }
        static string BlueItem(string n){
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

        public static void SeedFound(){
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("***   RNG Seed identified!   ***");
            Console.WriteLine("Press any key to incriment next value");
            Console.WriteLine("");
        }

        public static void SeedInfo(int seedIndex, ItemBox[] seedData, Dictionary<string,string> notice){
            Console.WriteLine("Seed Value is: " + seedIndex);
            Console.WriteLine("Next Red Item Is: " + RedItem(seedData[seedIndex].Red));
            Console.WriteLine("Next Blue Item Is: " + BlueItem(seedData[seedIndex].Blue));
            Console.WriteLine("Next Fan: " + Logic.NextBlueItem(seedData, seedIndex, 1));
            Console.WriteLine("Next Board: " + Logic.NextBlueItem(seedData, seedIndex, 6));
            foreach(string key in notice.Keys){
                int target = int.Parse(notice[key]);
                Console.WriteLine(key + ": " + Logic.RollIndexDelta(seedData.Length, target, seedIndex));
            } 
        }

        public static void ErrorMssg(Exception ex, string logDir){
            Console.WriteLine("");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("");
            Console.WriteLine("Sorry you've encountered an error. :(");
            Console.WriteLine("Please review the messages above, it might explain why the app broke.");
            Console.WriteLine("Log files of user input exists at: " + logDir);
            Console.WriteLine("Reach out for help and send the log file if you believe the tool isn't working.");
            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }

        public static void InvalidUserInput(){
            Console.WriteLine("");
            Console.WriteLine("Invalid item order, either user input error or seed data is wrong.");
            Console.WriteLine("Resetting the app.");
            Console.WriteLine("");
        }

    }
}