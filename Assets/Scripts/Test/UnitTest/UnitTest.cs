using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitTest : MonoBehaviour
    {
        private Vector3 targetPosition;

        private void Update()
        {
            float stoppingDistance = .1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                float moveSpeed = 4f;
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Move(MouseWorld.GetPosition());
            }
        }

        private void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }
    }
}

