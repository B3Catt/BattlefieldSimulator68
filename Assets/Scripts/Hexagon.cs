using UnityEngine;

public class Hexagon
{
    public float halfSize;
    public Vector3 centerPoint;
    public Vector3 upperRightCorner;
    public Vector3 upperLeftCorner;
    public Vector3 upperCorner;
    public Vector3 lowerRightCorner;
    public Vector3 lowerLeftCorner;
    public Vector3 lowerCorner;

    public Hexagon(Vector3 centerPoint, float halfSize)
    {
        this.centerPoint = centerPoint;
        this.halfSize = halfSize;

        upperCorner = centerPoint + new Vector3(0, 0, +1) * halfSize;
        lowerCorner = centerPoint + new Vector3(0, 0, -1) * halfSize;

        upperLeftCorner = centerPoint + new Vector3(-1, 0, 0.5f) * halfSize;
        upperRightCorner = centerPoint + new Vector3(1, 0, 0.5f) * halfSize;
        lowerLeftCorner = centerPoint + new Vector3(-1, 0, -0.5f) * halfSize;
        lowerRightCorner = centerPoint + new Vector3(-1, 0, -0.5f) * halfSize;
    }
}
