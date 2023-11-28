using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using UnityEngine;

//查询到的数据就存在dataList里
//然后返回回去就返回dataList给别的脚本


namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class DataModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //。。。
    }


    public class DataBaseHelper
    {
        /// <summary>
        /// 初始化连接
        /// </summary>
        private static string server = "127.0.0.1"; // MySQL 服务器地址
        private static string database = "battlefieldsimulator"; // 数据库名称
        private static string uid = "root"; // 用户名
        private static string password = "20020519"; // 密码
        // string constr = "server=127.0.0.1;User Id=root;password=20020519;Database=battlefieldsimulator;charset=utf8";
        private static string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};charset=utf8";
    
        static public List<DataModel> Read(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                List<DataModel> datalist = new List<DataModel>();
                connection.Open();
                //string query = "SELECT * FROM arm_type";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // int id = Convert.ToInt32(reader["id"]);
                            // string name = reader["name"].ToString();
                            // Console.WriteLine($"ID: {id}, Name: {name}");
                            DataModel data = new DataModel();
                            data.ID = reader.GetInt32(0);
                            data.Name = reader.GetString(1);
                            datalist.Add(data);
                        }
                    }
                }
                return datalist;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        static public void Update()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        static public void Delete()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        static public void Add()
        {

        }
    }
}
