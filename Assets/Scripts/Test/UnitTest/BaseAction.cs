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
        protected bool isActive=false;
        protected Action onActionComplete;
        protected virtual void Awake()
        {
            unitTest = GetComponent<UnitTest>();
        }
        public abstract string GetActionName();

        public abstract void TakeAction(HexTile hexTile, Action onActionComplete);

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

            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}