using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class HexGridSystem
    {
        private Transform terrainObj;

        private GeneratorData generatorData;
        public GeneratorData GeneratorData { get => generatorData; }

        private HexGridMapData data;

        public int seed {  get; private set; }
        public HexGridSystem(GeneratorData generatorData, Transform terrainObj)
        {
            this.generatorData = generatorData;
            this.terrainObj = terrainObj;
            data = new HexGridMapData(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitTiles()
        {
            if (data == null)
            {
                return;
            }

            foreach (HexTile hexTile in data.GetAllTiles())
            {
                hexTile.RegisterNeighbours();
            }
        }

        public bool FindPath(HexTile origin, HexTile destination, out List<HexTile> path)
        {
            path = Pathfinder.FindPath(origin, destination,
                new Vector2(generatorData.topografiaSettings.seaLevel, generatorData.topografiaSettings.heightScale), true);
            if (path == null) { return false; }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateMap()
        {
            HexGridMapGenerator.GenerateMap(this, generatorData, terrainObj, data);
            TextureGenerator.UpdateColours(generatorData.graphicsSettings);
            UpdateMaterial();
            EditParentObject(generatorData.topografiaSettings.baseScale);

            void UpdateMaterial()
            {
                //TODO: Add shader to visualize coordinate system option(cubic&grid)
                //TODO: Selected tile highlighter
                //TODO: Coordinate axis highlighter
                generatorData.graphicsSettings.terrainMaterial.SetVector("_heightBounds", new Vector2(generatorData.topografiaSettings.seaLevel + 1, generatorData.topografiaSettings.heightScale + 1));
                generatorData.graphicsSettings.terrainMaterial.SetFloat("_mainSeaLevel", generatorData.topografiaSettings.seaLevel);
                generatorData.graphicsSettings.terrainMaterial.SetInt("_seaTiles", 1);
                generatorData.graphicsSettings.terrainMaterial.SetFloat("_baseScale", generatorData.topografiaSettings.baseScale);
            }

            void EditParentObject(float scale)
            {
                terrainObj.transform.localScale = Vector3.one * scale;
                terrainObj.transform.localEulerAngles = generatorData.topografiaSettings.rotation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public List<HexTile> GetNeighbours(HexTile tile)
        {
            List<HexTile> neighbours = new List<HexTile>();
            Vector3Int[] neighbourCoordinary = new Vector3Int[]
                {
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(1, 0, -1),
                    new Vector3Int(0, 1, -1),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 0, 1),
                    new Vector3Int(0, -1, 1)
                };


            foreach (Vector3Int coord in neighbourCoordinary)
            {
                GridPosition gridPosition = tile.GridPosition;

                if (data.TryGetTileByGridPosition(gridPosition + coord, out HexTile neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <returns></returns>
        public Vector3 GetLocalPositionForHexFromCoordinate(GridPosition gridPosition)
        {
            float xPosition;
            float zPosition;
            float yPosition;
            float size = generatorData.topografiaSettings.baseScale;

            if ((data?.TryGetTileByGridPosition(gridPosition, out HexTile hexTile)) ?? false)
            {
                yPosition = hexTile.height;
            }
            else
            {
                yPosition = 0;
            }

            xPosition = (gridPosition.q + gridPosition.r * .5f) * (HexMetrics.innerRadius * 2f) * size;
            zPosition = gridPosition.r * (HexMetrics.outerRadius * 1.5f) * size;

            return new Vector3(xPosition, yPosition, -zPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexTile"></param>
        /// <returns></returns>
        public bool TryGetHexTileByMousePosition(out HexTile hexTile)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue, generatorData.tileLayer))
            {
                Transform objectHit = hit.transform;

                if (objectHit.TryGetComponent<HexTile>(out HexTile target))
                {
                    hexTile = target;
                    return true;
                }
            }
            hexTile = null;
            return false;
        }

        public bool CheckPosition(GridPosition gridPosition)
        {
            return gridPosition.q >= -generatorData.topografiaSettings.numTilePerEdge &&
                gridPosition.q <= generatorData.topografiaSettings.numTilePerEdge &&
                gridPosition.r >= -generatorData.topografiaSettings.numTilePerEdge &&
                gridPosition.r <= generatorData.topografiaSettings.numTilePerEdge &&
                gridPosition.s >= -generatorData.topografiaSettings.numTilePerEdge &&
                gridPosition.s <= generatorData.topografiaSettings.numTilePerEdge;
        }
    }
}
