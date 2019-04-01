using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.DataStructures;
using static System.Console;
using System.Linq;

namespace CSP_futoshiki_skyscrapper.FutoshikiStructures
{
    class FutoshikiGraph : Graph<int> 
    {
        public override GraphNode<int>[,] nodes { get => base.nodes; set => base.nodes = value; }

        public FutoshikiGraph(int problemSize) : base(problemSize)
        {}

        public void PrintAllElements()
        {
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    Write(nodes[i,j].data+" ");
                }
                WriteLine();
            }
        }

        public void PrintAllElementsMutables()
        {
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    Write(nodes[i,j].isMutable+" ");
                }
                WriteLine();
            }
        }

        public void PrintAllConstraints()
        {
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    WriteLine("NODE: " + nodes[i, j].data + ": ");
                    foreach (var item in nodes[i, j].outgoingEdges)
                    {
                        WriteLine(item.ToString());
                    }
                    WriteLine("---------");
                }
                 

            }
        }
    }
}
