
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadingController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        AsyncOperation asyncOperation;

        /// <summary>
        /// 
        /// </summary>
        public LoadingController() : base()
        {
            GameApp.ViewManager.Register(ViewType.LoadingView, new ViewInfo()
            { 
                PrefabName = "LoadingView",
                controller = this,
                parentTf = GameApp.ViewManager.canvasTf
            });

            InitModuleEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitModuleEvent()
        {
            RegisterFunc(Defines.LoadingScene, loadSceneCallBack);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objects"></param>
        private void loadSceneCallBack(params System.Object[] args)
        {
            LoadingModel loadingModel = args[0] as LoadingModel;

            SetModel(loadingModel);

            GameApp.ViewManager.Open(ViewType.LoadingView);

            asyncOperation =  SceneManager.LoadSceneAsync(loadingModel.SceneName);

            asyncOperation.completed += onLoadedEndCallBack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        private void onLoadedEndCallBack(AsyncOperation op)
        {
            asyncOperation.completed -= onLoadedEndCallBack;

            GetModel<LoadingModel>().callback?.Invoke();

            GameApp.ViewManager.Close(ViewType.LoadingView);

            var go = GameObject.Find("Main Camera");

            var cam = go.GetComponent<Camera>();

            cam.enabled = false;
        }
    }
}
