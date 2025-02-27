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

        public static List<int> SearchEntireArray(ItemBox[] seed, int[] input){
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

        static bool TrySearchArrayInstance(ItemBox[] seed, int[] input, int index, out int match){
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

                if(seed[adjIndex].Blue != input[n] && seed[adjIndex].Red != input[n]){
                    return false;
                }
            }

            match = adjIndex;
            return true;
        }
    }
}