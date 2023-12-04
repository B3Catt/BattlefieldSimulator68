using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Analytics;


namespace BattlefieldSimulator
{

    public class DataBaseHelper
    {
        /// <summary>
        /// 初始化连接
        /// </summary>
        private static string server = "127.0.0.1"; // MySQL 服务器地址
        //private static string database = "bs_data"; // 数据库名称
        private static string database = "battlefieldsimulator"; // 数据库名称

        private static string uid = "root"; // 用户名
        //private static string password = "Cf854122416!"; // 密码
        private static string password = "20020519"; // 密码

        private static string connectionString = $"Server={server};Database={database};Uid={uid};Pwd={password};charset=utf8";



        /// <summary>
        /// bt this method, we can create a list of instances of the certain table;
        ///     by using the REFLECTION tachnique, we can do this more readablely and simply;
        /// </summary>
        /// <typeparam name="T">must inherit from BaseModel</typeparam>
        /// <returns></returns>
        static public List<T> Traverse<T>() where T : DataModel
        {
            List<T> datalist = new List<T>();

            Type type = typeof(T);  // the reflection of the class T
            string name = GetTableName(type.Name);

            string query = $"SELECT * FROM {name.ToLower()}";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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
        /// 
        /// </summary>
        static public void Add<T>(T data) where T : DataModel
        {
            Type type = typeof(T);  // the reflection of the class T
            string name = GetTableName(type.Name);

            string query = $"INSERT INTO {name.ToLower()} (";
            string columns = "";
            string values = "";
            //编写符合T类型的sql语句
            foreach (var prop in type.GetProperties())
            {
                if (prop.Name.StartsWith("_"))
                {
                    columns += prop.Name.TrimStart('_') + ",";
                    values += $"@{prop.Name},";
                }
            }

            columns = columns.TrimEnd(',');
            values = values.TrimEnd(',');

            query += $"{columns}) VALUES ({values})";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var prop in type.GetProperties())
                    {
                        if (prop.Name.StartsWith("_"))
                        {
                            // 向sql语句中VALUE布冯的@auther等填充数据
                            command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(data));
                        }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static public void Delete<T>(int id) where T : DataModel
        {
            Type type = typeof(T);  // the reflection of the class T
            string name = GetTableName(type.Name);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"DELETE FROM {name} WHERE id = {id};";
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
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        static public void Update<T>(T data) where T : DataModel
        {
            Type type = typeof(T);  // the reflection of the class T
            string name = GetTableName(type.Name);
            string query = $"UPDATE {name.ToLower()} SET ";

            string primaryKey = "_id";

            foreach (var prop in type.GetProperties())
            {
                if (prop.Name.StartsWith("_") && prop.Name != primaryKey)
                {
                    query += $"{prop.Name.TrimStart('_')} = @{prop.Name},";
                }
            }
            query = query.TrimEnd(',');
            query += $" WHERE {primaryKey.TrimStart('_')} = @{primaryKey}";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    foreach (var prop in type.GetProperties())
                    {
                        if (prop.Name.StartsWith("_"))
                        {
                            command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(data));
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  we transfer the name of model class into name of the certain table
        ///  like "ArmType" => "arm_type"
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        static private string GetTableName(string className)
        {
            string name = "";
            List<string> ns = DataBaseHelper.SplitClassName(className);
            foreach (var n in ns)
            {
                name += n;
                name += '_';
            }
            name = name.TrimEnd('_');

            return name;
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

    }

}
