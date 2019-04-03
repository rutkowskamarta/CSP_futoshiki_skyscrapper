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
        {
        }

        public FutoshikiGraph DeepClone()
        {
            FutoshikiGraph futoshikiGraph = new FutoshikiGraph(problemSize);
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    futoshikiGraph.nodes[i, j] = nodes[i, j].DeepClone();
                }
                //po tym jak skopiuję wszystkie nody, muszę poprzypisywać im nowe source i dest

            }
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    for (int k = 0; k < nodes[i, j].outgoingEdges.Count; k++)
                    {
                        //skopiuję krawędź i podmienię jej source i dest
                        //przypisane są stare referencje
                        futoshikiGraph.nodes[i, j].outgoingEdges[k].sourceNode = futoshikiGraph.nodes[i, j];
                        GraphNode<int> oldDestination = nodes[i, j].outgoingEdges[k].destinationNode;
                        futoshikiGraph.nodes[i, j].outgoingEdges[k].destinationNode = futoshikiGraph.nodes[oldDestination.xIndex, oldDestination.yIndex];
                    }
                }
            }
            return futoshikiGraph;
        }

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

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                allPosibilites.Remove(itemsToRemove[i]);
            }

            itemsToRemove.Clear();


            for (int i = 0; i < node.outgoingEdges.Count; i++)
            {
                for (int j = 0; j < allPosibilites.Count; j++)
                {
                    if (node.outgoingEdges[i].edgeType == GraphEdge<int>.EDGE_TYPE_ENUM.DESTINATION_GRATER)
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

        public GraphNode<int> ChooseTheMostLimitedAndNotSet()
        {
            nodes.OfType<GraphNode<int>>().AsParallel().Where(i=>i.data==0).ForAll(i => i.measure = CalculateMeasure(i));
            var ordered = nodes.OfType<GraphNode<int>>().OrderByDescending(i => i.measure).Where(i => i.data == 0).ToList();
            if(ordered.Count == 0)
            {
                return null;
            }
            else
            {
                return ordered.First();
            }
        }

        public bool IsFutoshikiSolved()
        {

            if (nodes.OfType<GraphNode<int>>().AsParallel().Any(i => i.data == 0))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < problemSize; i++)
                {
                    for (int j = 0; j < problemSize; j++)
                    {
                        if (!nodes[i, j].AreConstraintsSatisfied())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private int CalculateMeasure(GraphNode<int> node)
        {
            int rowMeasure = nodes.OfType<GraphNode<int>>().Where(i => i.xIndex == node.xIndex && i.data != 0).Count();
            int columnMeasure = nodes.OfType<GraphNode<int>>().Where(i => i.yIndex == node.yIndex && i.data != 0).Count();
            int constraintMeasure = node.outgoingEdges.Count;
            return rowMeasure + columnMeasure + constraintMeasure;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    stringBuilder.Append(nodes[i, j].data +";");
                }
                stringBuilder.Append("=");
            }
            return stringBuilder.ToString();
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
