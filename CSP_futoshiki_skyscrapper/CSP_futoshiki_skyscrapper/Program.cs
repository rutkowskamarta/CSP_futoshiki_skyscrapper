using System;
using CSP_futoshiki_skyscrapper.Utils;
using static System.Console;
using CSP_futoshiki_skyscrapper.CSP;

namespace CSP_futoshiki_skyscrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            DataLoader dataLoader = new DataLoader();
            //CSPBacktracking cSPBacktracking = new CSPBacktracking();
            CSPForwardChecking cSPForwardChecking = new CSPForwardChecking();
            WriteLine("finished!");
            ReadLine();
        }

    }
}
