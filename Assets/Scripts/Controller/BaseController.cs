﻿

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.UIElements;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, System.Action<object[]>> message;

        /// <summary>
        /// 
        /// </summary>
        protected BaseModel model;

        /// <summary>
        /// 
        /// </summary>
        public BaseController() 
        {
            message = new Dictionary<string, System.Action<object[]>>();
        }

        /// <summary>
        /// initialize the controller after getting registered
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public virtual void OnLoadView(IBaseView view)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public virtual void OpenView(IBaseView view)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public virtual void CloseView(IBaseView view)
        {

        }

        /// <summary>
        /// register the event of the model
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void RegisterFunc(string eventName, System.Action<object[]> callback)
        {
            if (message.ContainsKey(eventName))
            {
                message[eventName] += callback;
            }
            else
            {
                message.Add(eventName, callback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        public void UnRegisterFunc(string eventName)
        {
            if (message.ContainsKey(eventName))
            {
                message.Remove(eventName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void ApplyFunc(string eventName, params object[] args)
        {
            if (message.ContainsKey(eventName))
            {
                message[eventName].Invoke(args);
            }
            else
            {
                Debug.Fail("error:" + eventName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void ApplyControllerFunc(int controllerKey, string eventName, params object[] args)
        {
            GameApp.ControllerManager.ApplyFunc(controllerKey, eventName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerType"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void ApplyControllerFunc(ControllerType controllerType, string eventName, params object[] args)
        {
            ApplyControllerFunc((int)controllerType, eventName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void SetModel(BaseModel model)
        {
            this.model = model;
            this.model.Controller = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BaseModel GetModel() { return this.model; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModel<T>() where T : BaseModel
        {
            return model as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <returns></returns>
        public BaseModel GetControllerModel(int controllerKey)
        {
            return GameApp.ControllerManager.GetControllerModel(controllerKey);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Destroy()
        {
            RemoveModuleEvent();
            RemoveGlobalEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void InitModuleEvent()
        {
            RegisterFunc(Defines.OpenView, openView);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RemoveModuleEvent()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void InitGlobalEvent()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RemoveGlobalEvent() 
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void openView(System.Object[] args)
        {
            GameApp.ViewManager.Open(int.Parse(args[0].ToString()), args.Skip(1).ToArray());
        }
    }
}
