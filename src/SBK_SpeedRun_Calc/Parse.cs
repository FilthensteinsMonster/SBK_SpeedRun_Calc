namespace SBK_SpeedRun_Calc{
    static class Parse{

        public static Dictionary<string,string> NoticeConfig(string filePath){
            Dictionary<string,string> result = new Dictionary<string, string>();

            if(!File.Exists(filePath)){
                throw new Exception("Error: Seed file not found: " + filePath);
            }

            using(StreamReader reader = new StreamReader(filePath)){
                while(!reader.EndOfStream){
                    string line = reader.ReadLine().Trim();

                    if(!string.IsNullOrWhiteSpace(line)){
                        string[] values = line.Split(',');
                        string details =  values[0].Trim(); 
                        string index =  values[1].Trim();

                        result.Add(details,index);
                    }
                }
            }

            return result;
        }


        public static Dictionary<string, string[]> KeyBindConfig(string filePath){
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
        public static ItemBox[] SeedData(string filePath){

            List<ItemBox> result = new List<ItemBox>();

            if(!File.Exists(filePath)){
                throw new Exception("Error: Seed file not found: " + filePath);
            }

            using(StreamReader reader = new StreamReader(filePath)){
                while(!reader.EndOfStream){
                    string line = reader.ReadLine().Trim();

                    if(!string.IsNullOrWhiteSpace(line)){
                        string[] values = line.Split(',');
                        int blue = int.Parse(values[0].Trim());
                        int red = int.Parse(values[1].Trim());

                        ItemBox item = new ItemBox(){Red = red, Blue = blue};
                        result.Add(item);
                    }
                }
            }

            return result.ToArray();
        }
    }
}