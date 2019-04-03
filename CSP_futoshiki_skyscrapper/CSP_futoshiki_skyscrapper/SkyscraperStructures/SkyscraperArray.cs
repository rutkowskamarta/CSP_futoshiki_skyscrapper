using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Linq;
using CSP_futoshiki_skyscrapper.CSP;
namespace CSP_futoshiki_skyscrapper.SkyscraperStructures
{
    class SkyscraperArray : ICSPSolvable
    {
        public int arraySize { get; set; }
        public SkyscraperNode[,] contentArray { get; set; }

        public SkyscraperArray(int arraySize)
        {
            this.arraySize = arraySize;
            contentArray = new SkyscraperNode[arraySize, arraySize];

            //przerobić na linq
            for (int i = 0; i < arraySize; i++)
            {
                //for (int j = 0; j < arraySize; j++)
                    //contentArray[i, j] = new SkyscraperNode();
            }
            
        }

        
     

        public void PrintArrayNumbers()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    Write($"{contentArray[i,j].data} ");
                }
                WriteLine();
            }
        }

        public ICSPSolvable DeepClone()
        {
            throw new NotImplementedException();
        }

        public void InitializeAllDomains()
        {
            throw new NotImplementedException();
        }

        public void AssignNewDataAndUpdateDomains(int xIndex, int yIndex, int newData)
        {
            throw new NotImplementedException();
        }

        public List<int> ReturnAllPossibilitiesForElement(CSPNode element)
        {
            throw new NotImplementedException();
        }

        public CSPNode ChooseTheMostLimitedAndNotSet()
        {
            throw new NotImplementedException();
        }

        public void PrintAllElements()
        {
            throw new NotImplementedException();
        }

        public bool IsSolved()
        {
            throw new NotImplementedException();
        }

        public bool IsAnyOfDomainsEmpty()
        {
            throw new NotImplementedException();
        }

        public void AssignNewData(int xIndex, int yIndex, int newData)
        {
            throw new NotImplementedException();
        }
    }
}
