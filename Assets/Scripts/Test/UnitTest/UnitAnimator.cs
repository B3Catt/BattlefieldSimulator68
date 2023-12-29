using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattlefieldSimulator
{
    public class UnitAnimator : MonoBehaviour
    {

        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (TryGetComponent<MoveAction>(out MoveAction moveAction))
            {
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
            }

            if (TryGetComponent<AttackAction>(out AttackAction shootAction))
            {
                shootAction.OnShoot += ShootAction_OnShoot;
            }
        }

        private void Start()
        {
        }


        private void MoveAction_OnStartMoving(object sender, EventArgs e)
        {
            animator.SetBool("IsWalking", true);
        }

        private void MoveAction_OnStopMoving(object sender, EventArgs e)
        {
            animator.SetBool("IsWalking", false);
        }

        private void ShootAction_OnShoot(object sender, AttackAction.OnShootEventArgs e)
        {
            //TODO ATTACK动画
        }


    }
}
