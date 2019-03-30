using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace CSP_futoshiki_skyscrapper.SkyscraperStructures
{
    sealed class SkyscraperProblemSingleton
    {
        private static SkyscraperProblemSingleton instance = null;

        public static int problemSize { get; set; }
        public static int[] upperContraints { get; set; }
        public static int[] lowerContraints { get; set; }
        public static int[] leftContraints { get; set; }
        public static int[] rightContraints { get; set; }

        public static SkyscraperProblemSingleton GetInstance()
        {
            if(instance == null)
            {
                instance = new SkyscraperProblemSingleton();
            }
            return instance;
        }

        private SkyscraperProblemSingleton()
        {

        }

        public void Initialize(int problemSizeArg)
        {
            problemSize = problemSizeArg;
            upperContraints = new int[problemSize];
            lowerContraints = new int[problemSize];
            leftContraints = new int[problemSize];
            rightContraints = new int[problemSize];
        }

        public void PrintConstraints()
        {
            PrintOneConstraintTab(upperContraints, "G");
            PrintOneConstraintTab(lowerContraints, "D");
            PrintOneConstraintTab(leftContraints, "L");
            PrintOneConstraintTab(rightContraints, "P");
            
            WriteLine();
        }

        private void PrintOneConstraintTab(int[] constraintTab, string annotation)
        {
            Write($"{annotation} ");
            for (int i = 0; i < problemSize; i++)
            {
                Write($"{constraintTab[i]} ");
            }
            WriteLine();
        }
    }
}
