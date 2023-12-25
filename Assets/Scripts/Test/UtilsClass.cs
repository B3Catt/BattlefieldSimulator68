using System;
using UnityEngine;

namespace BattlefieldSimulator
{
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

        static public Vector2Int CubeToOffsetPointTop(Vector3Int cube)
        {
            var x = cube.x + (cube.y + (cube.y % 2)) / 2;
            var y = cube.y;
            return new Vector2Int(x, y);
        }

        static public Vector2Int CubeToOffsetFlatTop(Vector3Int cube)
        {
            var x = cube.x;
            var y = cube.y + cube.x / 2;
            return new Vector2Int(x, y);
        }
    }

}