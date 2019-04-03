using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.CSP;

namespace CSP_futoshiki_skyscrapper.SkyscraperStructures
{
    class SkyscraperNode : CSPNode
    {
        public int data { get; set; } = 0;
        public int xIndex { get; }
        public int yIndex { get; }
        public int measure { get; set; }
        public List<int> domain { get; set; }

        public SkyscraperNode(int data, int xIndex, int yIndex) : base(data, false, xIndex, yIndex)
        {

        }

        public override CSPNode DeepClone()
        {
            throw new NotImplementedException();
        }
    }
}
