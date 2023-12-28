using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{    public class Test : MonoBehaviour
    {
        [SerializeField] private GeneratorData data;
        [SerializeField] private Transform terrainObj;
        private GridManager gridManager;

        private bool selectedFlag;
        private HexTile origin;
        private HexTile destination;
        private void Awake()
        {
            gridManager = new GridManager(data, terrainObj);
            selectedFlag = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            gridManager.gridSystem.GenerateMap();
            gridManager.gridSystem.InitTiles();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gridManager.gridSystem.TryGetHexTileByMousePosition(out HexTile hexTile))
                {
                    if (selectedFlag)
                    {
                        destination = hexTile;

                        if (gridManager.gridSystem.FindPath(origin, destination, out List<HexTile> path))
                        {
                            foreach (HexTile tile in path)
                            {
                                Debug.Log(tile);
                            }
                        }
                        else
                        {
                            Debug.Log("Unreachable!");
                        }

                        origin = null;
                        destination = null;
                        selectedFlag = false;
                    }
                    else
                    {
                        origin = hexTile;
                        selectedFlag = true;
                    }
                }
            }
        }
    }
}
