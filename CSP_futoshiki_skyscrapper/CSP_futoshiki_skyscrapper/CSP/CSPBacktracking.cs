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

namespace CSP_futoshiki_skyscrapper.CSP
{
    class CSPBacktracking
    {

        private List<ICSPSolvable> solutionsList;
        private List<CsvStatistics> statisticsList;

        private Stopwatch stopwatch = new Stopwatch();
        private ICSPSolvable rootData;

        private int numberOfIterations = 0;

        public CSPBacktracking()
        {
            solutionsList = new List<ICSPSolvable>();
            statisticsList = new List<CsvStatistics>();

            if (GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
            {
                rootData = FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph.DeepClone();
            }
            else if(GAME_TYPE == GAME_TYPE_ENUM.SKYSCRAPPER)
            {
                rootData = SkyscraperProblemSingleton.GetInstance().initialSkyscrapperArray.DeepClone();
            }
            Solver();
        }

        private void Solver()
        {
            stopwatch.Start();
            CreateChildren(rootData);
            stopwatch.Stop();

            statisticsList.Add(new CsvStatistics(0, 0, 0, stopwatch.Elapsed.TotalMilliseconds, numberOfIterations));
            PrintAllSolutions();
            WriteLine("Koniec: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
            SaveStatisticsToFile("back-tracking", statisticsList);
            SaveAllSolutionsToTxtFile("back-tracking", solutionsList);
        }

        private void CreateChildren(ICSPSolvable currentNode)
        {
            numberOfIterations++;
            CSPNode mostLimited = currentNode.ChooseElementByHeuristics();

            if (mostLimited == null)
            {
                CheckIfWonWhenNoElementsLeft(currentNode);
            }
            else
            {
                List<int> allPossibilities = currentNode.ReturnAllPossibilitiesForElement(mostLimited);
                AddChildenForEveryPossibility(allPossibilities, currentNode, mostLimited);
            }
        }

        private void CheckIfWonWhenNoElementsLeft(ICSPSolvable currentNode)
        {
            if (currentNode.IsSolved())
            {
                statisticsList.Add(new CsvStatistics(statisticsList.Count + 1, numberOfIterations, stopwatch.Elapsed.TotalMilliseconds, 0, 0));
                solutionsList.Add(currentNode);
                currentNode.IsSolved();
                WriteLine("ZNALAZŁEM teraz takie: "+ stopwatch.Elapsed.TotalMilliseconds+" ms");
                currentNode.PrintAllElements();
                WriteLine("================");

            }
        }

        private void AddChildenForEveryPossibility(List<int> allPossibilities, ICSPSolvable currentNode, CSPNode mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                ICSPSolvable CSPNodeClone = currentNode.DeepClone();
                CSPNodeClone.AssignNewData(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                

                CreateChildren(CSPNodeClone);
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
