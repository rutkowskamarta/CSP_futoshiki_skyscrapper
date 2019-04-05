﻿using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.DataStructures;
using static System.Console;
using System.Linq;
using CSP_futoshiki_skyscrapper.CSP;

namespace CSP_futoshiki_skyscrapper.FutoshikiStructures
{
    class FutoshikiGraph : Graph, ICSPSolvable
    {
        public override GraphNode[,] nodes { get => base.nodes; set => base.nodes = value; }

        public FutoshikiGraph(int problemSize) : base(problemSize){}

        #region FORWARD_CHECKING_METHODS

        public bool IsAnyOfDomainsEmpty()
        {
            return nodes.OfType<GraphNode>().AsParallel().Any(i => i.domain.Count== 0);
        }

        public void AssignNewDataAndUpdateDomains(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
            UpdateAllDomains(nodes[xIndex, yIndex], newData);
        }

        private void UpdateAllDomains(GraphNode node, int newData)
        {
            UpdateDomainInRowsAndColumns(node, newData);
            UpdateDomainsForConstraints(node, newData);
        }

        private void UpdateDomainInRowsAndColumns(GraphNode node, int data)
        {
            for (int i = 0; i < problemSize; i++)
            {
                if (i != node.xIndex)
                {
                    var columnElement = nodes[i, node.yIndex];
                    if (columnElement.isMutable)
                        columnElement.domain.Remove(data);
                }

                if (i != node.yIndex)
                {
                    var rowElement = nodes[node.xIndex, i];
                    if(rowElement.isMutable)
                        rowElement.domain.Remove(data);
                }
                
            }
        }

        private void UpdateDomainsForConstraints(GraphNode node, int data)
        {
            
            for (int i = 0; i < node.outgoingEdges.Count; i++)
            {
                List<int> domainItemsToRemove = new List<int>();
                GraphEdge outgoingEdge = node.outgoingEdges[i];
                GraphNode destination = outgoingEdge.destinationNode;
                if (outgoingEdge.edgeType == GraphEdge.EDGE_TYPE_ENUM.DESTINATION_GRATER)
                {
                    for (int j = 0; j < destination.domain.Count; j++)
                    {
                        if (destination.domain[j] < data && destination.isMutable)
                            domainItemsToRemove.Add(destination.domain[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < destination.domain.Count; j++)
                    {
                        if (destination.domain[j] > data && destination.isMutable)
                            domainItemsToRemove.Add(destination.domain[j]);
                    }
                }

                foreach (var item in domainItemsToRemove)
                {
                    destination.domain.Remove(item);
                }
            }
        }

        public void InitializeAllDomains()
        {
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    nodes[i, j].domain = ReturnAllPossibilitiesForElement(nodes[i, j]);
                }
            }
        }
        #endregion

        public void AssignNewData(int xIndex, int yIndex, int newData)
        {
            nodes[xIndex, yIndex].data = newData;
        }

        #region CLONING
        public ICSPSolvable DeepClone()
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
                    futoshikiGraph.nodes[i, j] = (GraphNode) nodes[i, j].DeepClone();
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
                        GraphNode oldDestination = nodes[i, j].outgoingEdges[k].destinationNode;
                        futoshikiGraph.nodes[i, j].outgoingEdges[k].destinationNode = futoshikiGraph.nodes[oldDestination.xIndex, oldDestination.yIndex];
                    }
                }
            }
        }
        #endregion

        public List<int> ReturnAllPossibilitiesForElement(CSPNode element)
        {
            List<int> allPossibilites = InitializeAllPossibilities();
            List<int> itemsToRemove = RepeatedPossibilitiesInColumnOrRowOfElement((GraphNode)element, allPossibilites);

            RemoveProperItemsFromAllPossibilities(allPossibilites, itemsToRemove);
            itemsToRemove = PossibilitiesNotFulfillingConstraints((GraphNode)element, allPossibilites);

            RemoveProperItemsFromAllPossibilities(allPossibilites, itemsToRemove);

            return allPossibilites;

        }

        private List<int> InitializeAllPossibilities()
        {
            List<int> allPossibilites = new List<int>();
            for (int i = 0; i < problemSize; i++)
                allPossibilites.Add(i + 1);
            return allPossibilites;
        }

        private List<int> RepeatedPossibilitiesInColumnOrRowOfElement(GraphNode node, List<int> allPossibilites)
        {
            List<int> itemsToRemove = new List<int>();

            for (int i = 0; i < problemSize; i++)
            {
                var columnElement = nodes[i, node.yIndex];
                if (allPossibilites.Contains(columnElement.data))
                    itemsToRemove.Add(columnElement.data);

                var rowElement = nodes[node.xIndex, i];
                if (allPossibilites.Contains(rowElement.data))
                    itemsToRemove.Add(rowElement.data);
            }

            return itemsToRemove;
        }
    
        private void RemoveProperItemsFromAllPossibilities(List<int> allPossibilites, List<int> itemsToRemove)
        {
            for (int i = 0; i < itemsToRemove.Count; i++)
                allPossibilites.Remove(itemsToRemove[i]);
        }

        private List<int> PossibilitiesNotFulfillingConstraints(GraphNode node, List<int> allPossibilites)
        {
            List<int> itemsToRemove = new List<int>();
            for (int i = 0; i < node.outgoingEdges.Count; i++)
            {
                for (int j = 0; j < allPossibilites.Count; j++)
                {
                    CheckConstraint(node.outgoingEdges[i], allPossibilites[j], itemsToRemove);
                }
            }
            return itemsToRemove;
        }

        private void CheckConstraint(GraphEdge graphEdge, int possibility, List<int> itemsToRemove)
        {
            if (graphEdge.edgeType == GraphEdge.EDGE_TYPE_ENUM.DESTINATION_GRATER)
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

        public CSPNode ChooseTheMostLimitedAndNotSet()
        {
            nodes.OfType<GraphNode>().AsParallel().Where(i=>i.data==0).ForAll(i => i.measure = CalculateMeasure(i));
            var ordered = nodes.OfType<GraphNode>().OrderByDescending(i => i.measure).Where(i => i.data == 0).ToList();
            if(ordered.Count == 0)
                return null;
            else
                return ordered.First();
        }

        public bool IsSolved()
        {

            if (nodes.OfType<GraphNode>().AsParallel().Any(i => i.data == 0))
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

        private int CalculateMeasure(GraphNode node)
        {
            int rowMeasure = nodes.OfType<GraphNode>().Where(i => i.xIndex == node.xIndex && i.data != 0).Count();
            int columnMeasure = nodes.OfType<GraphNode>().Where(i => i.yIndex == node.yIndex && i.data != 0).Count();
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
