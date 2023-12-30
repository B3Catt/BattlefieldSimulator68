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
            public Unit targetUnit;
            public Unit shootingUnit;
        }
        private enum State
        {
            Aiming,
            Shooting,
            Cooloff,
        }
        private float stateTimer;
        private State state;
        private Unit targetUnit;
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
                    Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
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
                shootingUnit = unit
            });

            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                targetUnit = targetUnit,
                shootingUnit = unit
            });


            targetUnit.Damage(5);

        }
        public override List<HexTile> GetValidActionGridPositionList()
        {
            HexTile unitGridPosition = unit.GetCurrentHexTile();
            return GetValidActionGridPositionList(unitGridPosition);
        }


        public List<HexTile> GetValidActionGridPositionList(HexTile hexTile)
        {
            List<HexTile> list = new List<HexTile>();
            for (int q = -unit.attackdistance; q <= unit.attackdistance; ++q)
            {
                for (int r = unit.attackdistance; r >= -unit.attackdistance; --r)
                {
                    if (q + r < -unit.attackdistance || q + r > unit.attackdistance || (q == 0 && r == 0))
                    {
                        continue;
                    }

                    GridPosition position = unit.GetCurrentHexTile().GridPosition + new Vector3Int(q, r, -q - r);
                    if (!GridSystem.Data.TryGetTileByGridPosition(position, out HexTile tile) || tile.ifEmpty == true)
                    {
                        continue;
                    }

                    Unit targetUnit = tile.unitOnIt;


                    if (targetUnit.IsEnemy() == unit.IsEnemy())
                    {
                        // Both Units on same 'team'
                        continue;
                    }
                    list.Add(tile);
                }
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

        public override EnemyAIAction GetEnemyAIAction(HexTile gridPosition)//可以attack时优先attack，设置value为最大
        {
            Unit targetUnit = gridPosition.unitOnIt;

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
            };
        }

        public int GetTargetCountAtPosition(HexTile gridPosition)//返回移动到此单元格所能攻击到的目标数
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }
    }
}