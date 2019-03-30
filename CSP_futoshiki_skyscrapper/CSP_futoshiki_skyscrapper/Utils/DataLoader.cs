using System;
using System.Collections.Generic;
using System.Text;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;
using CSP_futoshiki_skyscrapper.SkyscraperStructures;
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

        }

    }
}
