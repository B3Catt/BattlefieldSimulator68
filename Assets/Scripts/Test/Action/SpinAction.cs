using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattlefieldSimulator
{
    public class SpinAction : BaseAction
    {

        private float totalSpinAmount;


        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount >= 360f)
            {
                ActionComplete();
            }
        }

        public override void TakeAction(HexTile gridPosition, Action onActionComplete)
        {
            totalSpinAmount = 0f;

            ActionStart(onActionComplete);
        }


        public override string GetActionName()
        {
            return "Spin";
        }

        public override List<HexTile> GetValidActionGridPositionList()
        {
            HexTile unitGridPosition = unitTest.GetCurrentHexTile();

            return new List<HexTile>
        {
            unitGridPosition
        };
        }

        public override int GetActionPointsCost()
        {
            return 1;
        }

    }
}