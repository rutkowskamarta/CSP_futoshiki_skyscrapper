using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using CSP_futoshiki_skyscrapper.DataStructures;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;
using CSP_futoshiki_skyscrapper.SkyscraperStructures;
using CSP_futoshiki_skyscrapper.FutoshikiStructures;

namespace CSP_futoshiki_skyscrapper.CSP
{
    class CSPBacktracking
    {

        List<FutoshikiGraph> solutionsList;

        public CSPBacktracking()
        {
            solutionsList = new List<FutoshikiGraph>();
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
            FutoshikiGraph rootData = FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph.DeepClone();

            CreateChildren(rootData);
            PrintAllSolutions();
            
        }

        private void CreateChildren(FutoshikiGraph currentNode)
        {
            GraphNode<int> mostLimited = currentNode.ChooseTheMostLimitedAndNotSet();

            if (mostLimited == null)
                CheckIfWonWhenNoElementsLeft(currentNode);
            else
            {
                List<int> allPossibilities = currentNode.ReturnAllPossibilitiesForNode(mostLimited);
                AddChildenForEveryPossibility(allPossibilities, currentNode, mostLimited);
            }
        }

        private void CheckIfWonWhenNoElementsLeft(FutoshikiGraph currentNode)
        {
            if (currentNode.IsFutoshikiSolved())
            {
                solutionsList.Add(currentNode);
            }
        }

        private void AddChildenForEveryPossibility(List<int> allPossibilities, FutoshikiGraph currentNode, GraphNode<int> mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                FutoshikiGraph futoshikiGraphClone = currentNode.DeepClone();
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
