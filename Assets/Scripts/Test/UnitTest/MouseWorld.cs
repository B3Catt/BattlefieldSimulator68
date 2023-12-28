using BattlefieldSimulator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSSLG;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
    //返回当前射线所碰撞的HexTile
    public static HexTile GetHexTile()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            HexTile hexTile = hit.collider.GetComponent<HexTile>();
            if (hexTile != null)
            {
                return hexTile;
            }
            else return null;
        }
        else return null;
    }
}
