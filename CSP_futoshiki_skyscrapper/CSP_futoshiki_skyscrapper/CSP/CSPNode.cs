using System;
using System.Collections.Generic;
using System.Text;

namespace CSP_futoshiki_skyscrapper.CSP
{
    abstract class CSPNode
    {
        public int data { get; set; }
        public bool isMutable { get; }
        public int xIndex { get; }
        public int yIndex { get; }
        public int measure { get; set; }
        public List<int> domain { get; set; }

        public CSPNode(int data, bool isMutable, int xIndex, int yIndex)
        {
            this.data = data;
            this.isMutable = isMutable;
            this.xIndex = xIndex;
            this.yIndex = yIndex;

            domain = new List<int>();
        }

        public abstract CSPNode DeepClone();

    }
}
