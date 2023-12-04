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
            List<T> values = DataBaseHelper.Traverse<T>();
            Dictionary<int, DataModel> keyValuePairs = new Dictionary<int, DataModel>();
            foreach (T value in values)
            {
                keyValuePairs.Add(value._id, value);
            }

            Type type = typeof(T);
            if (_allData.ContainsKey(type.Name))
            {
                _allData[type.Name] = keyValuePairs;
                return;
            }

            _allData.Add(type.Name, keyValuePairs);
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

        public void WriteData<T>() where T : DataModel
        {

        }
    }
}
