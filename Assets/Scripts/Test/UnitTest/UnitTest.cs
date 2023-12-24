using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitTest : MonoBehaviour
    {
        private Vector3 targetPosition;

        [SerializeField] private Animator unitAnimator;

        private void Awake()
        {
            targetPosition = transform.position;
        }

        private void Update()
        {
            float stoppingDistance = .1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                unitAnimator.SetBool("IsMoving", true);
                Vector3 moveDirection = (targetPosition - transform.position).normalized;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

                float moveSpeed = 4f;
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
            }
            else
            {
                unitAnimator.SetBool("IsMoving", false);
            }
        }

        public void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }
    }
}

