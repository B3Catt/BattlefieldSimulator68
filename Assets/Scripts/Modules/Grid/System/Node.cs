using UnityEngine;

namespace BattlefieldSimulator
{
    public class Node
    {
        public Node parent;
        public HexTile target;
        public HexTile destination;
        public HexTile origin;

        public int baseCost;
        public int costFromOrigin;
        public int costToDestination;
        public int pathcost;
        public int heightcost;
        public Node(HexTile current, HexTile origin, HexTile destination, int pathcost)
        {
            parent = null;
            this.target = current;
            this.origin = origin;
            this.destination = destination;
            baseCost = 1;
            costFromOrigin = (int)Vector3Int.Distance(current.GridPosition.GetCubeCoordinate(), origin.GridPosition.GetCubeCoordinate());
            costToDestination = (int)Vector3Int.Distance(current.GridPosition.GetCubeCoordinate(), destination.GridPosition.GetCubeCoordinate());
            this.pathcost = pathcost;
            this.heightcost = 0;
        }

        public int Getcost()
        {
            return pathcost + baseCost + costFromOrigin + costToDestination + heightcost;
        }
        public void SetParent(Node node)
        {
            this.parent=node;
        }
    }
}