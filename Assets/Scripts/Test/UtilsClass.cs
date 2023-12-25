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

        static public void offsetToCubePointTop(int x, int z, out int q, out int r, out int s)
        {
            q = x - (z + (z % 2)) / 2;
            r = z;
            s = -q - r;
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

        static public void offsetToCubeFlatTop(int x, int z, out int q, out int r, out int s)
        {
            q = x;
            r = z - x / 2;
            s = -q - r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        /// <returns></returns>
        static public Vector2Int CubeToOffsetPointTop(Vector3Int cube)
        {
            var x = cube.x + (cube.y + (cube.y % 2)) / 2;
            var y = cube.y;
            return new Vector2Int(x, y);
        }

        static public void CubeToOffsetPointTop(int q, int r, out int x, out int z)
        {
            x = q + (r + (r % 2)) / 2;
            z = r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cube"></param>
        /// <returns></returns>
        static public Vector2Int CubeToOffsetFlatTop(Vector3Int cube)
        {
            var x = cube.x;
            var y = cube.y + cube.x / 2;
            return new Vector2Int(x, y);
        }

        static public void CubeToOffsetFlatTop(int q, int r, out int x, out int z)
        {
            x = q;
            z = r + q / 2;
        }
    }

}