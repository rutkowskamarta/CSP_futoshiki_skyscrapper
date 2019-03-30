using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.SkyscrapperStructures
{
    class SkyscrapperArray
    {
        public int arraySize { get; set; }
        public int[,] contentArray { get; set; }

        public SkyscrapperArray(int arraySize)
        {
            this.arraySize = arraySize;
            contentArray = new int[arraySize, arraySize];
            foreach (var item in contentArray)
            {
                Console.WriteLine(item);
            }
        }

        public int[] GetRow(int rowNumber)
        {
            //public static void BlockCopy (Array src, int srcOffset, Array dst, int dstOffset, int count);
            int[] rowOfArray = new int[contentArray.GetLength(0)];
            return rowOfArray;
        }

        public int[] GetColumn(int columnNumber)
        {
            int[] columnOfArray = new int[contentArray.GetLength(1)];
            return columnOfArray;

        }

    }
}
