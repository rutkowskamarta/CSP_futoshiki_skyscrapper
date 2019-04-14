using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using static System.Console;
using CSP_futoshiki_skyscrapper.DataStructures;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;
using CSP_futoshiki_skyscrapper.SkyscraperStructures;
using CSP_futoshiki_skyscrapper.FutoshikiStructures;
using CSP_futoshiki_skyscrapper.Utils;
using System.Threading.Tasks;

namespace CSP_futoshiki_skyscrapper.CSP
{
    class CSPForwardChecking
    {
        private List<ICSPSolvable> solutionsList;
        private List<CsvStatistics> statisticsList;

        private Stopwatch stopwatch = new Stopwatch();
        private ICSPSolvable rootData;
        private int numberOfIterations = 0;

        private static bool hasRootBeenParallel = false;

        public CSPForwardChecking()
        {
            solutionsList = new List<ICSPSolvable>();
            statisticsList = new List<CsvStatistics>();

            if (GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
            {
                rootData = FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph.DeepClone();
            }
            else if (GAME_TYPE == GAME_TYPE_ENUM.SKYSCRAPPER)
            {
                rootData = SkyscraperProblemSingleton.GetInstance().initialSkyscrapperArray.DeepClone();
            }
            Solver();
        }
        
        private void Solver()
        {
            stopwatch.Start();
            rootData.InitializeAllDomains();
            CreateChildren(rootData);
            stopwatch.Stop();

            PrintAllSolutions();

            statisticsList.Add(new CsvStatistics(0, 0, 0, stopwatch.Elapsed.TotalSeconds, numberOfIterations));
            SaveStatisticsToFile(ALGORITHM_TYPE.ToString(), statisticsList);
            SaveAllSolutionsToTxtFile(ALGORITHM_TYPE.ToString(), solutionsList);
        }

        private void CreateChildren(ICSPSolvable currentNode)
        {
            CSPNode mostLimited = currentNode.ChooseElementByHeuristics();

            if (mostLimited == null)
                CheckIfWonWhenNoElementsLeft(currentNode);
            else
            {
                List<int> allPossibilities = mostLimited.domain;

                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = 4;

                Parallel.ForEach(allPossibilities, options, (i) =>
                {
                    AddChild(i, currentNode, mostLimited);
                });
            }
        }
        
        private void AddChildenForEveryPossibility(List<int> allPossibilities, ICSPSolvable currentNode, CSPNode mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                ICSPSolvable nodeClone = currentNode.DeepClone();
                nodeClone.AssignNewDataAndUpdateDomains(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                
                if (!nodeClone.IsAnyOfDomainsEmpty())
                {
                    CreateChildren(nodeClone);
                }
            }
            
        }

        private void AddChild(int possibility, ICSPSolvable currentNode, CSPNode mostLimited)
        {
            ICSPSolvable nodeClone = currentNode.DeepClone();
            nodeClone.AssignNewDataAndUpdateDomains(mostLimited.xIndex, mostLimited.yIndex, possibility);

            if (!nodeClone.IsAnyOfDomainsEmpty())
                CreateChildren(nodeClone);
            else
                numberOfIterations++;
        }


        private void CheckIfWonWhenNoElementsLeft(ICSPSolvable currentNode)
        {
            if (currentNode.IsSolved())
            {
                statisticsList.Add(new CsvStatistics(statisticsList.Count+1, numberOfIterations, stopwatch.Elapsed.TotalSeconds, 0, 0));
                solutionsList.Add(currentNode);
                currentNode.PrintAllElements();
            }
        }


        private void PrintAllSolutions()
        {
            WriteLine();
            WriteLine();
            WriteLine("=========================");
            WriteLine("OSTATECZNE ROZWIĄZANIA");
            WriteLine("=========================");
            WriteLine();
            foreach (var item in solutionsList)
            {
                item.PrintAllElements();
                WriteLine("===");
            }

        }
    }
}
