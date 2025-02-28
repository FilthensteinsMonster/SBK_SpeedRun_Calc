

namespace SBK_SpeedRun_Calc{
    class Logger{
        private readonly string LogFileCon;
        private List<string> Logs;
        public Logger(string logFileCon){
            LogFileCon = logFileCon;
            Logs = new List<string>();
        }

        public void Add(string mssg){
            Logs.Add(mssg);
        }

        public void Add(Exception ex){
            Logs.Add("");
            Logs.Add(ex.Message);
            Logs.Add("");
            Logs.Add(ex.StackTrace);
            Logs.Add("");
        }

        public void WriteLogs(){
            using(StreamWriter writer = new StreamWriter(LogFileCon)){
                Logs.ForEach(log => {
                    writer.WriteLine(log);
                });
            }
        }
    }
}