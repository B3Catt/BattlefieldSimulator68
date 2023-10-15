using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    private int xnum = 10;
    private int znum = 10;
    private float cellSize = 10f;
    public class GridObject
    {
        private GridXZ<GridObject> grid;

        private int x;
        private int z;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {

        }
    }
    private GridXZ<GridObject> grid;

    private void Awake()
    {
        grid = new GridXZ<GridObject>(xnum, znum, cellSize, Vector3.zero, (grid, x, z) => 
        {
            return new GridObject(grid, x, z);
        });
    }
}
