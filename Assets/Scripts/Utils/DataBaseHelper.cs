using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Analytics;

//查询到的数据就存在dataList里
//然后返回回去就返回dataList给别的脚本


namespace BattlefieldSimulator
{

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
        private static string database = "bs_data"; // 数据库名称
        private static string uid = "root"; // 用户名
        private static string password = "Cf854122416!"; // 密码
        // string constr = "server=127.0.0.1;User Id=root;password=Cf854122416!;Database=bs_data;charset=utf8";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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
        /// bt this method, we can create a list of instances of the certain chart;
        ///     by using the REFLECTION tachnique, we can do this more readablely and simply;
        /// </summary>
        /// <typeparam name="T">must inherit from BaseModel</typeparam>
        /// <returns></returns>
        static public List<T> Traverse<T>() where T : BaseModel
        {
            List<T> datalist = new List<T>();

            Type type = typeof(T);  // the reflection of the class T

            // we transfer the name of model class into name of the certain chart
            //      like "ArmType" => "arm_type"
            string name = "";
            List<string> ns = DataBaseHelper.SplitClassName(type.Name);
            foreach(var n in ns)
            {
                name += n;
                name += '_';
            }
            name = name.TrimEnd('_');

            string query = $"SELECT * FROM { name.ToLower() }";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // int id = Convert.ToInt32(reader["id"]);
                            // string name = reader["name"].ToString();
                            // Console.WriteLine($"ID: {id}, Name: {name}");

                            T data = Activator.CreateInstance<T>();     // create the instance of the corresponding class

                            // for every property in the class, set the value to the instance;
                            // By property of the class, we means like
                            /*
                             * public int _id { get; set; }
                             */
                            // but here's a new problem.
                            // To deal with the Constructor in the BaseModel, which will bring a bug in reader, we need to separate them apart;
                            // So I add '_' before the data from the database, just for eg., to tag them;
                                foreach (var p in type.GetProperties())
                            {
                                if (p.Name.StartsWith("_"))
                                {
                                    p.SetValue(data, reader[p.Name.TrimStart('_')]);
                                }
                            }
                            datalist.Add(data);
                        }
                    }
                }
            }
            return datalist;
        }

        /// <summary>
        /// this is a util function to split the name in upper camel case
        ///     eg: "ArmType" => {"Arm", "Type"}
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static private List<string> SplitClassName(string name)
        {
            List<string> ns = new List<string>();

            for (int i = 0, j = -1; i < name.Length; ++i)
            {
                if (name[i] > 'A' && name[i] < 'Z')
                {
                    ++j;
                }
                if (j == -1) ++j;
                if (ns.Count <= j) ns.Add("");
                ns[j] += name[i];
            }

            return ns;
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
