using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using static System.Console;
using CSP_futoshiki_skyscrapper.DataStructures;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;
using CSP_futoshiki_skyscrapper.SkyscraperStructures;
using CSP_futoshiki_skyscrapper.FutoshikiStructures;

namespace CSP_futoshiki_skyscrapper.CSP
{
    class CSPBacktracking
    {

        List<ICSPSolvable> solutionsList;
        Stopwatch stopwatch = new Stopwatch();
        ICSPSolvable rootData;

        public CSPBacktracking()
        {
            solutionsList = new List<ICSPSolvable>();
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
            PrintAllSolutions();
            WriteLine("Koniec: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
            stopwatch.Stop();
        }

        private void CreateChildren(ICSPSolvable currentNode)
        {
            CSPNode mostLimited = currentNode.ChooseTheMostLimitedAndNotSet();

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
                solutionsList.Add(currentNode);
                currentNode.IsSolved();
                WriteLine("ZNALAZŁEM!: "+ stopwatch.Elapsed.TotalMilliseconds+" ms");
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
            foreach (var item in solutionsList)
            {
                item.PrintAllElements();
                WriteLine("===");
            }

        }
    }
}
