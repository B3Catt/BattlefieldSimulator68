using System.Collections;
using System.Collections.Generic;
using BattlefieldSimulator;
using UnityEditor.Build;
using UnityEngine;

public class MoveAction : BaseAction
{
    private Vector3 targetPosition;
    private float stoppingDistance = .1f;
    // Start is called before the first frame update
    [SerializeField] private Animator unitAnimator;
    public bool IfMoving;
    protected override void Awake()
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
            IfMoving = true;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (Vector3.Distance(transform.forward, moveDirection) > stoppingDistance)
            {
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            }
            else
            {
                float moveSpeed = 8f;
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
            }
        }
        else
        {
            unitAnimator.SetBool("IsMoving", false);
            IfMoving = false;
        }

    }

    public void Move(HexTile targetTile)
    {
        bool ifvalidmove = true;
        List<HexTile> Path = Pathfinder.FindPath(unitTest.GetCurrentHexTile(), targetTile);
        if (targetTile == unitTest.GetCurrentHexTile()) ifvalidmove = false;//重叠
        if (Path.Count > unitTest.movedistance + 1 || Path == null) ifvalidmove = false;//超出距离（忽略自身的一格）
        if (ifvalidmove)
        {
            StartCoroutine(MoveThroughPath(Path));
        }
        else
        {
            Debug.Log("move unable");
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

    public override string GetActionName()
    {
        
        return "Move";
    }
}
