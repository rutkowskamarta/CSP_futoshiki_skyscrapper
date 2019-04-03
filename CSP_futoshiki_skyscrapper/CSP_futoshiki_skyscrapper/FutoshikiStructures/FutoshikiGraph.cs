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

        public FutoshikiGraph(int problemSize) : base(problemSize){}

        public bool IsAnyOfDomainsEmpty()
        {
            return nodes.OfType<GraphNode<int>>().AsParallel().Any(i => i.domain.Count== 0);
        }

        public void AssignNewData(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
        }

        public void AssignNewDataAndUpdateDomains(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
            UpdateAllDomains(nodes[xIndex, yIndex], newData);
        }

        private void UpdateAllDomains(GraphNode<int> node, int newData)
        {
            UpdateDomainInRowsAndColumns(node, newData);
            UpdateDomainsForConstraints(node, newData);
        }

        private void UpdateDomainInRowsAndColumns(GraphNode<int>node, int data)
        {
            for (int i = 0; i < problemSize; i++)
            {
                var columnElement = nodes[i, node.yIndex];
                columnElement.domain.Remove(data);

                var rowElement = nodes[node.xIndex, i];
                rowElement.domain.Remove(data);
            }
        }

        private void UpdateDomainsForConstraints(GraphNode<int> node, int data)
        {
            for (int i = 0; i < node.outgoingEdges.Count; i++)
            {
                GraphEdge<int> outgoingEdge = node.outgoingEdges[i];
                GraphNode<int> destination = outgoingEdge.destinationNode;
                if (outgoingEdge.edgeType == GraphEdge<int>.EDGE_TYPE_ENUM.DESTINATION_GRATER)
                {
                    for (int j = 0; j < destination.domain.Count; j++)
                    {
                        if (destination.domain[j] < data)
                            destination.domain.RemoveAt(j);
                    }
                }
                else
                {
                    for (int j = 0; j < destination.domain.Count; j++)
                    {
                        if (destination.domain[j] > data)
                            destination.domain.RemoveAt(j);
                    }
                }
                
            }
        }

        public void InitializeAllDomains()
        {
            nodes.OfType<GraphNode<int>>().AsParallel().ForAll(i=>i.InitializeDomain(problemSize));
        }

        #region CLONING
        public FutoshikiGraph DeepClone()
        {
            FutoshikiGraph futoshikiGraph = new FutoshikiGraph(problemSize);

            CloneAllNodesToNewGraph(futoshikiGraph);
            CloneAllEdgesToNewGraph(futoshikiGraph);

            return futoshikiGraph;
        }

        private void CloneAllNodesToNewGraph(FutoshikiGraph futoshikiGraph)
        {
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    futoshikiGraph.nodes[i, j] = nodes[i, j].DeepClone();
                }
            }
        }

        private void CloneAllEdgesToNewGraph(FutoshikiGraph futoshikiGraph)
        {
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    for (int k = 0; k < nodes[i, j].outgoingEdges.Count; k++)
                    {
                        futoshikiGraph.nodes[i, j].outgoingEdges[k].sourceNode = futoshikiGraph.nodes[i, j];
                        GraphNode<int> oldDestination = nodes[i, j].outgoingEdges[k].destinationNode;
                        futoshikiGraph.nodes[i, j].outgoingEdges[k].destinationNode = futoshikiGraph.nodes[oldDestination.xIndex, oldDestination.yIndex];
                    }
                }
            }
        }
        #endregion

        public List<int> ReturnAllPossibilitiesForNode(GraphNode<int> node)
        {
            List<int> allPosibilites = InitializeAllPossibilities();
            List<int> itemsToRemove = RepeatedPossibilitiesInColumnOrRowOfElement(node, allPosibilites);

            RemoveProperItemsFromAllPossibilities(allPosibilites, itemsToRemove);
            itemsToRemove = PossibilitiesNotFulfillingConstraints(node, allPosibilites);

            RemoveProperItemsFromAllPossibilities(allPosibilites, itemsToRemove);

            return allPosibilites;

        }

        private List<int> InitializeAllPossibilities()
        {
            List<int> allPosibilites = new List<int>();
            for (int i = 0; i < problemSize; i++)
                allPosibilites.Add(i + 1);
            return allPosibilites;
        }

        private List<int> RepeatedPossibilitiesInColumnOrRowOfElement(GraphNode<int> node, List<int> allPosibilites)
        {
            List<int> itemsToRemove = new List<int>();

            for (int i = 0; i < problemSize; i++)
            {
                var columnElement = nodes[i, node.yIndex];
                if (allPosibilites.Contains(columnElement.data))
                    itemsToRemove.Add(columnElement.data);

                var rowElement = nodes[node.xIndex, i];
                if (allPosibilites.Contains(rowElement.data))
                    itemsToRemove.Add(rowElement.data);
            }

            return itemsToRemove;
        }
    
        private void RemoveProperItemsFromAllPossibilities(List<int> allPosibilites, List<int> itemsToRemove)
        {
            for (int i = 0; i < itemsToRemove.Count; i++)
                allPosibilites.Remove(itemsToRemove[i]);
        }

        private List<int> PossibilitiesNotFulfillingConstraints(GraphNode<int> node, List<int> allPosibilites)
        {
            List<int> itemsToRemove = new List<int>();
            for (int i = 0; i < node.outgoingEdges.Count; i++)
            {
                for (int j = 0; j < allPosibilites.Count; j++)
                {
                    CheckConstraint(node.outgoingEdges[i], allPosibilites[j], itemsToRemove);
                }
            }
            return itemsToRemove;
        }

        private void CheckConstraint(GraphEdge<int> graphEdge, int possibility, List<int> itemsToRemove)
        {
            if (graphEdge.edgeType == GraphEdge<int>.EDGE_TYPE_ENUM.DESTINATION_GRATER)
            {
                if (graphEdge.destinationNode.data != 0 && possibility > graphEdge.destinationNode.data)
                    itemsToRemove.Add(possibility);
            }
            else
            {
                if (graphEdge.destinationNode.data != 0 && possibility < graphEdge.destinationNode.data)
                    itemsToRemove.Add(possibility);
            }
        }

        public GraphNode<int> ChooseTheMostLimitedAndNotSet()
        {
            nodes.OfType<GraphNode<int>>().AsParallel().Where(i=>i.data==0).ForAll(i => i.measure = CalculateMeasure(i));
            var ordered = nodes.OfType<GraphNode<int>>().OrderByDescending(i => i.measure).Where(i => i.data == 0).ToList();
            if(ordered.Count == 0)
                return null;
            else
                return ordered.First();
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

        #region PRINTING
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
        #endregion
    }
}
