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

        public CSPBacktracking()
        {
            solutionsList = new List<ICSPSolvable>();
            if (GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
            {
                FutoshikiSolver();
            }
            else if(GAME_TYPE == GAME_TYPE_ENUM.SKYSCRAPPER)
            {
                SkyscrapperSolver();
            }
            

        }

        #region FUTOSHIKI
        private void FutoshikiSolver()
        {
            stopwatch.Start();
            ICSPSolvable rootData = FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph.DeepClone();

            CreateChildren(rootData);
            PrintAllSolutions();
            WriteLine("Koniec: " + stopwatch.ElapsedMilliseconds+" ms");
            stopwatch.Stop();
        }

        private void CreateChildren(ICSPSolvable currentNode)
        {
            CSPNode mostLimited = currentNode.ChooseTheMostLimitedAndNotSet();

            if (mostLimited == null)
                CheckIfWonWhenNoElementsLeft(currentNode);
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
                WriteLine("ZNALAZŁEM!: "+ stopwatch.ElapsedMilliseconds+" ms");
                currentNode.PrintAllElements();
                WriteLine("================");

            }
        }

        private void AddChildenForEveryPossibility(List<int> allPossibilities, ICSPSolvable currentNode, CSPNode mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                ICSPSolvable futoshikiGraphClone = currentNode.DeepClone();
                futoshikiGraphClone.AssignNewData(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                CreateChildren(futoshikiGraphClone);
            }
        }

        
        #endregion

        #region SKYSCRAPPER
        private void SkyscrapperSolver()
        {
            SkyscraperArray initialArray = new SkyscraperArray(SkyscraperProblemSingleton.problemSize);
            //teraz znajdź pierwsze najbardziej ograniczone, wypełnij zgodnie z ograniczeniami i dołóż jako dziecko roota

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
