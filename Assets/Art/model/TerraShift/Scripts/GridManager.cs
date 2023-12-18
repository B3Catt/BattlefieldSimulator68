using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    //following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this variable using unity editor
    public GameObject[] Hex;
    public GameObject house;
    //next two variables can also be instantiated using unity editor
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;

    public float perlScale = 1;
    public enum Tiles : int{ Void = 0, Grass = 1, Forest = 2, Water = 3, Mountain = 4, Desert = 5, Dirt = 6};

    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;

    private GameObject hexGridGO;

    public GameObject[,] map;

    //Set up the map
    void Start()
    {
        setSizes();
        createStart();
    }

    //Method to initialise Hexagon width and height
    void setSizes()
    {
        //renderer component attached to the Hex prefab is used to get the current width and height
        hexWidth = Hex[0].transform.GetComponent<MeshRenderer>().bounds.size.x;
        hexHeight = Hex[0].transform.GetComponent<MeshRenderer>().bounds.size.z;

        map = new GameObject[gridWidthInHexes, gridHeightInHexes];

        hexGridGO = new GameObject("HexGrid");
    }

    //Method to calculate the position of the first hexagon tile
    //The center of the hex grid is (0,0,0)
    Vector3 calcInitPos()
    {
        Vector3 initPos;
        //the initial position will be in the left upper corner
        initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0,
            gridHeightInHexes / 2f * hexHeight - hexHeight / 2);

        return initPos;
    }

    //method used to convert hex grid coordinates to game world coordinates
    public Vector3 calcWorldCoord(Vector2 gridPos)
    {
        //Position of the first hex tile
        Vector3 initPos = calcInitPos();
        //Every second row is offset by half of the tile width
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = initPos.x + offset + gridPos.x * hexWidth;
        //Every new line is offset in z direction by 3/4 of the hexagon height
        float z = initPos.z - gridPos.y * hexHeight * 0.75f;
        return new Vector3(x, 0, z);
    }

    //function to set the tile x y to a specific land type
    public void setHex(int type, int x, int y)
    {
        map[x, y].AddComponent<ScaleOut>().speed = 3f;
        GameObject hex = (GameObject)Instantiate(Hex[type]);
        Vector2 gridPos = new Vector2(x, y);
        hex.transform.position = calcWorldCoord(gridPos);
        if (type != (int)Tiles.Void) hex.transform.Translate(new Vector3(0, 0, Mathf.PerlinNoise(hex.transform.position.x * perlScale, hex.transform.position.z * perlScale)));
        hex.transform.parent = hexGridGO.transform;
        hex.GetComponent<Hex>().x = (int)x;
        hex.GetComponent<Hex>().y = (int)y;
        hex.GetComponent<Hex>().houses = new List<GameObject>();
        hex.name = (int)x + " | " + (int)y + " _" + hex.name;

        if(!map[x, y].name.Contains("Null") && !map[x, y].name.Contains("Void")) {
            hex.GetComponent<Hex>().greenLevel = map[x, y].GetComponent<Hex>().greenLevel;
            hex.GetComponent<Hex>().numHouses = 0;
            hex.GetComponent<Hex>().changeHouse(map[x, y].GetComponent<Hex>().numHouses);
            hex.GetComponent<Hex>().house = house;
        }
        map[(int)x, (int)y] = hex;
        hex.AddComponent<ScaleIn>().speed = 3f;

        //set neighbours to void tiles, if not already land tile
        if (type != (int)Tiles.Void) {
            GameObject[] n = getNeighbours(x, y);
            for (int i = 0; i < n.Length; i++)
            {
                if(n[i] == null)//end of map
                {

                }else if (n[i].name.Contains("Null"))
                {
                    setHex((int)Tiles.Void, n[i].GetComponent<Hex>().x, n[i].GetComponent<Hex>().y);
                }
            }
        }

    }

    //Neighbour Layout
    //  0 1
    // 2 X 3
    //  4 5

    //return the 6 neighbours of a tile in the map
    public GameObject[] getNeighbours(int x, int y)
    {
        GameObject[] n = new GameObject[6];
        if(y % 2 != 0)
        {
            if (x >=0 && y-1>=0)n[0] = map[x, y - 1];
            if (x+1 < gridWidthInHexes && y - 1 >= 0) n[1] = map[x + 1, y - 1];
            if (x-1 >= 0 && y >= 0) n[2] = map[x - 1, y];
            if (x+1 < gridWidthInHexes && y >= 0) n[3] = map[x + 1, y];
            if (x >= 0 && y + 1 < gridHeightInHexes) n[4] = map[x, y + 1];
            if (x+1 < gridWidthInHexes && y + 1 < gridHeightInHexes) n[5] = map[x + 1, y + 1];
        }
        else
        {
            if (x-1 >= 0 && y - 1 >= 0) n[0] = map[x - 1, y - 1];
            if (x >= 0 && y - 1 >= 0) n[1] = map[x, y - 1];
            if (x - 1 >= 0 && y >= 0) n[2] = map[x - 1, y];
            if (x + 1 < gridWidthInHexes && y >= 0) n[3] = map[x + 1, y];
            if (x-1 >= 0 && y + 1 < gridHeightInHexes) n[4] = map[x - 1, y + 1];
            if (x >= 0 && y + 1 < gridHeightInHexes) n[5] = map[x, y + 1];
        }
        
        return n;
    }

    //Set up the map and create a predefined Island for the game to start on
    void createStart()
    {
        // fill the map with null tiles
        for (float y = 0; y < gridHeightInHexes; y++)
        {
            for (float x = 0; x < gridWidthInHexes; x++)
            {
                GameObject hex = new GameObject();
                Vector2 gridPos = new Vector2(x, y);
                hex.transform.position = calcWorldCoord(gridPos);
                hex.transform.parent = hexGridGO.transform;
                hex.name = x + " | " + y + " _" + "Null";
                hex.AddComponent<Hex>();
                hex.GetComponent<Hex>().x = (int)x;
                hex.GetComponent<Hex>().y = (int)y;
                map[(int)x, (int)y] = hex;
            }
        }

        //Fill with 7 Tiles
        int cx = gridWidthInHexes / 2;
        int cy = gridHeightInHexes / 2;
        setHex((int)Tiles.Grass, cx, cy);
        setHex((int)Tiles.Mountain, cx - 1, cy - 1);
        setHex((int)Tiles.Grass, cx, cy - 1);
        setHex((int)Tiles.Desert, cx - 1, cy);
        setHex((int)Tiles.Forest, cx + 1, cy);
        setHex((int)Tiles.Dirt, cx - 1, cy + 1);
        setHex((int)Tiles.Forest, cx, cy + 1);

        //Fill middle tile with 3 houses
        map[cx, cy].transform.GetComponent<Hex>().changeHouse(3);
    }

}
