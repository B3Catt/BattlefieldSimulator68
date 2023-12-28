using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class Pathfinder
    {
        private static int MAX_COST = 10000;
        private static float HEIGHT_COST_WEIGHT = 30;
        public static List<HexTile> FindPath(HexTile origin, HexTile destination, Vector2 heightBounds, bool isFlight)
        {
            Dictionary<HexTile, Node> nodesNotEvaluated = new Dictionary<HexTile, Node>();
            Dictionary<HexTile, Node> nodesAlreadyEvaluated = new Dictionary<HexTile, Node>();

            Node startNode = new Node(origin, origin, destination, 0);
            nodesNotEvaluated.Add(origin, startNode);

            bool gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, heightBounds, isFlight, out List <HexTile> path);
            while (!gotPath)
            {
                gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, heightBounds, isFlight, out path);
            }
            return path;
        }

        public static bool EvaluateNextNode(Dictionary<HexTile, Node> nodesNotEvaluated, Dictionary<HexTile, Node> nodesEvaluated, HexTile origin, HexTile destination, Vector2 heightBounds, bool isFlight, out List<HexTile> Path)
        {
            Node currentNode = GetCheapestNode(nodesNotEvaluated.Values.ToArray());

            if (currentNode == null)
            {
                Path = new List<HexTile>();
                return false;
            }

            nodesNotEvaluated.Remove(currentNode.target);
            nodesEvaluated.Add(currentNode.target, currentNode);

            Path = new List<HexTile>();
            if (currentNode.target == destination)
            {
                Path.Add(currentNode.target);
                while (currentNode.target != origin)
                {
                    Path.Add(currentNode.parent.target);
                    if(currentNode.Getcost() > MAX_COST)
                    {
                        Path=null;
                        return true;
                    }
                    currentNode = currentNode.parent;
                }
                return true;
            }

            List<Node> neighbours = new List<Node>();
            foreach (HexTile tile in currentNode.target.neighbours)
            {
                Node node = new Node(tile, origin, destination, currentNode.Getcost());
                node.baseCost += (tile.Disable || (tile.IsSeaTile ^ currentNode.target.IsSeaTile)) ? MAX_COST : 0;
                node.heightcost = isFlight ? 0 : GetHeightCost(tile.height, currentNode.target.height, heightBounds);
                neighbours.Add(node);
            }
            foreach (Node neighbour in neighbours)
            {
                if (nodesEvaluated.Keys.Contains(neighbour.target)) { continue; }

                if (neighbour.Getcost() < currentNode.Getcost() || !nodesNotEvaluated.Keys.Contains(neighbour.target))
                {
                    neighbour.SetParent(currentNode);
                    if (!nodesNotEvaluated.Keys.Contains(neighbour.target))
                    {
                        nodesNotEvaluated.Add(neighbour.target, neighbour);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Weight nearly equals to 2 ^ (e ^ (k * weight) - 1), where k equals to the ratio the height increase/decrease in total;
        ///     witch means that when baseN is nearly 13 ( = log2(MAX_COST)), it will return the height cost max(10000), when k_max = 2.639 / HEIGHT_COST_WEIGHT;
        ///     e.g.: 
        ///         HEIGHT_COST_WEIGHT = 30, total height difference = 10, then k_max = 0.088;
        ///         So if the height difference is greater than 0.88(k = 0.88 / 10), then it's unreachable;
        /// </summary>
        /// <param name="height"></param>
        /// <param name="height2"></param>
        /// <param name="heightBounds"></param>
        /// <returns></returns>
        private static int GetHeightCost(float height, float height2, Vector2 heightBounds)
        {
            int baseN = Mathf.RoundToInt(Mathf.Exp(Mathf.Abs((height - height2) / (heightBounds.y - heightBounds.x)) * HEIGHT_COST_WEIGHT) - 1);

            int cost = Mathf.Clamp(1 << Mathf.Clamp(baseN, 0, 30), 1, MAX_COST + 1);

            return cost;
        }

        private static Node GetCheapestNode(Node[] nodesNotEvaluated)
        {
            if (nodesNotEvaluated.Length == 0) { return null; }

            Node selectedNode = nodesNotEvaluated[0];

            for (int i = 1; i < nodesNotEvaluated.Length; i++)
            {
                var currentNode = nodesNotEvaluated[i];
                if (currentNode.Getcost() < selectedNode.Getcost())
                {
                    selectedNode = currentNode;
                }
                else if (currentNode.Getcost() == selectedNode.Getcost() && currentNode.costToDestination < selectedNode.costToDestination)
                {
                    selectedNode = currentNode;
                }
            }

            return selectedNode;
        }
    }
}