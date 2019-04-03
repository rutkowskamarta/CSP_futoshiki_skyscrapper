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
    class CSPForwardChecking
    {
        List<FutoshikiGraph> solutionsList;

        public CSPForwardChecking()
        {
            solutionsList = new List<FutoshikiGraph>();
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
            FutoshikiGraph rootData = FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph.DeepClone();
            rootData.InitializeAllDomains();
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
                List<int> allPossibilities = mostLimited.domain;
                AddChildenForEveryPossibility(allPossibilities, currentNode, mostLimited);
            }
        }
        
        private void AddChildenForEveryPossibility(List<int> allPossibilities, FutoshikiGraph currentNode, GraphNode<int> mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                FutoshikiGraph futoshikiGraphClone = currentNode.DeepClone();
                futoshikiGraphClone.AssignNewDataAndUpdateDomains(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                
                if (!futoshikiGraphClone.IsAnyOfDomainsEmpty())
                {
                    CreateChildren(futoshikiGraphClone);
                }
            }
            
        }

        private void CheckIfWonWhenNoElementsLeft(FutoshikiGraph currentNode)
        {
            if (currentNode.IsFutoshikiSolved())
            {
                solutionsList.Add(currentNode);
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
