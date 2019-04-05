using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.CSP;

namespace CSP_futoshiki_skyscrapper.SkyscraperStructures
{
    class SkyscraperNode : CSPNode
    {
        public SkyscraperNode(int data, int xIndex, int yIndex) : base(data, true, xIndex, yIndex)
        {
        }

        public override CSPNode DeepClone()
        {
            SkyscraperNode skyscraperNode = new SkyscraperNode(data, xIndex, yIndex);
            skyscraperNode.domain = new List<int>();
            for (int i = 0; i < domain.Count; i++)
            {
                skyscraperNode.domain[i] = domain[i];
            }
            return skyscraperNode;
        }

        public void InitializeDomain(int problemSize)
        {
            for (int i = 0; i < problemSize; i++)
            {
                domain.Add(i + 1);
            }
        }
    }
}
