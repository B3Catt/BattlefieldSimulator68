using System;
using System.Collections.Generic;
using UnityEngine;
namespace BattlefieldSimulator
{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;

<<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Action/BaseAction.cs
        protected Unit unit;
        protected bool isActive=false;
========
        protected UnitTest unitTest;
        protected bool isActive = false;
>>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/Action/BaseAction.cs
        protected Action onActionComplete;
        protected virtual void Awake()
        {
            unit = GetComponent<Unit>();
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
            isActive = true;
            this.onActionComplete = onActionComplete;

            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }

        protected void ActionComplete()
        {
            isActive = false;
            onActionComplete();
            unitTest.gridSystemVisual.UpdateGridVisual();
            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public virtual int GetActionPointsCost()
        {
            return 1;
        }
    }
}