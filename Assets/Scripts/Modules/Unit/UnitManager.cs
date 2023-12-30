using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitManager
    {

        private List<Unit> unitList;
        private List<Unit> friendlyUnitList;
        private List<Unit> enemyUnitList;

        public UnitManager(Unit[] units)
        {
            unitList = new List<Unit>();
            friendlyUnitList = new List<Unit>();
            enemyUnitList = new List<Unit>();

            foreach (Unit unit in units)
            {
                unitList.Add(unit);
                if (unit.IsEnemy()) enemyUnitList.Add(unit);
                else friendlyUnitList.Add(unit);
            }

            Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
            Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        }

        public void Initialize()
        {
            foreach(Unit unit in unitList)
            {
                unit.Initialize(unit.q, unit.r);
            }
        }

        private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            unitList.Add(unit);

            if (unit.IsEnemy())
            {
                enemyUnitList.Add(unit);
            }
            else
            {
                friendlyUnitList.Add(unit);
            }
        }

        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;

            unitList.Remove(unit);

            if (unit.IsEnemy())
            {
                Debug.Log("enemy die");
                enemyUnitList.Remove(unit);
            }
            else
            {
                Debug.Log("your army die");
                friendlyUnitList.Remove(unit);
            }
        }

        public List<Unit> GetUnitList()
        {
            return unitList;
        }

        public List<Unit> GetFriendlyUnitList()
        {
            return friendlyUnitList;
        }

        public List<Unit> GetEnemyUnitList()
        {
            return enemyUnitList;
        }
    }
}
