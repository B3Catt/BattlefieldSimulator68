
using System.Collections.Generic;
using UnityEditor;
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

        public TileManager tileManager;

        public Vector2Int offsetCoordinate;
        public Vector3Int cubeCoordinate;

        public GridPosition gridPosition;

        public List<HexTile> neighbours;

        public float radius;

        public float height;

        private bool isDirty = false;

        private void OnValidate()
        {
            if (tile == null) { return; }
            isDirty = true;
        }

        public void InitializeTile(TileManager manager)
        {
            tileManager = manager;
            //tileManager.Awake(); 
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
            tile = GameObject.Instantiate(settings.Mesh);
            Material mat = settings.GetTileMaterial(tileType);
            tile.transform.SetParent(transform, false);
            tile.GetComponent<MeshRenderer>().material = mat;
            if (gameObject.GetComponent<MeshCollider>() == null)
            {
                MeshCollider collider = gameObject.AddComponent<MeshCollider>();
#if UNITY_EDITOR
                collider.sharedMesh = GetComponentInChildren<MeshFilter>().sharedMesh;
#else
                collider.sharedMesh = GetComponentInChildren<MeshFilter>().mesh;
#endif
            }

            transform.localScale = (Vector3.forward + Vector3.right) * radius + Vector3.up * height;
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

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position, 0.1f);
            foreach (HexTile neighbour in neighbours)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }

        public void OnHighlightTile()
        {
            tileManager.OnHighlightTile(this);
        }
        public void OnSelcetTile()
        {
            tileManager.OnSelectTile(this);
        }
        public Vector3 GetWorldPostion()
        {
            HexTile hexTile;
            //Vector2Int key = new Vector2Int(x, z);
            Vector2Int key = offsetCoordinate;
            if (tileManager.tiles.TryGetValue(key, out hexTile))
            {
                return hexTile.transform.position;
            }
            else
            {
                return new Vector3(0,0,0);
            }

        }
    }
}
