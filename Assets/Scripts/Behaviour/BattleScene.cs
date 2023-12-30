using UnityEngine;

namespace BattlefieldSimulator
{
    public class BattleScene : MonoBehaviour
    {

        [SerializeField] private GeneratorData data;
        [SerializeField] private Transform terrainObj;
        [SerializeField] private Transform visualObj;
        [SerializeField] private Transform visualSinglePrefab;

        [SerializeField] private Unit[] units;

        private void Awake()
        {
            InstanceManager.UnitManager = new UnitManager(units);
            InstanceManager.GridManager = new GridManager(data, terrainObj, visualObj, visualSinglePrefab);
            // 1. generate the map tiles and details, initialize tiles
            // 2. Initialize the grid visual system
            // 3. Initialize the unit, set the tiles they at
            // 4. Initialize the Unit systems, set the unit
            InstanceManager.GridManager.Intialize();
            InstanceManager.UnitManager.Initialize();
        }

        // Start is called before the first frame update
        void Start()
        {
        }
    }
}
