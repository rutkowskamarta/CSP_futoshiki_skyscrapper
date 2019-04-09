using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.Utils
{
    class CsvStatistics
    {
        public int numberOfSolution { get; }
        public int numberOfIterations { get; }
        public double time { get; }
        public double totalTime { get; }
        public int totalIterations { get; }

        public CsvStatistics(int numberOfSolution, int numberOfIterations, double time, double totalTime, int totalIterations)
        {
            this.numberOfSolution = numberOfSolution;
            this.numberOfIterations = numberOfIterations;
            this.time = time;
            this.totalIterations = totalIterations;
            this.totalTime = totalTime;
        }
    }
}
