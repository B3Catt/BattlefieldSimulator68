using Polygen.HexagonGenerator;
using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class HexGridMapGenerator
    {
        //Generate map with details and scripts
        public static void GenerateMap(HexGridSystem gridSystem, GeneratorData data, Transform parent, HexGridMapData mapData)
        {
            Initialize(data, parent, out Vector2 heightBounds, out GameObject detailParent);

            for (int q = -data.topografiaSettings.numTilePerEdge; q <= data.topografiaSettings.numTilePerEdge; q++)
            {
                for (int r = data.topografiaSettings.numTilePerEdge; r >= -data.topografiaSettings.numTilePerEdge; r--)
                {
                    if ((q + r < -data.topografiaSettings.numTilePerEdge) || (q + r > data.topografiaSettings.numTilePerEdge))
                    {
                        continue;
                    }

                    GridPosition gridPosition = new GridPosition(new Vector3Int(q, r, -q - r));

                    Vector3 position = gridSystem.GetLocalPositionForHexFromCoordinate(gridPosition) / data.topografiaSettings.baseScale;

                    // Random set the hight data
                    float height = data.noiseSettings.Evaluate(data.seed, q, r, data.topografiaSettings.offset, data.topografiaSettings.baseNoiseScale) * data.topografiaSettings.heightScale;
                    position.y = height;
                    bool isSeaTile = height < data.topografiaSettings.seaLevel;

                    // Instantiate tile gameobject at given position
                    GameObject tileObj = InstantiateTile(data, parent, position, isSeaTile);
                    tileObj.name = "Tile_ " + gridPosition.q + "_" + gridPosition.r + "_" + gridPosition.s;

                    // Get biome data for tile
                    /// TODO:
                    /// 
                    BiomeDataSettings biomeData = GetBiomeDataSettings(data, heightBounds, position, isSeaTile, out float tileHeightRatio);

                    // Generate details for tile
                    /// TODO:
                    /// 
                    bool disable = InstantiateDetails(data, detailParent.transform, q, r, position, biomeData, isSeaTile);

                    // Assign and init HexagonTile monobehaviour component
                    HexTile tile = GenerateTileScript(height, isSeaTile, tileObj, gridPosition, biomeData, tileHeightRatio, disable);
                    if (!mapData.TryAddTileByGridPosition(gridPosition, tile))
                    {
                        Debug.LogError("Add Tile False!");
                    }
                }
            }

            HexTile GenerateTileScript(float height, bool isSeaTile, GameObject tileObj, GridPosition gridPosition, BiomeDataSettings biomeData, float tileHeightRatio, bool disable)
            {
                HexTile tile = tileObj.AddComponent<HexTile>();
                tile.Initialize(biomeData, gridPosition, height, GetTileColor(), isSeaTile, gridSystem, disable);
                return tile;

                Color GetTileColor()
                {
                    return isSeaTile ? data.graphicsSettings.waterBaseColor : data.GetColorByHeight(tileHeightRatio);
                }
            }
        }

        #region Helpers
        public static void Initialize(GeneratorData data, Transform parent, out Vector2 heightBounds, out GameObject detailParent)
        {
            heightBounds = new Vector2(data.topografiaSettings.seaLevel, data.topografiaSettings.heightScale);
            detailParent = new GameObject("Detail Parent");
            detailParent.transform.SetParent(parent);
            detailParent.transform.localPosition = Vector3.zero;
        }

        private static BiomeDataSettings GetBiomeDataSettings(GeneratorData data, Vector2 heightBounds, Vector3 position, bool isSeaTile, out float tileHeight)
        {
            tileHeight = Mathf.InverseLerp(heightBounds.x, heightBounds.y, position.y);
            BiomeDataSettings biomeData = isSeaTile ? data.graphicsSettings.seaBiomeData : data.graphicsSettings.GetBiomeData(tileHeight);
            return biomeData;
        }

        private static GameObject InstantiateTile(GeneratorData data, Transform parent, Vector3 position, bool isSeaTile)
        {
            //Create local variable for tile gameobject
            GameObject tileObj;

            //Instantiate tile gameobject and assign parent transform and material
            if (isSeaTile)
            {
                position.y = data.topografiaSettings.seaLevel;
                tileObj = Object.Instantiate(data.topografiaSettings.seaPrefab, parent);
                tileObj.GetComponentInChildren<Renderer>().sharedMaterial = data.graphicsSettings.waterMaterial;
            }
            else
            {
                tileObj = Object.Instantiate(data.topografiaSettings.tilePrefab, parent);
                tileObj.GetComponentInChildren<Renderer>().sharedMaterial = data.graphicsSettings.terrainMaterial;
            }

            //Set height and position for tile
            tileObj.transform.localScale += Vector3.up * position.y;
            tileObj.transform.localPosition = position;

            //Add collider if selected
            var collider = tileObj.AddComponent<MeshCollider>();
            collider.sharedMesh = tileObj.GetComponentInChildren<MeshFilter>().sharedMesh;

            tileObj.layer = RuntimeExtensions.ToLayer(data.tileLayer);
            tileObj.isStatic = data.staticTiles;

            return tileObj;
        }

        private static bool InstantiateDetails(GeneratorData data, Transform parent, int x, int z, Vector3 position, BiomeDataSettings biomeData, bool isSeaTile)
        {
            if (biomeData != null && biomeData.details != null)
            {
                BiomeDataSettings.DetailData detailData = data.graphicsSettings.fixedDetails ? biomeData.details.GetRandomWeightedPrefab(x, z, data.seed, biomeData.detailDensity * data.graphicsSettings.baseDetailDensity * 2) : biomeData.details.GetRandomWeightedPrefab(biomeData.detailDensity * data.graphicsSettings.baseDetailDensity * 2);
                if (detailData != null && detailData.detailPrefab != null)
                {
                    if (data.noiseSettings.Evaluate(data.graphicsSettings.baseDetailSeed + data.seed, x, z, data.topografiaSettings.offset + data.graphicsSettings.detailOffset, data.graphicsSettings.baseDetailNoiseScale * biomeData.biomeDetailNoiseScale) < detailData.density * biomeData.detailDensity * data.graphicsSettings.baseDetailDensity)
                    {
                        int variantCount = Random.Range(0, detailData.maxVariantCount);
                        for (int i = 0; i <= variantCount; i++)
                        {
                            GameObject detailObj = Object.Instantiate(detailData.detailPrefab, parent);
                            detailObj.transform.localScale = Vector3.one * data.graphicsSettings.baseDetailScale * biomeData.biomeDetailScale * detailData.detailScale;
                            detailObj.transform.localScale += new Vector3(detailData.variantOffset, detailData.variantOffset, detailData.variantOffset) * Random.Range(-.1f, .1f);
                            detailObj.transform.localEulerAngles += new Vector3(detailData.variantOffset, detailData.variantOffset, detailData.variantOffset) * Random.Range(-5, 5);

                            if (isSeaTile)
                                detailObj.transform.localPosition = new Vector3(position.x, data.topografiaSettings.seaLevel, position.z) + new Vector3(detailData.variantOffset * Random.Range(-.5f, .5f), detailData.verticalOffset, detailData.variantOffset * Random.Range(-.5f, .5f));
                            else
                                detailObj.transform.localPosition = position + new Vector3(detailData.variantOffset * Random.Range(-.5f, .5f), detailData.verticalOffset, detailData.variantOffset * Random.Range(-.5f, .5f));

                            if (detailData.randomizeRotation)
                                detailObj.transform.localEulerAngles += Vector3.up * Random.Range(0, 360);

                            detailObj.AddComponent<MeshCollider>();

                            detailObj.layer = RuntimeExtensions.ToLayer(data.detailLayer);
                            detailObj.isStatic = data.staticDetails;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
