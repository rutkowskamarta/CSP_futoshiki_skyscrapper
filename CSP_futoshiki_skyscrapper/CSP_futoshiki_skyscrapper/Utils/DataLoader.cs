using System;
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
                    futoshikiGraph.AddNewFutoshikiElement(ParseNode(oneMatrixLine[i], counter - 2, i));
                }
                counter++;
            }

            Console.WriteLine("=========");
            futoshikiGraph.PrintAllElements();
            Console.WriteLine();
            futoshikiGraph.PrintAllElementsMutables();
            Console.WriteLine();


        }

        private GraphNode<int> ParseNode(string line, int x, int y)
        {
            int nodeValue = int.Parse(line);
            bool isMutable = (nodeValue == 0) ? true : false;



            GraphNode<int> node = new GraphNode<int>(nodeValue, isMutable, x, y);
            return node;

        }

        private List<GraphEdge<int>>[,] ParseConstraints(string[] allLines, int counter, int problemSize)
        {
            //TUTAJ!!!!
            List<GraphEdge<int>>[,] edgesMatrix = new List<GraphEdge<int>>[problemSize, problemSize];
            List<GraphEdge<int>> fileEdges = new List<GraphEdge<int>>();
            while (counter<allLines.Length)
            {
                string[] constraintLine = allLines[counter].Split(';');
                
            }

            return edgesMatrix;

        }



    }
}
