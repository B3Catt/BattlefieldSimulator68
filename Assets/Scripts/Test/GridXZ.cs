using UnityEngine;

public class GridXZ<TGridObject>
{
    private float cellSize;
    private Vector3 originPosition; 
    private TGridObject[,] gridArray;

    public GridXZ(int xnum, int znum, float cellSize, Vector3 originPosition, System.Func<GridXZ<TGridObject>, int, int, TGridObject> createObject)
    {
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.gridArray = new TGridObject[xnum, znum];

        for (int x = 0; x < gridArray.GetLength(0); ++x)
        {
            for (int z = 0; z < gridArray.GetLength(1); ++z)
            {
                gridArray[x, z] = createObject(this, x, z);
            }
        }
    }
}
