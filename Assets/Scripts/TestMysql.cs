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
    // 建立连接语句
    // charset=utf8这句要写，不然可能会报错                                 
    string constr = "server=127.0.0.1;User Id=root;password=20020519;Database=battlefieldsimulator;charset=utf8";
    //建立连接
    public static MySqlConnection mycon;
    void Start()
    {
        //ConnectMysql();
        //SearchMysql();
        //UpadteMysql();
        List<Arm_typeDateModel> dataList = DataBaseHelper.TraversalArm_type("SELECT * FROM arm_type");
        foreach (Arm_typeDateModel data in dataList)
        {
            Debug.Log($"ID: {data.id}, Name: {data.name},author: {data.auther},isable: {data.isable},speed: {data.speed},createtime: {data.createtime}");
        }
        string tes=DataBaseHelper.Search(52,"arm_type","auther");
        Debug.Log($"id=52,auther={tes}");
    }
    private void ConnectMysql()
    {
        string constr = "server=127.0.0.1;User Id=root;password=20020519;Database=battlefieldsimulator;charset=utf8";
        //建立连接
        mycon = new MySqlConnection(constr);
        //打开连接
        mycon.Open();
        bool isOK = mycon.Ping();
        if (isOK)
        {
            Debug.Log("数据库已连接");
        }
        else
        {
            Debug.Log("数据库连接错误");
        }
    }
    private void SearchMysql()
    {
        //查询数据
        string selstr = "select * from arm_type";
        MySqlCommand myselect = new MySqlCommand(selstr, mycon);
        DataSet ds = new DataSet();
        try
        {
            MySqlDataAdapter da = new MySqlDataAdapter(selstr, mycon);
            da.Fill(ds);
            Console.WriteLine("数据库第一行数据:\n");
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                Debug.Log(ds.Tables[0].Rows[0][i]);
            }
        }
        catch (Exception e)
        {
            throw new Exception("SQL:" + selstr + "\n" + e.Message.ToString());
        }
    }
    private void UpadteMysql()
    {
        //修改数据
        MySqlCommand cmd = new MySqlCommand();
        try
        {
            cmd.Connection = mycon;
            cmd.CommandText = "UPDATE studentscores SET name = @name WHERE guid = @guid";
            Debug.Log("取出guid=1的元组，更改属性为name=C#Test");
            String name = "C#Test";
            String guid = "1";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@guid", guid);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            mycon.Close();
        }
    }
}
