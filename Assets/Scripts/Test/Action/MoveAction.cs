//using System;
//using System.Collections;
//using System.Collections.Generic;
//using BattlefieldSimulator;
//using UnityEditor.Build;
//using UnityEngine;

//public class MoveAction : BaseAction
//{
//    private Vector3 targetPosition;
//    private float stoppingDistance = .1f;
//    // Start is called before the first frame update
//    [SerializeField] private Animator unitAnimator;
//<<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Action/MoveAction.cs
//    public bool OnDev = false;

//    public Action onMoveOneTile;

//    protected override void Awake()
//    {
//        targetPosition = transform.position;
//        unit = GetComponent<Unit>();
//========
//    private bool onDev = false;
//    protected override void Awake()
//    {
//        unitTest = GetComponent<UnitTest>();
//>>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/Action/MoveAction.cs
//    }
//    void Start()
//    {
//        targetPosition = transform.position;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!isActive) return;
//        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
//        {
//            //unitAnimator.SetBool("IsMoving", true);

//            Vector3 moveDirection = (targetPosition - transform.position).normalized;
//            if (Vector3.Distance(transform.forward, moveDirection) > stoppingDistance)
//            {
//                float rotateSpeed = 10f;
//                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
//            }
//            else
//            {
//                float moveSpeed = 8f;
//                transform.position += moveDirection * Time.deltaTime * moveSpeed;
//            }
//        }
//        else
//        {
//<<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Action/MoveAction.cs
//            if (OnDev)
//========
//            if (onDev)
//>>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/Action/MoveAction.cs
//            {
//                ActionComplete();
//<<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Action/MoveAction.cs
//                OnDev = false;
//========
//                onDev=false;
//>>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/Action/MoveAction.cs
//            }

//        }
//    }

//    public override List<HexTile> GetValidActionGridPositionList()
//    {
//        List<HexTile> list = new List<HexTile>();
//        for (int q = -unit.movedistance; q <= unit.movedistance; ++q)
//        {
//            for (int r = unit.movedistance; r >= -unit.movedistance; --r)
//            {
//                if (q + r < -unit.movedistance || q + r > unit.movedistance || (q == 0 && r == 0))
//                {
//                    continue;
//                }

//                GridPosition position = unit.GetCurrentHexTile().GridPosition + new Vector3Int(q, r, -q - r);
//                if (!unit.GridManager.gridSystem.Data.TryGetTileByGridPosition(position, out HexTile tile))
//                {
//                    continue;
//                }

//                if(unit.GridManager.gridSystem.FindPath(unit.GetCurrentHexTile(), tile, out List<HexTile> Path, unit.isFlight) && Path.Count <= unit.movedistance + 1)
//                {
//                    list.Add(tile);
//                }
//            }
//        }
//        return list;
//    }

//    public override void TakeAction(HexTile targetTile, Action onActionComplete)
//    {
//<<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Action/MoveAction.cs
//        bool ifvalidmove = true;
//        if(!GetValidActionGridPositionList().Contains(targetTile)) ifvalidmove = false;
//        if (ifvalidmove)
//        {
//            unit.GridManager.gridSystem.FindPath(unit.GetCurrentHexTile(), targetTile, out List<HexTile> Path, unit.isFlight);
//            OnDev = true;
//            ActionStart(onActionComplete);
//            StartCoroutine(MoveThroughPath(Path));
//        }
//        else
//        {
//            Debug.Log("move unable");
//            ActionStart(onActionComplete);
//            ActionComplete();
//        }
//========
//        List<HexTile> Path = Pathfinder.FindPath(unitTest.GetCurrentHexTile(), targetTile);
//        ActionStart(onActionComplete);
//        StartCoroutine(MoveThroughPath(Path));
//>>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/Action/MoveAction.cs
//    }

//    IEnumerator MoveThroughPath(List<HexTile> path)
//    {
//        for (int i = path.Count - 1; i >= 0; i--)
//        {
//            isActive = true;
//            HexTile tile = path[i];
//            yield return StartCoroutine(MoveToTile(tile));
//            if (i == 0)
//            {
//<<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Action/MoveAction.cs
//                //移动结束，设置先现在的tile
//                OnDev = true;
//========
//                unitTest.SetCurrentHexTile(tile);
//                onDev=true;
//>>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/Action/MoveAction.cs
//            }
//            unit.SetCurrentHexTile(tile);

//            onMoveOneTile?.Invoke();
//        }
//    }

//    IEnumerator MoveToTile(HexTile hexTile)
//    {
//        targetPosition = unit.GridManager.gridSystem.GetLocalPositionForHexFromCoordinate(hexTile.GridPosition);

//        while (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
//        {
//            // 等待当前位置接近目标位置
//            yield return null;
//        }
//    }

//    public override string GetActionName()
//    {

//        return "Move";
//    }
//}
