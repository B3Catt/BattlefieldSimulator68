using System;
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
    private bool onDev = false;
    protected override void Awake()
    {
        unitTest = GetComponent<UnitTest>();
    }
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            //unitAnimator.SetBool("IsMoving", true);

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
            if (onDev)
            {
                ActionComplete();
                onDev=false;
            }

        }

    }

    public override List<HexTile> GetValidActionGridPositionList()
    {
        List<HexTile> list = new List<HexTile>();
        foreach (KeyValuePair<Vector2Int, HexTile> pair in unitTest.hexGrid.tiles)
        {
            Vector2Int key = pair.Key;
            HexTile tile = pair.Value;
            if (unitTest.GetCurrentHexTile() == tile || Vector2Int.Distance(tile.offsetCoordinate, unitTest.GetCurrentHexTile().offsetCoordinate) > unitTest.movedistance) continue;
            List<HexTile> Path = Pathfinder.FindPath(unitTest.GetCurrentHexTile(), tile);
            if (Path != null && Path.Count <= unitTest.movedistance + 1)
            {
                list.Add(tile);
                //gridSystemVisual.ShowTile(key.x, key.y);
            }
        }
        return list;
    }
    public override void TakeAction(HexTile targetTile, Action onActionComplete)
    {
        List<HexTile> Path = Pathfinder.FindPath(unitTest.GetCurrentHexTile(), targetTile);
        ActionStart(onActionComplete);
        StartCoroutine(MoveThroughPath(Path));
    }

    IEnumerator MoveThroughPath(List<HexTile> path)
    {
        for (int i = path.Count - 1; i >= 0; i--)
        {
            isActive = true;
            HexTile tile = path[i];
            yield return StartCoroutine(MoveToTile(tile));
            //移动结束，设置先现在的tile
            if (i == 0)
            {
                unitTest.SetCurrentHexTile(tile);
                onDev=true;
            }
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
