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
            Tree<FutoshikiGraph> forwardCheckingTree = new Tree<FutoshikiGraph>(rootData);
            
            CreateChildren(forwardCheckingTree.root);

        }

        private void CreateChildren(TreeNode<FutoshikiGraph> currentNode)
        {
            GraphNode<int> mostLimited = currentNode.data.ChooseTheMostLimitedAndNotSet();

            if (mostLimited == null)
            {
                //TODO
                solutionsList.Add(currentNode.data);
            }
            else
            {
                List<int> allPossibilities = mostLimited.domain;
                AddChildenForEveryPossibility(allPossibilities, currentNode, mostLimited);
            }
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
                futoshikiGraphClone.AssignNewDataAndUpdateDomains(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);

                
                TreeNode<FutoshikiGraph> newChild = new TreeNode<FutoshikiGraph>(currentNode, futoshikiGraphClone);
                currentNode.AddChild(newChild);
                
                if (futoshikiGraphClone.IsAnyOfDomainsEmpty())
                {
                    Backtracking(currentNode);
                }
                else
                {
                    CreateChildren(newChild);
                }
            }

            //TODO
        }

      
        #endregion

        #region SKYSCRAPPER
        private void SkyscrapperSolver()
        {


        }

        #endregion


    }
}
