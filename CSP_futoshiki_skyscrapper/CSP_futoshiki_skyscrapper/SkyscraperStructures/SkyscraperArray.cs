using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Linq;

namespace CSP_futoshiki_skyscrapper.SkyscraperStructures
{
    class SkyscraperArray
    {
        public int arraySize { get; set; }
        public SkyscraperNode[,] contentArray { get; set; }

        public SkyscraperArray(int arraySize)
        {
            this.arraySize = arraySize;
            contentArray = new SkyscraperNode[arraySize, arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    contentArray[i, j] = new SkyscraperNode();
                }
            }

            //PrintArrayNumbers();
        }

        public bool IsSolved()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    if (contentArray[i, j].assignedNumber == 0)
                        return false;
                }
            }
            return true;
        }

        public SkyscraperNode[] GetRow(int rowNumber)
        {
            //public static void BlockCopy (Array src, int srcOffset, Array dst, int dstOffset, int count);
            SkyscraperNode[] rowOfArray = new SkyscraperNode[contentArray.GetLength(0)];
            throw new NotImplementedException();
            return rowOfArray;
        }

        public SkyscraperNode[] GetColumn(int columnNumber)
        {
            SkyscraperNode[] columnOfArray = new SkyscraperNode[contentArray.GetLength(1)];
            throw new NotImplementedException();
            return columnOfArray;

        }

        public void PrintArrayNumbers()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    Write($"{contentArray[i,j].assignedNumber} ");
                }
                WriteLine();
            }
        }
    }
}
