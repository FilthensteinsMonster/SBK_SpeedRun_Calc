namespace SBK_SpeedRun_Calc{
    static class Logic{
        public static int IncrimentSeedIndex(int current, ItemBox[] seed){
            int max = seed.Length - 1;
            int next = current + 1;
            if(next > max){
                next = 0;
            }
            return next;
        }

        public static List<int> SearchEntireArray(ItemBox[] seed, int[] input, string[] color){
            List<int> seedIndexs = new List<int>();

            int start = input.Length;

            for(int n = start; n < seed.Length; n++){
                int match = -1;
                bool found = TrySearchArrayInstance(seed, input, color, n, out match);

                if(found && match != -1){
                    seedIndexs.Add(match);
                }
            }

            return seedIndexs;
        }

        static bool TrySearchArrayInstance(ItemBox[] seed, int[] inputVal, string[] inputColor, int index, out int match){
            match = -1;

            int max = seed.Length;
            int adjIndex = -1;

            for(int n = 0; n < inputVal.Length; n++){

                int idx = index + n;
                int maxL = max - 1;
                int overflow = maxL / idx;
                int loopIdx = (idx % maxL) - 1;

                bool hasLoop = (overflow == 0) ? true : false;
                adjIndex = (hasLoop == true) ? loopIdx : idx;
                
                // debug
                // Console.WriteLine("index: " + index +  " hasLoop: " + hasLoop + " idx,maxL,over,loopIdx: " + idx + "," + maxL + "," + overflow + "," + loopIdx +  " adj: " + adjIndex);

                if(inputColor[n] == "red"){
                    if(seed[adjIndex].Red != inputVal[n]){
                        return false;
                    }
                }

                if(inputColor[n] == "blue"){
                    if(seed[adjIndex].Blue != inputVal[n]){
                        return false;
                    }
                }
            }

            match = adjIndex;
            return true;
        }

        public static int NextBlueItem(ItemBox[] seed, int index, int itemId){      
            List<ItemBox> temp = new List<ItemBox>(seed);
            for(int n = 0; n < index; n++){
               temp.Add(seed[n]);
            }

            for(int n = index; n < temp.Count; n++){
                if(temp[n].Blue == itemId){
                    return n - index;
                }
            }
            
            throw new Exception("Error related to NextBlueItem() no item matching id " + itemId + " was found. Review KeyBind/Seed file.");
        }

        public static int RollIndexDelta(int seedSize, int targetIndex, int currentIndex){
            if(targetIndex >= currentIndex){
                return targetIndex - currentIndex;
            }

           return targetIndex - currentIndex + seedSize;
        }
    }
}