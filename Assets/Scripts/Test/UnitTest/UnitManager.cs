//using System;
//using System.Collections;
//using System.Collections.Generic;
//using BattlefieldSimulator;
//using UnityEngine;
//namespace BattlefieldSimulator
//{
//    public class UnitManager : MonoBehaviour
//    {

//        public static UnitManager Instance { get; private set; }


//        private List<UnitTest> unitList;
//        private List<UnitTest> friendlyUnitList;
//        private List<UnitTest> enemyUnitList;


//        private void Awake()
//        {
//            if (Instance != null)
//            {
//                Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
//                Destroy(gameObject);
//                return;
//            }
//            Instance = this;

//            unitList = new List<UnitTest>();
//            friendlyUnitList = new List<UnitTest>();
//            enemyUnitList = new List<UnitTest>();
//        }

//        private void Start()
//        {
//            UnitTest.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
//            UnitTest.OnAnyUnitDead += Unit_OnAnyUnitDead;
//        }

//        private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
//        {
//            UnitTest unit = sender as UnitTest;
//            unitList.Add(unit);

//            if (unit.IsEnemy())
//            {
//                enemyUnitList.Add(unit);
//            }
//            else
//            {
//                friendlyUnitList.Add(unit);
//            }
//        }

//        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
//        {
//            UnitTest unit = sender as UnitTest;

//            unitList.Remove(unit);

//            if (unit.IsEnemy())
//            {
//                Debug.Log("enemy die");
//                enemyUnitList.Remove(unit);
//            }
//            else
//            {
//                Debug.Log("your army die");
//                friendlyUnitList.Remove(unit);
//            }
//        }

//        public List<UnitTest> GetUnitList()
//        {
//            return unitList;
//        }

//        public List<UnitTest> GetFriendlyUnitList()
//        {
//            return friendlyUnitList;
//        }

//        public List<UnitTest> GetEnemyUnitList()
//        {
//            return enemyUnitList;
//        }

//    }
//}
