using UnityEngine;

namespace BattlefieldSimulator
{
    public struct GridPosition
    {
        public int x { get; private set; }
        public int z { get; private set; }

        public int q { get; private set; }
        public int r { get; private set; }
        public int s { get; private set; }

        private bool isFlatTopped;

        public GridPosition(int x, int z, bool isFlatTopped = false)
        {
            this.x = x;
            this.z = z;
            this.isFlatTopped = isFlatTopped;
            q = 0;
            r = 0;
            s = 0;

            OffsetToCube();
        }

        public GridPosition(int q, int r, int s, bool isFlatTopped = false)
        {
            this.q = q;
            this.r = r;
            this.s = s;
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

        public override string ToString()
        {
            return $"offset coordinary: (x: {x}, z: {z})\ncube Coordinary: (q: {q}, r: {r}, s: {s})\nIsFlatTopped: {isFlatTopped}";
        }
    }
}
