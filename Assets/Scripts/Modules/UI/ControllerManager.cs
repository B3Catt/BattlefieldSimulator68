using System;
using System.Collections.Generic;
using System.Linq;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class ControllerManager
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, BaseController> _modules;

        /// <summary>
        /// 
        /// </summary>
        public ControllerManager() 
        {
            _modules = new Dictionary<int, BaseController>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitAllMoudles()
        {
            foreach(var module in _modules)
            {
                module.Value.Init();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <param name="controller"></param>
        public void Register(int controllerKey, BaseController controller)
        {
            if (_modules.ContainsKey(controllerKey) == false)
            {
                _modules.Add(controllerKey, controller);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="controller"></param>
        public void Register(ControllerType controllerType, BaseController controller)
        {
            Register((int)controllerType, controller);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        public void UnRegister(int controllerKey)
        {
            if ( _modules.ContainsKey(controllerKey) )
            {
                _modules.Remove(controllerKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() { _modules.Clear(); }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAllModules()
        {
            List<int> keys = _modules.Keys.ToList();
            foreach (int key in keys)
            {
                _modules[key].Destroy();
                _modules.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void ApplyFunc(int controllerKey, string eventName, params Object[] args)
        {
            if (_modules.ContainsKey(controllerKey))
            {
                _modules[controllerKey].ApplyFunc(eventName, args);
            }
        }

        public void ApplyFunc(ControllerType type, string eventName, params Object[] args)
        {
            ApplyFunc((int)type, eventName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <returns></returns>
        public BaseModel GetControllerModel(int controllerKey)
        {
            if (_modules.ContainsKey(controllerKey))
            {
                return _modules[controllerKey].GetModel();
            }
            return null;
        }
    }
}
