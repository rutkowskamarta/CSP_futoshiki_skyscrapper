﻿using System;
using System.Collections.Generic;
using System.Text;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;
using CSP_futoshiki_skyscrapper.SkyscraperStructures;
using CSP_futoshiki_skyscrapper.DataStructures;
using CSP_futoshiki_skyscrapper.FutoshikiStructures;
using System.IO;

namespace CSP_futoshiki_skyscrapper.Utils
{
    class DataLoader
    {
        private delegate void loaderMethodDelegate();

        private loaderMethodDelegate loaderMethodFunction;

        public DataLoader()
        {
            if(GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
            {
                loaderMethodFunction = LoadDataForFutoshiki;
                CheckIfCorrectFileSelected("futo");
            }
            else if(GAME_TYPE == GAME_TYPE_ENUM.SKYSCRAPPER)
            {
                loaderMethodFunction = LoadDataForSkyscrapper;
                CheckIfCorrectFileSelected("sky");
                
            }

            loaderMethodFunction();
        }

        private void CheckIfCorrectFileSelected(string prefix)
        {
            if (!FILE_NAME.Contains(prefix))
            {
                throw new WrongFileException($"selected file is not {prefix} file");
            }
        }

        private void LoadDataForSkyscrapper()
        {
            string[] allLines = File.ReadAllLines(Utilities.FILE_NAME);

            int problemSize = int.Parse(allLines[0]);

            //SkyscraperArray initialSkyScraperArray= new SkyscraperArray(problemSize);

            SkyscraperProblemSingleton skyscraperProblem = SkyscraperProblemSingleton.GetInstance();
            skyscraperProblem.Initialize(problemSize);
            SkyscraperProblemSingleton.upperContraints = ParseConstraintLineForSkyscrapper(allLines[1], problemSize);
            SkyscraperProblemSingleton.lowerContraints = ParseConstraintLineForSkyscrapper(allLines[2], problemSize);
            SkyscraperProblemSingleton.leftContraints = ParseConstraintLineForSkyscrapper(allLines[3], problemSize);
            SkyscraperProblemSingleton.rightContraints = ParseConstraintLineForSkyscrapper(allLines[4], problemSize);
            //skyscraperProblem.PrintConstraints();

        }

        private int[] ParseConstraintLineForSkyscrapper(string line, int problemSize)
        {
            int[] convertedConstraints = new int[problemSize];

            string[] constraints = line.Split(";");
            for (int i = 1; i < constraints.Length; i++)
            {
                convertedConstraints[i-1] = int.Parse(constraints[i]);
            }
            return convertedConstraints;
        }

        private void LoadDataForFutoshiki()
        {
            string[] allLines = File.ReadAllLines(Utilities.FILE_NAME);
            int problemSize = int.Parse(allLines[0]);
            int counter = 2;

            FutoshikiGraph futoshikiGraph = new FutoshikiGraph(problemSize);
            while (allLines[counter] != "REL:")
            {
                string[] oneMatrixLine = allLines[counter].Split(';');
                for (int i = 0; i < oneMatrixLine.Length; i++)
                {
                    futoshikiGraph.AddNode(ParseNode(oneMatrixLine[i], counter - 2, i));
                }
                counter++;
            }

            Console.WriteLine("=========");
            futoshikiGraph.PrintAllElements();
            Console.WriteLine();
            futoshikiGraph.PrintAllElementsMutables();
            Console.WriteLine();

            counter++;

            while (counter < allLines.Length)
            {
                char[] constraintLine = allLines[counter].ToCharArray();
                int xIndex1 = System.Convert.ToInt32(constraintLine[0]) - 65;
                int yIndex1 = int.Parse(constraintLine[1].ToString()) - 1;
                int xIndex2 = System.Convert.ToInt32(constraintLine[3]) - 65;
                int yIndex2 = int.Parse(constraintLine[4].ToString()) - 1;

                futoshikiGraph.AddEdgeFromSource(xIndex1, yIndex1, xIndex2, yIndex2, GraphEdge<int>.EDGE_TYPE_ENUM.DESTINATION_GRATER);
                futoshikiGraph.AddEdgeFromSource(xIndex2, yIndex2, xIndex1, yIndex1, GraphEdge<int>.EDGE_TYPE_ENUM.SOURCE_GREATER);

                counter++;

            }

            futoshikiGraph.PrintAllConstraints();
            FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph = futoshikiGraph;

        }

        private GraphNode<int> ParseNode(string line, int x, int y)
        {
            int nodeValue = int.Parse(line);
            bool isMutable = (nodeValue == 0) ? true : false;

            GraphNode<int> node = new GraphNode<int>(nodeValue, isMutable, x, y);
            return node;

        }

    }
}
