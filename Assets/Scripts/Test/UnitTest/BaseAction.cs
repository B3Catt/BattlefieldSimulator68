using System;
using UnityEngine;
namespace BattlefieldSimulator
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected UnitTest unitTest;
        protected bool isActive;
        protected Action onAnctionComplete;
        protected virtual void Awake()
        {
            unitTest = GetComponent<UnitTest>();
        }
        public abstract string GetActionName();
    }
}