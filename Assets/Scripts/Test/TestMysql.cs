using System;
using System.Collections.Generic;
using UnityEngine;
using BattlefieldSimulator;

public class TestMysql : MonoBehaviour
{
    void Start()
    {

        //测试ADD
        ArmType test1 = new ArmType();
        //test1._id = 1;
        test1._auther = "1";
        test1._information = "1";
        test1._updateby = "1";
        test1._value = 1;
        test1._isable = false;
        test1._attack_distance = 1;
        test1._createtime = DateTime.Now.TimeOfDay;
        test1._updatetime = DateTime.Now.TimeOfDay;
        test1._name = "1";
        DataBaseHelper.Add<ArmType>(test1);

        //����updata
        test1._id = 1;
        test1._auther = "1wancong11111";
        test1._information = "123123131";
        test1._updateby = "1";
        test1._value = 1;
        test1._isable = false;
        test1._attack_distance = 1;
        test1._createtime = DateTime.Now.TimeOfDay;
        test1._updatetime = DateTime.Now.TimeOfDay;
        test1._name = "1";
        DataBaseHelper.Update<ArmType>(test1);

        //测试Traverse
        List<ArmType> dataList = DataBaseHelper.Traverse<ArmType>();
        foreach (ArmType data in dataList)
        {
            Debug.Log($"ID: {data._id}, Name: {data._name},author: {data._auther},isable: {data._isable},speed: {data._speed},createtime: {data._createtime}");
        }

        //测试search
        //Debug.Log($"id=52,auther={tes}");

        //测试DELETE
        //DataBaseHelper.Delete("arm_type",55);
    }
}