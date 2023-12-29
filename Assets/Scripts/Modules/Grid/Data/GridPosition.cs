using Org.BouncyCastle.Asn1.Esf;
using System;
using UnityEngine;

namespace BattlefieldSimulator
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int x { get; private set; }
        public int z { get; private set; }

        public int q { get; private set; }
        public int r { get; private set; }
        public int s { get; private set; }

        private bool isFlatTopped;
        public GridPosition(Vector2Int vector, bool isFlatTopped = false)
        {
            x = vector.x;
            z = vector.y;
            this.isFlatTopped = isFlatTopped;
            q = 0;
            r = 0;
            s = 0;

            OffsetToCube();
        }

        public GridPosition(Vector3Int vector, bool isFlatTopped = false)
        {
            q = vector.x;
            r = vector.y;
            s = vector.z;
            this.isFlatTopped = isFlatTopped;
            x = 0;
            z = 0;

            CubeToOffset();
        }

        public bool IsFlatTopped() { return isFlatTopped; }

        public void Set(int x, int z)
        {
            this.x = x;
            this.z = z;

            OffsetToCube();
        }

        public void Set(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;

            CubeToOffset();
        }

        private void OffsetToCube()
        {
            if (isFlatTopped)
            {
                UtilsClass.offsetToCubeFlatTop(x, z, out int q, out int r, out int s);
                this.q = q;
                this.r = r;
                this.s = s;
            }
            else
            {
                UtilsClass.offsetToCubePointTop(x, z, out int q, out int r, out int s);
                this.q = q;
                this.r = r;
                this.s = s;
            }
        }

        private void CubeToOffset()
        {
            if (isFlatTopped)
            {
                UtilsClass.CubeToOffsetFlatTop(q, r, out int x, out int z);
                this.x = x;
                this.z = z;
            }
            else
            {
                UtilsClass.CubeToOffsetPointTop(q, r, out int x, out int z);
                this.x = x;
                this.z = z;
            }
        }

        public Vector3Int GetCubeCoordinate() => new Vector3Int(q, r, s);

        public override string ToString()
        {
            return $"offset coordinary: (x: {x}, z: {z})\ncube Coordinary: (q: {q}, r: {r}, s: {s})\nIsFlatTopped: {isFlatTopped}";
        }

        public static GridPosition operator +(GridPosition left, Vector3Int right)
        {
            return new GridPosition(new Vector3Int(left.q, left.r, left.s) + right, left.isFlatTopped);
        }

        public static GridPosition operator +(GridPosition left, Vector2Int right)
        {
            return new GridPosition(new Vector2Int(left.x, left.z) + right, left.isFlatTopped);
        }

        public static bool operator ==(GridPosition left, GridPosition right) => (left.x == right.x && left.z == right.z);

        public static bool operator !=(GridPosition left, GridPosition right) => !(left == right);

        public override bool Equals(object obj)
        {
            return obj is GridPosition position &&
                   x == position.x &&
                   z == position.z &&
                   q == position.q &&
                   r == position.r &&
                   s == position.s &&
                   isFlatTopped == position.isFlatTopped;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, z, q, r, s, isFlatTopped);
        }

        public bool Equals(GridPosition other)
        {
            return this == other;
        }
    }
}
