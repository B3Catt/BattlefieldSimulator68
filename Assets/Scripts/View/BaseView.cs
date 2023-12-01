using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseView : MonoBehaviour, IBaseView
    {
        /// <summary>
        /// 
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseController Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Canvas _canvas;

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, GameObject> m_cache_gos = new Dictionary<string, GameObject>();

        /// <summary>
        /// 
        /// </summary>
        private bool _isInit = false;

        /// <summary>
        /// 
        /// </summary>
        void Awake()
        {
            _canvas = gameObject.GetComponent<Canvas>();
            OnAwake();
        }

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            OnStart();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnAwake()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStart()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void ApplyControllerFunc(int controllerKey, string eventName, params object[] args)
        {
            this.Controller.ApplyControllerFunc(controllerKey, eventName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void ApplyFunc(string eventName, params object[] args)
        {
            this.Controller.ApplyFunc(eventName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public virtual void Close(params object[] args)
        {
            SetVisible(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroyView()
        {
            Controller = null;
            Destroy(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void InitData()
        {
            _isInit = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void InitUI()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsInit()
        {
            return _isInit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsShow()
        {
            return _canvas.enabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Open(params object[] args)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetVisible(bool value)
        {
            this._canvas.enabled = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public GameObject Find(string res)
        {
            if (m_cache_gos.ContainsKey(res) == false)
            {
                m_cache_gos.Add(res, transform.Find(res).gameObject);
            }
            return m_cache_gos[res];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="res"></param>
        /// <returns></returns>
        public T Find<T>(string res) where T : Component
        {
            GameObject obj = Find(res);
            return  obj.GetComponent<T>();
        }
    }
}
