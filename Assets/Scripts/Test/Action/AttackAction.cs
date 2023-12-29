using System;
using System.Collections;
using System.Collections.Generic;
using BattlefieldSimulator;
using UnityEditor.Build;
using UnityEngine;
namespace BattlefieldSimulator
{
    public class AttackAction : BaseAction
    {
        public static event EventHandler<OnShootEventArgs> OnAnyShoot;

        public event EventHandler<OnShootEventArgs> OnShoot;

        public class OnShootEventArgs : EventArgs
        {
            public UnitTest targetUnit;
            public UnitTest shootingUnit;
        }
        private enum State
        {
            Aiming,
            Shooting,
            Cooloff,
        }
        private float stateTimer;
        private State state;
        private UnitTest targetUnit;
        private bool canShootBullet;
        public override string GetActionName()
        {
            return "Attack";
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            stateTimer -= Time.deltaTime;

            switch (state)
            {
                case State.Aiming:
                    Vector3 aimDir = (targetUnit.GetWorldPosition() - unitTest.GetWorldPosition()).normalized;
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                case State.Shooting:
                    if (canShootBullet)
                    {
                        Shoot();
                        canShootBullet = false;
                    }
                    break;
                case State.Cooloff:
                    break;
            }

            if (stateTimer <= 0f)
            {
                NextState();
            }
        }
        private void NextState()
        {
            switch (state)
            {
                case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                    break;
                case State.Shooting:
                    state = State.Cooloff;
                    float coolOffStateTime = 0.5f;
                    stateTimer = coolOffStateTime;
                    break;
                case State.Cooloff:
                    ActionComplete();
                    break;
            }
        }

        private void Shoot()
        {
            OnAnyShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = targetUnit,
                shootingUnit = unitTest
            });

            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = targetUnit,
                shootingUnit = unitTest
            });

            //TODO 
            targetUnit.Damage(40);

        }


        public override List<HexTile> GetValidActionGridPositionList()
        {
            List<HexTile> list = new List<HexTile>();
            foreach (KeyValuePair<Vector2Int, HexTile> pair in unitTest.hexGrid.tiles)
            {
                Vector2Int key = pair.Key;
                HexTile tile = pair.Value;
                if (tile.ifEmpty == true) continue;
                if (unitTest.GetCurrentHexTile() == tile ||//自身
                Vector2Int.Distance(tile.offsetCoordinate, unitTest.GetCurrentHexTile().offsetCoordinate) > unitTest.attackdistance) //超范围
                    continue;
                UnitTest targetUnit = tile.unitOnIt;


                if (targetUnit.IsEnemy() == unitTest.IsEnemy())
                {
                    // Both Units on same 'team'
                    continue;
                }
                list.Add(tile);
            }
            return list;
        }

        public override void TakeAction(HexTile hexTile, Action onActionComplete)
        {
            targetUnit = hexTile.unitOnIt;

            state = State.Aiming;
            float aimingStateTime = 1f;
            stateTimer = aimingStateTime;

            canShootBullet = true;

            ActionStart(onActionComplete);
        }
    }
}