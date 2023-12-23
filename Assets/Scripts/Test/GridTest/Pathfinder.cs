using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;

namespace BattlefieldSimulator
{
    public class Pathfinder
    {
        public static List<HexTile> FindPath(HexTile origin, HexTile destination)
        {
            Dictionary<HexTile, Node> nodesNotEvaluated = new Dictionary<HexTile, Node>();
            Dictionary<HexTile, Node> nodesAlreadyEvaluated = new Dictionary<HexTile, Node>();

            Node startNode = new Node(origin, origin, destination, 0);
            nodesNotEvaluated.Add(origin, startNode);

            bool gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out List<HexTile> path);
            while (!gotPath)
            {
                gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out path);
            }
            return path;
        }

        public static bool EvaluateNextNode(Dictionary<HexTile, Node> nodesNotEvaluated, Dictionary<HexTile, Node> nodesEvaluated, HexTile origin, HexTile destination, out List<HexTile> Path)
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
                    if(currentNode.Getcost()>10000) 
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

                if (tile.tileType != HexTileGenerationSetting.TileType.Standard)
                {
                    // should change it to height distance
                    node.baseCost = 9999999;
                }
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