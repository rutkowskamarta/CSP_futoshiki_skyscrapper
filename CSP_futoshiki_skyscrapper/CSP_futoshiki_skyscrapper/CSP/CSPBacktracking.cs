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
            Tree<FutoshikiGraph> backtrackingTree = new Tree<FutoshikiGraph>(rootData);

            CreateChildren(backtrackingTree.root);
            PrintAllSolutions();
            
        }

        private void CreateChildren(TreeNode<FutoshikiGraph> currentNode)
        {
            GraphNode<int> mostLimited = currentNode.data.ChooseTheMostLimitedAndNotSet();

            if (mostLimited == null)
                CheckIfWonWhenNoElementsLeft(currentNode);
            else
            {
                List<int> allPossibilities = currentNode.data.ReturnAllPossibilitiesForNode(mostLimited);
                AddChildenForEveryPossibility(allPossibilities, currentNode, mostLimited);
                PropagateInformationOfSolutionInAnyChildToParent(currentNode);
            }
        }

        private void CheckIfWonWhenNoElementsLeft(TreeNode<FutoshikiGraph> currentNode)
        {
            if (currentNode.data.IsFutoshikiSolved())
            {
                currentNode.isSolved = true;
                currentNode.parent.isSolved = true;
                solutionsList.Add(currentNode.data);
            }
            else
                Backtracking(currentNode);
        }

        private void Backtracking(TreeNode<FutoshikiGraph> currentNode)
        {
            currentNode.parent.children.Remove(currentNode);
        }

        private void AddChildenForEveryPossibility(List<int> allPossibilities, TreeNode<FutoshikiGraph> currentNode, GraphNode<int> mostLimited)
        {
            for (int i = 0; i < allPossibilities.Count; i++)
            {
                FutoshikiGraph futoshikiGraphClone = currentNode.data.DeepClone();
                futoshikiGraphClone.AssignNewData(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                TreeNode<FutoshikiGraph> newChild = new TreeNode<FutoshikiGraph>(currentNode, futoshikiGraphClone);
                currentNode.AddChild(newChild);
                //rekurencja
                CreateChildren(newChild);
            }
        }

        private void PropagateInformationOfSolutionInAnyChildToParent(TreeNode<FutoshikiGraph> currentNode)
        {
            if (currentNode.parent != null)
            {
                if (!currentNode.isSolved)
                    Backtracking(currentNode);

                else
                {
                    if (currentNode.parent != null)
                        currentNode.parent.isSolved = true;
                }
            }
        }
        #endregion

        #region SKYSCRAPPER
        private void SkyscrapperSolver()
        {
            SkyscraperArray initialArray = new SkyscraperArray(SkyscraperProblemSingleton.problemSize);
            Tree<SkyscraperArray> solutionsTree = new Tree<SkyscraperArray>(initialArray);
            //teraz znajdź pierwsze najbardziej ograniczone, wypełnij zgodnie z ograniczeniami i dołóż jako dziecko roota

        }
        
        private TreeNode<SkyscraperArray> fillTheTree(TreeNode<SkyscraperArray> startingNode)
        {
            //trochę inaczaj, bo to znajdzie jedno
            if (startingNode.data.IsSolved())
                return startingNode;
            return startingNode;

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
