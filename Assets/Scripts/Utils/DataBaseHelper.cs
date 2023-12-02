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
        public int id { get; set; }
        public string name { get; set; }
        public string auther { get; set; }
        public string updateby { get; set; }
        public string information { get; set; }
        public bool isable { get; set; }
        public TimeSpan createtime { get; set; }
        public TimeSpan updatetime { get; set; }

        //。。。
    }

    public class Arm_typeDateModel : DataModel
    {
        public float speed { get; set; }
        public float value { get; set; }
        public int attack_distance { get; set; }
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
        static public string Search(int id, string tablename, string searchname)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string data = null;
                //List<Arm_typeDateModel> datalist = new List<Arm_typeDateModel>();
                connection.Open();
                string query = $"SELECT {searchname} FROM {tablename} WHERE id = {id}";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = reader.GetString(0);
                        }
                    }
                }
                return data;
            }
        }

        static public List<Arm_typeDateModel> TraversalArm_type(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                List<Arm_typeDateModel> datalist = new List<Arm_typeDateModel>();
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
                            Arm_typeDateModel data = new Arm_typeDateModel();
                            data.id = Convert.ToInt32(reader["id"]);
                            data.name = Convert.ToString(reader["name"]);
                            data.auther = Convert.ToString(reader["auther"]);
                            data.updateby = Convert.ToString(reader["updateby"]);
                            data.information = Convert.ToString(reader["information"]);
                            data.attack_distance = Convert.ToInt32(reader["attack_distance"]);
                            data.speed = Convert.ToSingle(reader["speed"]);
                            data.value = Convert.ToSingle(reader["value"]);
                            data.isable = Convert.ToBoolean(reader["isable"]);
                            //data.createtime=Convert.ToDateTime(reader["createtime"]);
                            data.createtime = ((TimeSpan)reader["createtime"]);
                            data.updatetime = ((TimeSpan)reader["updatetime"]);
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
        static public void Arm_typeAdd(Arm_typeDateModel model)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string createtimeString = model.createtime.ToString(@"hh\:mm\:ss");
                string updatetimeString = model.updatetime.ToString(@"hh\:mm\:ss");
                //string query = $"INSERT INTO arm_type(auther,updateby,createtime,updatetime,isable,name,information,speed,value,attack_distance) VALUES ({model.auther},{model.updateby},{createtimeString},{updatetimeString},{model.isable},{model.name},{model.information},{model.speed},{model.value},{model.attack_distance});";
                string query = $"INSERT INTO arm_type (auther, updateby, createtime, updatetime, isable, name, information, speed, value, attack_distance) VALUES ('{model.auther}', '{model.updateby}', '{createtimeString}', '{updatetimeString}', {model.isable}, '{model.name}', '{model.information}', {model.speed}, {model.value}, {model.attack_distance})";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                    
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static public void Delete(string tablename,int id)
        {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query=$"DELETE FROM {tablename} WHERE id = {id};";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>

    }
}
