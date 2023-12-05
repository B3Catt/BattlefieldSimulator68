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
        /// the cache data that
        ///     1. was already delete from _allData
        ///     2. but not delete from database yet;
        /// </summary>
        private Dictionary<string, List<int>> _dataToDelete;

        /// <summary>
        /// the cache data that
        ///     1. was already add into _allData
        ///     2. but not add into database yet;
        /// </summary>
        private Dictionary<string, Dictionary<int, DataModel>> _dataToAdd;

        /// <summary>
        /// 
        /// </summary>
        public ModelManager()
        {
            // Read the data
            _allData = new Dictionary<string, Dictionary<int, DataModel>>();
            _dataToDelete = new Dictionary<string, List<int>>();
            _dataToAdd = new Dictionary<string, Dictionary<int, DataModel>>();

            ReadData<ArmType>();
            //ReadData<User>();
        }

        /// <summary>
        /// by this method, we load the data from the database;
        ///     before reading, Flush the cache data into the database,
        ///     to ensure that we get the right data up to date;
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
                return null;
            }
            return _allData[typeof(T).Name][id] as T;
        }

        /// <summary>
        /// get all instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        // public Dictionary<int, T> GetData<T>() where T : DataModel
        // {
        //     if (!_allData.ContainsKey(typeof(T).Name))
        //     {
        //         return null;
        //     }
        //     return _allData[typeof(T).Name] as Dictionary<int, T>;
        //     var data = _allData[typeof(T).Name];
        //     // if (data is Dictionary<int, T> typedData)
        //     // {
        //     //     return typedData;
        //     // }
        //     // return null;
        // }
        public Dictionary<int, T> GetData<T>() where T : DataModel
        {
            if (!_allData.ContainsKey(typeof(T).Name))
            {
                return null;
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
                // if there's add_cache in certain model,
                //      delete after flushing the cache into database;
                if (_dataToAdd.ContainsKey(name))
                {
                    AddAllData<T>();
                }

                // delete the data from _allData, and add it to the delete cache
                if (!_dataToDelete.ContainsKey(name))
                {
                    _dataToDelete.Add(name, new List<int>());
                }
                _dataToDelete[name].Add(id);
                _allData[name].Remove(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// delete all data in the delete cache from database,
        ///     by this, we can decrease the times we connect to the database;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private bool DeleteAllData<T>() where T : DataModel
        {
            string name = typeof(T).Name;
            if (!_dataToDelete.ContainsKey(name))
            {
                return false;
            }

            try
            {
                DataBaseHelper.OpenConnection();

                var list = _dataToDelete[name];
                _dataToDelete.Remove(name);

                foreach (var id in list)
                {
                    DataBaseHelper.Delete<T>(id);
                }

                DataBaseHelper.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
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

            // Initialy, we check if there has the data with _id already,
            //      we check the _dataToAdd first as an optimizing
            if (_dataToAdd.ContainsKey(name) && _dataToAdd[name].ContainsKey(data._id))
            {
                return false;
            }

            if (_allData.ContainsKey(name) && _allData[name].ContainsKey(data._id))
            {
                return false;
            }

            // if there's delete_cache in certain model,
            //      add after flushing the cache into database;
            if (_dataToDelete.ContainsKey(name))
            {
                DeleteAllData<T>();
            }

            // add the data into _dataToAdd and _allData
            if (!_dataToAdd.ContainsKey(name))
            {
                _dataToAdd.Add(name, new Dictionary<int, DataModel>());
            }
            _dataToAdd[name].Add(data._id, data);

            if (!_allData.ContainsKey(name))
            {
                _allData.Add(name, new Dictionary<int, DataModel>());
            }
            _allData[name].Add(data._id, data);

            return true;
        }

        /// <summary>
        /// add all data in the add cache from database,
        ///     by this, we can decrease the times we connect to the database;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private bool AddAllData<T>() where T : DataModel
        {
            string name = typeof(T).Name;
            if (!_dataToAdd.ContainsKey(name))
            {
                return false;
            }

            try
            {
                DataBaseHelper.OpenConnection();

                var dit = _dataToAdd[name];
                _dataToAdd.Remove(name);

                foreach (var pair in dit)
                {
                    DataBaseHelper.Add(pair.Value as T);
                }

                DataBaseHelper.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
                // before we update the data, we need to clear the cache;
                //      as the result of AddData and DeleteData,
                //      we has sth. like mutex in _dataToAdd and _dataToDelete;
                //      So only on of the two flushes below we will do,
                //      which means you should not worry about the sequences;
                if (_dataToDelete.ContainsKey(name))
                {
                    DeleteAllData<T>();
                }

                if (_dataToAdd.ContainsKey(name))
                {
                    AddAllData<T>();
                }

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
                return false;
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
