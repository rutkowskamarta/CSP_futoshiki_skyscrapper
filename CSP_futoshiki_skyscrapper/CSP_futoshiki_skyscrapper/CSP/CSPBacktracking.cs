using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.DataStructures;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;
using CSP_futoshiki_skyscrapper.SkyscraperStructures;
using CSP_futoshiki_skyscrapper.FutoshikiStructures;

namespace CSP_futoshiki_skyscrapper.CSP
{
    class CSPBacktracking<T>
    {
        //back tracking dla skyscrapper- budowanie drzewa:
        //1. wybierz najbardziej ograniczony wiersz/kolumnę 
        //2. wypełnij go zgodnie z ograniczeniami - zapisz wszystkie mozliwe sposoby - to będą węzły drzewa
        //3. dla każdego węzła wybierz następnika (kolejnego najbardziej ograniczonego) 
        //4. powtórz krok 2, potem 3, 2, 3...
        //5. jeśli nie można wypełnić zgodnie z ograniczeniami - powróć do rodzica, porzucając rozwiązanie
        //6. wybierz inną trasę
        //7. jeśli uda Ci się wypełnić wszystkie wiersze/kolumny czyli wszystkie kratki będą zapełnione, znalezione zostało rozwiązanie!
        //8. Zapisz rozwiązanie i szukaj kolejnych rozwiązań, cofaj się poprzez rodziców, do pierwszego węzła, który ma kolejną odnogę

        //metoda do wyboru oraz jaka gierka


        public CSPBacktracking()
        {
            if(GAME_TYPE == GAME_TYPE_ENUM.FUTOSHIKI)
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
            TreeNode<FutoshikiGraph> root = new TreeNode<FutoshikiGraph>(FutoshikiProblemSingleton.GetInstance().initialFutoshikiGraph);
            //wypełniać drzewo dopóki nie wrócę do korzenia i nie będzie kolejnych ścieżek
            GraphNode<int> mostLimited = root.data.ChooseTheMostLimitedAndNotSet();
            List<int> allPossibilities = root.data.ReturnAllPossibilitiesForNode(mostLimited);
            //dodaj jako nowe plansze i nowe dzieci, jeżeli liczba możliwości równa się 0, backtrack

            while (true) //tutaj dodać ten warunek, może coś z visited
            {

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

    }
}
