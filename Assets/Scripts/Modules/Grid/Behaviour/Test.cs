using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{    
    public class Test : MonoBehaviour
    {
        [SerializeField] private GeneratorData data;
        [SerializeField] private Transform terrainObj;
        [SerializeField] private Transform visualObj;
        [SerializeField] private Transform visualSinglePrefab;

        private GridManager gridManager;

        private bool selectedFlag;
        private HexTile origin;
        private HexTile destination;
        private void Awake()
        {
            gridManager = new GridManager(data, terrainObj, visualObj, visualSinglePrefab);
            selectedFlag = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            gridManager.Intialize();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
