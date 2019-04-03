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
    class CSPForwardChecking
    {
        List<ICSPSolvable> solutionsList;
        Stopwatch stopwatch = new Stopwatch();


        public CSPForwardChecking()
        {
            solutionsList = new List<ICSPSolvable>();
            if (GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
            {
                FutoshikiSolver();
            }
            else if (GAME_TYPE == GAME_TYPE_ENUM.SKYSCRAPPER)
            {
                SkyscrapperSolver();
            }
            
        }

        #region FUTOSHIKI
        private void FutoshikiSolver()
        {
            stopwatch.Start();
            ICSPSolvable rootData = FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph.DeepClone();
            rootData.InitializeAllDomains();
            CreateChildren(rootData);
            PrintAllSolutions();
            WriteLine("Koniec: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Stop();
        }

        private void CreateChildren(ICSPSolvable currentNode)
        {
            CSPNode mostLimited = currentNode.ChooseTheMostLimitedAndNotSet();

            if (mostLimited == null)
                CheckIfWonWhenNoElementsLeft(currentNode);
            else
            {
                List<int> allPossibilities = mostLimited.domain;
                AddChildenForEveryPossibility(allPossibilities, currentNode, mostLimited);
            }
        }
        
        private void AddChildenForEveryPossibility(List<int> allPossibilities, ICSPSolvable currentNode, CSPNode mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                ICSPSolvable futoshikiGraphClone = currentNode.DeepClone();
                futoshikiGraphClone.AssignNewDataAndUpdateDomains(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                
                if (!futoshikiGraphClone.IsAnyOfDomainsEmpty())
                {
                    CreateChildren(futoshikiGraphClone);
                }
            }
            
        }

        private void CheckIfWonWhenNoElementsLeft(ICSPSolvable currentNode)
        {
            if (currentNode.IsSolved())
            {
                solutionsList.Add(currentNode);
                WriteLine("ZNALAZŁEM!: "+stopwatch.ElapsedMilliseconds+" ms");
                currentNode.PrintAllElements();
                WriteLine("================");
            }
        }

        #endregion

        #region SKYSCRAPPER
        private void SkyscrapperSolver()
        {


        }

        #endregion

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
