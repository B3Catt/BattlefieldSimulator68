
using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{

    [ExecuteInEditMode]
    public class HexTile : MonoBehaviour
    {
        public HexTileGenerationSetting settings;

        public HexTileGenerationSetting.TileType tileType;

        public GameObject tile;

        public GameObject fow;

        public Vector2Int offsetCoordinate;

        public Vector3Int cubeCoordinate;

        public List<HexTile> neighbours;

        private bool isDirty = false;

        private void OnValidate()
        {
            if (tile == null) { return; }
            isDirty = true;
        }

        private void Update()
        {
            if (!isActiveAndEnabled)
            {
                Destroy();
            }
            if (isDirty)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(tile);
                }
                else
                {
                    GameObject.DestroyImmediate(tile);
                }

                AddTile();
                isDirty = false;
            }
        }

        public void RollTileType()
        {
            tileType = (HexTileGenerationSetting.TileType)Random.Range(0, 3);
        }

        public void AddTile()
        {
            tile = GameObject.Instantiate(settings.GetTile(tileType));
            if (gameObject.GetComponent<MeshCollider>() == null)
            {
                //MeshCollider collider = gameObject.AddComponent<MeshCollider>();
                //collider.sharedMesh = GetComponentInChildren<MeshFilter>().mesh;
            }
            tile.transform.SetParent(transform, false);
        }

        public void Destroy()
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(tile);
                GameObject.Destroy(gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(tile);
                GameObject.DestroyImmediate(gameObject);
            }
        }

    }
}
