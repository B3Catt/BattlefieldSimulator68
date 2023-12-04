using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelManager
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, Dictionary<int, DataModel>> _allData;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, List<int>> _dataToDelete;

        /// <summary>
        /// 
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
        }

        /// <summary>
        /// 
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
            catch(Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetDataById<T>(int id) where T : DataModel
        {
            return _allData[typeof(T).Name][id] as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<int, T> GetData<T>() where T : DataModel
        {
            return _allData[typeof(T).Name] as Dictionary<int, T>;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteData<T>(int id) where T : DataModel
        {
            string name = typeof(T).Name;

            if (_allData.ContainsKey(name) && _allData[name].ContainsKey(id))
            {
                if (_dataToAdd.ContainsKey(name))
                {
                    AddAllData<T>();
                }

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
        /// 
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddData<T>(T data) where T : DataModel
        {
            string name = typeof(T).Name;

            if (_dataToAdd.ContainsKey(name) && _dataToAdd[name].ContainsKey(data._id))
            {
                return false;
            }

            if (_allData.ContainsKey(name) && _allData[name].ContainsKey(data._id))
            {
                return false;
            }

            if (_dataToDelete.ContainsKey(name))
            {
                DeleteAllData<T>();
            }

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
        /// 
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
                    DataBaseHelper.Add<T>(pair.Value as T);
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
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateData<T>(T data) where T : DataModel
        {
            string name = typeof(T).Name;

            if (_allData.ContainsKey(name) && _allData[name].ContainsKey(data._id))
            {
                if (_dataToDelete.ContainsKey(name))
                {
                    DeleteAllData<T>();
                }

                if (_dataToAdd.ContainsKey(name))
                {
                    AddAllData<T>();
                }

                _allData[name][data._id] = data;
                return true;
            }
            return false;
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
                    DataBaseHelper.Update<T>(pair.Value as T);
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
                MethodInfo method = typeof(ModelManager).GetMethod("UpdateAllData").MakeGenericMethod(type);
                method.Invoke(this, null);
            }
        }

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
