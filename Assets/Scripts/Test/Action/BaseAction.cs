using System;
using System.Collections.Generic;
using UnityEngine;
namespace BattlefieldSimulator
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;

        protected UnitTest unitTest;
        protected bool isActive = false;
        protected Action onActionComplete;
        protected virtual void Awake()
        {
            unitTest = GetComponent<UnitTest>();
        }
        public abstract string GetActionName();

        public abstract void TakeAction(HexTile hexTile, Action onActionComplete);
        public virtual bool IsValidActionGridPosition(HexTile gridPosition)
        {
            List<HexTile> validGridPositionList = GetValidActionGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }
        public abstract List<HexTile> GetValidActionGridPositionList();
        protected void ActionStart(Action onActionComplete)
        {
            unitTest.GetCurrentHexTile().ifEmpty = true;
            unitTest.GetCurrentHexTile().unitOnIt = null;
            isActive = true;
            this.onActionComplete = onActionComplete;

            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            unitTest.GetCurrentHexTile().ifEmpty = false;
            unitTest.GetCurrentHexTile().unitOnIt = unitTest;
            isActive = false;
            onActionComplete();
            unitTest.gridSystemVisual.UpdateGridVisual();

            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public virtual int GetActionPointsCost()
        {
            return 1;
        }
        public EnemyAIAction GetBestEnemyAIAction()
        {
            List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

            List<HexTile> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (HexTile gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);//对每一个enemy这个操作有效的hextile处理，来计算其value点数，并且加入到list
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)//选择其中value点数最高的作为此baseAction的bestEnemyAIAction来返回（同时返回此操作选择的Hextile和此操作BaseAction）
            {
                enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
                return enemyAIActionList[0];
            }
            else
            {
                // No possible Enemy AI Actions
                return null;
            }

        }
        public abstract EnemyAIAction GetEnemyAIAction(HexTile gridPosition);


    }
}