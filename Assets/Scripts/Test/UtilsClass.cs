using System;
using UnityEngine;

public static class UtilsClass
{
    static public Vector3 ApplyRotation2VectorXZ(this Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        return q * source;
    }

    /// <summary>
    /// even-r horizontal
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    static public Vector3Int offsetToCubePointTop(Vector2Int offset)
    {
        var q = offset.x - (offset.y + (offset.y % 2)) / 2;
        var r = offset.y;
        return new Vector3Int(q, r, -q - r);
    }

    /// <summary>
    /// odd-q vertical
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    static public Vector3Int offsetToCubeFlatTop(Vector2Int offset)
    {
        var q = offset.x;
        var r = offset.y - offset.x / 2;
        return new Vector3Int(q, r, -q - r);
    }
}
