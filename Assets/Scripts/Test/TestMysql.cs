using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using UnityEngine;
using BattlefieldSimulator;

public class TestMysql : MonoBehaviour
{
    void Start()
    {

        //测试ADD
        Arm_typeDateModel test1=new Arm_typeDateModel();
        test1.id = 1;
        test1.auther="1";
        test1.information="1";
        test1.updateby="1";
        test1.value=1;
        test1.isable=false;
        test1.attack_distance=1;
        test1.createtime=DateTime.Now.TimeOfDay;
        test1.updatetime=DateTime.Now.TimeOfDay;
        test1.name="1";
        //DataBaseHelper.Arm_typeAdd(test1);

        //测试Traversal
        List<ArmType> dataList = DataBaseHelper.Traverse<ArmType>();
        foreach (ArmType data in dataList)
        {
            Debug.Log($"ID: {data._id}, Name: {data._name},author: {data._auther},isable: {data._isable},speed: {data._speed},createtime: {data._createtime}");
        }

        //测试search
        string tes=DataBaseHelper.Search(52,"arm_type","auther");
        //Debug.Log($"id=52,auther={tes}");

        //测试DELETE
        //DataBaseHelper.Delete("arm_type",55);
    }
}