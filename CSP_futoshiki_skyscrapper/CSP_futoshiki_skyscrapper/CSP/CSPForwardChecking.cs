﻿using System;
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
        List<int> numberOfIterationsPerSolution;
        List<int> timePerSolution;
        Stopwatch stopwatch = new Stopwatch();
        ICSPSolvable rootData;
        int numberOfIterations = 0;
        
        public CSPForwardChecking()
        {
            solutionsList = new List<ICSPSolvable>();
            timePerSolution = new List<int>();
            numberOfIterationsPerSolution = new List<int>();

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
            WriteLine("Koniec: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
        }

        private void CreateChildren(ICSPSolvable currentNode)
        {
            //numberOfIterations++;
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
                ICSPSolvable nodeClone = currentNode.DeepClone();
                nodeClone.AssignNewDataAndUpdateDomains(mostLimited.xIndex, mostLimited.yIndex, allPossibilities[i]);
                
                if (!nodeClone.IsAnyOfDomainsEmpty())
                {
                    CreateChildren(nodeClone);
                }
            }
            
        }

        private void CheckIfWonWhenNoElementsLeft(ICSPSolvable currentNode)
        {
            if (currentNode.IsSolved())
            {
                solutionsList.Add(currentNode);
                WriteLine("ZNALAZŁEM teraz takie: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
                currentNode.PrintAllElements();
                WriteLine("================");
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
