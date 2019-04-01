using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.DataStructures;
using static System.Console;

namespace CSP_futoshiki_skyscrapper.FutoshikiStructures
{
    class FutoshikiGraph
    {
        public int arraySize { get; }
        public Graph<int> elementsGraph;

        public FutoshikiGraph(int arraySize)
        {
            this.arraySize = arraySize;
            elementsGraph = new Graph<int>(arraySize);
        }

        public void AddNewFutoshikiElement(GraphNode<int> futoshikiElement)
        {
            elementsGraph.AddNode(futoshikiElement);
        }

        public void PrintAllElements()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    Write(elementsGraph.nodes[i,j].data);
                }
                WriteLine();
            }
        }

        public void PrintAllElementsMutables()
        {
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    Write(elementsGraph.nodes[i,j].isMutable+" ");
                }
                WriteLine();
            }
        }
    }
}
