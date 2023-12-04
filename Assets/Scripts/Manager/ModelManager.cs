using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ModelManager()
        {
            // Read the data
            ReadData<ArmType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void ReadData<T>() where T : DataModel
        {
            try
            {
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

        public void UpdateData<T>() where T : DataModel
        {
            try
            {
                DataBaseHelper.OpenConnection();

                var dit = _allData[typeof(T).Name];

                foreach (var key in dit)
                {
                    DataBaseHelper.Update<T>(key.Value as T);
                }

                DataBaseHelper.CloseConnection();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public void DeleteData<T>(int id) where T : DataModel
        {

            try
            {
                DataBaseHelper.OpenConnection();
                DataBaseHelper.Delete<T>(id);
                DataBaseHelper.CloseConnection();

                _allData[typeof(T).Name].Remove(id);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
