using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.FutoshikiStructures
{
    class FutoshikiProblemSingleton
    {
        private static FutoshikiProblemSingleton instance = null;
        public FutoshikiGraph initialFutoshikiGraph { get; set; }

        public static FutoshikiProblemSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FutoshikiProblemSingleton();
            }
            return instance;
        }

    }
}
