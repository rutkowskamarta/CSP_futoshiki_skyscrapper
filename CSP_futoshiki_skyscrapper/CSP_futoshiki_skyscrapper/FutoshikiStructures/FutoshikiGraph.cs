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


       
        public List<int> ReturnAllPossibilitiesForNode(GraphNode<int> node)
        {
            List<int> allPosibilites = new List<int>();
            List<int> itemsToRemove = new List<int>();

            for (int i = 0; i < problemSize; i++)
                allPosibilites.Add(i+1);

            for (int i = 0; i < problemSize; i++)
            {
                var columnElement = nodes[i, node.yIndex];
                if (allPosibilites.Contains(columnElement.data))
                    itemsToRemove.Add(columnElement.data);

                var rowElement = nodes[node.xIndex, i];
                if (allPosibilites.Contains(rowElement.data))
                    itemsToRemove.Add(rowElement.data);
            }
           
            for (int i = 0; i < allPosibilites.Count; i++)
            {
                for (int j = 0; j < node.outgoingEdges.Count; j++)
                {
                    if(node.outgoingEdges[i].edgeType == GraphEdge<int>.EDGE_TYPE_ENUM.DESTINATION_GRATER)
                    {
                        if (node.outgoingEdges[i].destinationNode.data != 0 && allPosibilites[j] > node.outgoingEdges[i].destinationNode.data)
                            itemsToRemove.Add(allPosibilites[j]);
                    }
                    else
                    {
                        if (node.outgoingEdges[i].destinationNode.data != 0 && allPosibilites[j] < node.outgoingEdges[i].destinationNode.data)
                            itemsToRemove.Add(allPosibilites[j]);
                    }
                }

            }

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                allPosibilites.Remove(itemsToRemove[i]);
            }

            return allPosibilites;

        }

        //moja heurystyka, wybieram tego, co ma najwięcej ograniczeń z edge
        public GraphNode<int> ChooseTheMostLimitedAndNotSet()
        {
            var ordered = nodes.OfType<GraphNode<int>>();
            return ordered.Select(i => i).Where(i => i.data == 0).OrderBy(i => i.outgoingEdges.Count).First();

        }

       

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
