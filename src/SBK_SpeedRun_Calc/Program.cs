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
            Assembly assembly = Assembly.GetExecutingAssembly();
            string exePath = Path.GetDirectoryName(assembly.Location);
            string dir = Path.Combine(exePath, "UserInput"); 
            string logDir = Path.Combine(exePath, "Logs");
            string configFileCon = Path.Combine(dir, "KeyBind.txt");
            string seedFileCon = Path.Combine(dir,"SeedData.txt");
            string noticeFileCon = Path.Combine(dir,"CustomNotice.txt");
            string logFileCon = Path.Combine(logDir, DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("hhmmss") + "_log.txt");
            string escapeCondition = "1";
            string resetCondition = "0";
            Logger Logs = new Logger(logFileCon);
            
            try{
                if(!Directory.Exists(dir)){
                    throw new Exception("Error, Folder not found: " + dir);  
                } 

                if(!Directory.Exists(logDir)){
                    Directory.CreateDirectory(logDir);
                } 

                ItemBox[] seedData = Parse.SeedData(seedFileCon);
                Dictionary<string, string[]> keyBinds = Parse.KeyBindConfig(configFileCon);
                Dictionary<string, string> notice = Parse.NoticeConfig(noticeFileCon);

                string inputSelected = "";
                do{
                    Display.KeyBinds(keyBinds);
                    List<int> userInputVal = new List<int>();
                    List<string> userInputColor = new List<string>();
                    List<int> matches = new List<int>();

                    Console.WriteLine("Enter Items in the order obtained: ");
                    do{
                        inputSelected = Console.ReadKey().KeyChar.ToString().ToLower();
                        Logs.Add(inputSelected);

                        if(keyBinds.Keys.Contains(inputSelected)){                                               
                            userInputColor.Add(keyBinds[inputSelected][0]);
                            userInputVal.Add(int.Parse(keyBinds[inputSelected][1]));
                            matches = Logic.SearchEntireArray(seedData, userInputVal.ToArray(), userInputColor.ToArray());    

                            if(matches.Count == 0){
                                Display.InvalidUserInput();
                                inputSelected = resetCondition;
                            } 
                        }     

                    }while(matches.Count >= 2 && inputSelected != escapeCondition && inputSelected != resetCondition);

                    if(inputSelected != escapeCondition && inputSelected != resetCondition){

                        int seedIndex = matches[0];                
                        Display.SeedFound();

                        do{
                            seedIndex = Logic.IncrimentSeedIndex(seedIndex, seedData);
                            Display.SeedInfo(seedIndex, seedData, notice);
                            inputSelected = Console.ReadKey().KeyChar.ToString().ToLower();
                            Logs.Add(inputSelected);
                            Console.WriteLine("");
                        }while(inputSelected != escapeCondition && inputSelected != resetCondition);
                    }

                }while(inputSelected != escapeCondition);
            } 
            catch (Exception ex)
            {
                Logs.Add(ex);
                Display.ErrorMssg(ex, logDir);
            } 
            finally{
                Logs.WriteLogs();
            }
        }
    }
}