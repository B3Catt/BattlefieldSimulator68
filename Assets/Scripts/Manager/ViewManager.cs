using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string PrefabName;

        /// <summary>
        /// the parent transform of the view
        /// </summary>
        public Transform parentTf;

        /// <summary>
        /// the controller to witch the view belong
        /// </summary>
        public BaseController controller;

        /// <summary>
        /// 
        /// </summary>
        public int Sorting_Order;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ViewManager
    {
        /// <summary>
        /// 
        /// </summary>
        public Transform canvasTf;

        /// <summary>
        /// 
        /// </summary>
        public Transform worldCanvasTf;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, IBaseView> _opens;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, IBaseView> _viewCache;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, ViewInfo> _views;

        /// <summary>
        /// 
        /// </summary>
        public string canvasName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string worldCanvasName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectName"></param>
        public ViewManager(string canvasName, string worldCanvasName)
        {
            this.canvasName = canvasName;
            this.worldCanvasName = worldCanvasName;

            canvasTf = GameObject.Find(canvasName).transform;
            worldCanvasTf = GameObject.Find(worldCanvasName).transform;
            _opens = new Dictionary<int, IBaseView>();
            _viewCache = new Dictionary<int, IBaseView>();
            _views = new Dictionary<int, ViewInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewKey"></param>
        /// <param name="viewInfo"></param>
        public void Register(int viewKey, ViewInfo viewInfo)
        {
            if (_views.ContainsKey(viewKey) == false)
            {
                _views.Add(viewKey, viewInfo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="viewInfo"></param>
        public void Register(ViewType viewType, ViewInfo viewInfo)
        {
            Register((int)viewType, viewInfo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewKey"></param>

        public void Unregister(int viewKey)
        {
            if ( _views.ContainsKey(viewKey) )
            {
                _views.Remove(viewKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveView(int viewKey)
        {
            _views.Remove(viewKey);
            _viewCache.Remove(viewKey);
            _opens.Remove(viewKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public void RemoveViewByController(BaseController controller)
        {
            foreach (var item in _views)
            {
                if (item.Value.controller == controller)
                {
                    RemoveView(item.Key);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewKey"></param>
        /// <returns></returns>
        public bool IsOpen(int viewKey)
        {
            return _opens.ContainsKey(viewKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewKey"></param>
        /// <returns></returns>
        public IBaseView GetView(int viewKey)
        {
            if (_opens.ContainsKey(viewKey))
            {
                // the openned view has higher priority in searching
                return _opens[viewKey];
            }
            if (_viewCache.ContainsKey(viewKey))
            {
                return _viewCache[viewKey];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public IBaseView GetView(ViewType viewType)
        {
            return GetView((int)viewType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewKey"></param>
        /// <returns></returns>
        public T GetView<T>(int viewKey) where T : class, IBaseView
        {
            IBaseView view = GetView(viewKey);
            if (view != null)
            {
                return view as T;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public T GetView<T>(ViewType viewType) where T : class, IBaseView
        {
            return GetView<T>((int)viewType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewKey"></param>
        public void Destroy(int viewKey)
        {
            IBaseView oldView = GetView(viewKey);
            if (oldView != null)
            {
                Unregister(viewKey);
                oldView.DestroyView();
                _viewCache.Remove(viewKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewKey"></param>
        /// <param name="args"></param>
        public void Close(int viewKey, params System.Object[] args)
        {
            if (IsOpen(viewKey) == false)
            {
                return;
            }

            IBaseView view = GetView(viewKey);
            
            if (view != null)
            {
                _opens.Remove(viewKey);
                view.Close(args);
                _views[viewKey].controller.CloseView(view);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="args"></param>
        public void Close(ViewType viewType, params System.Object[] args)
        {
            Close((int)viewType, args);
        }

        /// <summary>
        /// openning the view with id includes below steps
        ///     1. get the view whose id equals the key, if there's the instance of the view in the cache;
        ///     2. else, create the instance of the view;
        ///     3. open the view
        /// </summary>
        /// <param name="viewKey"></param>
        /// <param name="args"></param>
        public void Open(int viewKey, params System.Object[] args)
        {
            // get the view instance from the cache
            IBaseView view = GetView(viewKey);
            ViewInfo info = _views[viewKey];

            // if there's not instance, create it, then add it to the cache
            if (view == null)
            {
                string type = ((ViewType)viewKey).ToString();
                GameObject uiObject = UnityEngine.Object.Instantiate(Resources.Load($"Views/{info.PrefabName}"), info.parentTf) as GameObject;
                Canvas canvas = uiObject.GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = uiObject.AddComponent<Canvas>();
                }

                if (uiObject.GetComponent<GraphicRaycaster>() == null)
                {
                    uiObject.AddComponent<GraphicRaycaster>();
                }

                canvas.overrideSorting = true; // open override sorting
                canvas.sortingOrder = info.Sorting_Order;

                view = uiObject.AddComponent(Type.GetType($"BattlefieldSimulator.{type}")) as IBaseView; // add scripts

                view.ViewId = viewKey;
                view.Controller = info.controller;

                _viewCache.Add(viewKey, view);
                info.controller.OnLoadView(view);
            }

            // open the view
            if (this._opens.ContainsKey(viewKey))
            {
                // already opened;
                return;
            }
            this._opens.Add(viewKey, view);

            if (view.IsInit())
            {
                view.SetVisible(true);
                view.Open(args);
                info.controller.OpenView(view);
            }
            else
            {
                view.InitUI();
                view.InitData();
                view.Open(args);
                info.controller.OpenView(view);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="args"></param>
        public void Open(ViewType viewType, params System.Object[] args)
        {
            Open((int)viewType, args);
        }
    }
}
