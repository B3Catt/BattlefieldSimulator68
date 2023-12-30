using System;
using UnityEngine;

public static class UtilsClass
{
    static public Vector3 ApplyRotation2VectorXZ(this Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        return q * source;
    }

    static public Vector3Int offsetToCube(Vector2Int offset)
    {
        var q = offset.x - (offset.y + (offset.y % 2)) / 2;
        var r = offset.y;
        return new Vector3Int(q, r, -q - r);
    }
}
