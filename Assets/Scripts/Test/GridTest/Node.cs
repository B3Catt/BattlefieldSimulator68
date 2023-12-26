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
        public Node(HexTile current, HexTile origin, HexTile destination, int pathcost)
        {
            parent = null;
            this.target = current;
            this.origin = origin;
            this.destination = destination;
            baseCost = 1;
            costFromOrigin = (int)Vector3Int.Distance(current.cubeCoordinate, origin.cubeCoordinate);
            costToDestination = (int)Vector3Int.Distance(current.cubeCoordinate, destination.cubeCoordinate);
            this.pathcost = pathcost;
        }

        public int Getcost()
        {
            return pathcost + baseCost + costFromOrigin + costToDestination;
        }
        public void SetParent(Node node)
        {
            this.parent=node;
        }
    }
}