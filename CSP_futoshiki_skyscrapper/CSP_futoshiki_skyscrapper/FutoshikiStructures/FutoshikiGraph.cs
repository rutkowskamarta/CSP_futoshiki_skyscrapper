using System;
using System.Collections.Generic;
using System.Text;
using CSP_futoshiki_skyscrapper.DataStructures;
using static System.Console;
using System.Linq;
using CSP_futoshiki_skyscrapper.CSP;
using static CSP_futoshiki_skyscrapper.Utils.Utilities;

namespace CSP_futoshiki_skyscrapper.FutoshikiStructures
{
    class FutoshikiGraph : Graph, ICSPSolvable
    {
        private delegate CSPNode choosingVariableHeuristicsDelegate();
        private choosingVariableHeuristicsDelegate choosingVariableHeuristicsMethod;

        private bool firstIteration = true;

        public override GraphNode[,] nodes { get => base.nodes; set => base.nodes = value; }

        public FutoshikiGraph(int problemSize) : base(problemSize)
        {
            if (HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.GREEDY)
            {
                choosingVariableHeuristicsMethod = ChooseFirstNotSet;
            }
            else if (HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.MOST_LIMITING)
            {
                choosingVariableHeuristicsMethod = ChooseTheMostLimitedAndNotSet;
            }
            else if(HEURISTIC_TYPE == HEURISTIC_TYPE_ENUM.SMALL_DOMAIN_AND_MANY_CONSTRAINTS)
            {
                choosingVariableHeuristicsMethod = ChooseTheSmallestDomainAndGreatestConstraints;
            }
        }

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
            firstIteration = false;
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
                if(firstIteration)
                    RemoveInitialNotFulfillingConstraints(node.outgoingEdges[i], itemsToRemove, allPossibilites);

                for (int j = 0; j < allPossibilites.Count; j++)
                {
                    CheckConstraint(node.outgoingEdges[i], allPossibilites[j], itemsToRemove);
                }
            }
            return itemsToRemove;
        }

        private void RemoveInitialNotFulfillingConstraints(GraphEdge graphEdge, List<int> itemsToRemove, List<int> allPossibilites)
        {
            if (graphEdge.edgeType == GraphEdge.EDGE_TYPE_ENUM.DESTINATION_GRATER)
            {
                if (allPossibilites.Contains(problemSize + 1))
                    itemsToRemove.Add(allPossibilites.Find(item => item == problemSize + 1));
            }
            else
            {
                if (allPossibilites.Contains(1))
                    itemsToRemove.Add(allPossibilites.Find(item => item == 1));
            }
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

        public CSPNode ChooseElementByHeuristics()
        {
            return choosingVariableHeuristicsMethod();
        }


        private CSPNode ChooseTheMostLimitedAndNotSet()
        {
            nodes.OfType<GraphNode>().AsParallel().Where(i=>i.data==0).ForAll(i => i.measure = CalculateMeasureMostLimited(i));
            var ordered = nodes.OfType<GraphNode>().OrderByDescending(i => i.measure).Where(i => i.data == 0).ToList();
            if(ordered.Count == 0)
                return null;
            else
                return ordered.First();
        }

        private CSPNode ChooseFirstNotSet()
        {
            return nodes.OfType<GraphNode>().Where(i => i.data == 0).ToList().First();
        }

        private CSPNode ChooseTheSmallestDomainAndGreatestConstraints()
        {

            var ordered = nodes.OfType<GraphNode>().Select(item=>item).Where(item=>item.data==0).OrderBy(i => i.domain.Count).ToList();
            if (ordered.Count == 0)
                return null;
            var elementsWithSmallestDomain = ordered.Select(item => item).Where(item => item.domain.Count == ordered[0].domain.Count);
            var elementsOrdered = elementsWithSmallestDomain.OrderByDescending(item => item.outgoingEdges.Count);
            return elementsOrdered.First();

            //tutuutututu return ordered.First();
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

        private int CalculateMeasureMostLimited(GraphNode node)
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
                StringBuilder stringBuilderLower = new StringBuilder();

                for (int j = 0; j < problemSize; j++)
                {
                    stringBuilder.Append(nodes[i, j].data);

                    GraphEdge leftEdge = null;
                    for (int k = 0; k < nodes[i,j].outgoingEdges.Count; k++)
                    {
                        if(nodes[i,j].outgoingEdges[k].destinationNode.xIndex == nodes[i,j].xIndex && nodes[i, j].outgoingEdges[k].destinationNode.yIndex == nodes[i, j].yIndex + 1)
                            leftEdge = nodes[i, j].outgoingEdges[k];
                    }

                    if (leftEdge == null)
                        stringBuilder.Append("   ");
                    else
                    {
                        if (leftEdge.edgeType == GraphEdge.EDGE_TYPE_ENUM.DESTINATION_GRATER)
                            stringBuilder.Append(" < ");
                        else
                            stringBuilder.Append(" > ");

                    }

                    GraphEdge lowerEdge = null;
                    for (int k = 0; k < nodes[i, j].outgoingEdges.Count; k++)
                    {
                        if (nodes[i, j].outgoingEdges[k].destinationNode.yIndex == nodes[i, j].yIndex && nodes[i, j].outgoingEdges[k].destinationNode.xIndex == nodes[i, j].xIndex + 1)
                            lowerEdge = nodes[i, j].outgoingEdges[k];
                    }

                    if (lowerEdge == null)
                        stringBuilderLower.Append("    ");
                    else
                    {
                        if (lowerEdge.edgeType == GraphEdge.EDGE_TYPE_ENUM.DESTINATION_GRATER)
                            stringBuilderLower.Append("^   ");
                        else
                            stringBuilderLower.Append("v   ");
                    }
                }
                stringBuilder.Append("\n");
                stringBuilder.Append(stringBuilderLower.ToString()+"\n");
                stringBuilderLower.Clear();

            }
            return stringBuilder.ToString();
        }

        public void PrintAllElements()
        {
            WriteLine(ToString());
        }

       

       
        #endregion
    }
}
