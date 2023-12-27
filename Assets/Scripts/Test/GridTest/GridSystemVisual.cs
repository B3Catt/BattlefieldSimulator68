using System.Collections;
using System.Collections.Generic;
using BattlefieldSimulator;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    public HexGrid hexGrid;
    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            hexGrid.x,
            hexGrid.z
        ];
        foreach (KeyValuePair<Vector2Int, HexTile> pair in hexGrid.tiles)
        {
            Vector2Int key = pair.Key;
            HexTile value = pair.Value;
            Transform gridSystemVisualSingleTransform =
                Instantiate(gridSystemVisualSinglePrefab, value.GetWorldPostion() + new Vector3(0, 1.5f, 0), Quaternion.identity);
            gridSystemVisualSingleArray[key.x, key.y] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
        }
        HideAllSingle();
    }

    public void HideAllSingle()
    {
        for (int x = 0; x < hexGrid.x; x++)
        {
            for (int z = 0; z < hexGrid.z; z++)
            {
                gridSystemVisualSingleArray[x,z].hide();
            }
        }
    }

    public void ShowTile(int x,int z)
    {
        gridSystemVisualSingleArray[x,z].show();
    }
}
