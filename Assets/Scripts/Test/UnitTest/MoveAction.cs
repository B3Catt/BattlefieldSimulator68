using System.Collections;
using System.Collections.Generic;
using BattlefieldSimulator;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private UnitTest unitTest;
    private Vector3 targetPosition;
    private float stoppingDistance = .1f;
    // Start is called before the first frame update
    [SerializeField] private Animator unitAnimator;

    private void Awake()
    {
        targetPosition = transform.position;
        unitTest = GetComponent<UnitTest>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Move(HexTile targetTile)
    {
        List<HexTile> Path = Pathfinder.FindPath(unitTest.GetCurrentHexTile(), targetTile);
        if (Path != null)
        {
            StartCoroutine(MoveThroughPath(Path));
        }
    }

    IEnumerator MoveThroughPath(List<HexTile> path)
    {
        for (int i = path.Count - 1; i >= 0; i--)
        {
            HexTile tile = path[i];
            yield return StartCoroutine(MoveToTile(tile));
            //移动结束，设置先现在的tile
            if (i == 0)
                unitTest.SetCurrentHexTile(tile);
        }
    }

    IEnumerator MoveToTile(HexTile hexTile)
    {
        targetPosition = hexTile.GetWorldPostion() + new Vector3(0f, 1f, 0f);

        while (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // 等待当前位置接近目标位置
            yield return null;
        }
    }
}
