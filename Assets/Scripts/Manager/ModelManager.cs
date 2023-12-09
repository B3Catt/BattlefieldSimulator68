using System;
using System.Collections.Generic;
using System.Reflection;

namespace BattlefieldSimulator
{
    /// <summary>
    /// by this manager, controller can get the data from database
    /// </summary>
    public class ModelManager
    {
        /// <summary>
        /// all data of all models
        /// </summary>
        private Dictionary<string, Dictionary<int, DataModel>> _allData;

        /// <summary>
        /// 
        /// </summary>
        public ModelManager()
        {
            // Read the data
            _allData = new Dictionary<string, Dictionary<int, DataModel>>();

            ReadData<ArmType>();
            ReadData<Terrain>();
            ReadData<TerrainModel>();
            //ReadData<User>();
        }

        /// <summary>
        /// by this method, we load the data from the database;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ReadData<T>() where T : DataModel
        {
            try
            {
                Flush<T>();

                // read the data
                DataBaseHelper.OpenConnection();
                List<T> values = DataBaseHelper.Traverse<T>();
                DataBaseHelper.CloseConnection();

                // convert it into dictionary
                Dictionary<int, DataModel> keyValuePairs = new Dictionary<int, DataModel>();
                foreach (T value in values)
                {
                    keyValuePairs.Add(value._id, value);
                }

                // add it into _allData
                Type type = typeof(T);
                if (_allData.ContainsKey(type.Name))
                {
                    _allData[type.Name] = keyValuePairs;
                    return;
                }

                _allData.Add(type.Name, keyValuePairs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get an instance by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDataById<T>(int id) where T : DataModel
        {
            if (!_allData.ContainsKey(typeof(T).Name))
            {
                ReadData<T>();
            }
            return _allData[typeof(T).Name][id] as T;
        }

        /// <summary>
        /// get all instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<int, T> GetData<T>() where T : DataModel
        {
            if (!_allData.ContainsKey(typeof(T).Name))
            {
                ReadData<T>();
            }

            Dictionary<int, T> _tempDic = new Dictionary<int, T>();

            foreach (var pair in _allData[typeof(T).Name])
            {
                _tempDic.Add(pair.Key, pair.Value as T);
            }

            return _tempDic;
        }

        /// <summary>
        /// delete data by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteData<T>(int id) where T : DataModel
        {
            string name = typeof(T).Name;

            if (_allData.ContainsKey(name) && _allData[name].ContainsKey(id))
            {
                try
                {
                    DataBaseHelper.OpenConnection();
                    DataBaseHelper.Delete<T>(id);
                    DataBaseHelper.CloseConnection();
                    _allData[name][id] = null;
                    _allData[name].Remove(id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddData<T>(T data) where T : DataModel
        {
            string name = typeof(T).Name;

            if (_allData.ContainsKey(name) && data._id != 0 && _allData[name].ContainsKey(data._id))
            {
                return false;
            }

            try
            {
                DataBaseHelper.OpenConnection();
                DataBaseHelper.Add(data);
                DataBaseHelper.CloseConnection();
                ReadData<T>();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private bool UpdateAllData<T>() where T : DataModel
        {
            string name = typeof(T).Name;
            if (!_allData.ContainsKey(name))
            {
                return false;
            }

            try
            {
                DataBaseHelper.OpenConnection();

                var dit = _allData[name];

                foreach (var pair in dit)
                {
                    DataBaseHelper.Update(pair.Value as T);
                }

                DataBaseHelper.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            foreach (var pair in _allData)
            {
                Type type = Type.GetType(pair.Key);
                MethodInfo method = typeof(ModelManager).GetMethod("Flush").MakeGenericMethod(type);
                method.Invoke(this, null);
                // this just work as "Flush<type>()", which you can't use
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Flush<T>() where T : DataModel
        {
            UpdateAllData<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            Flush();
        }
    }
}
