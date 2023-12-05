
using UnityEngine;
using UnityEngine.UI;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class StartView : BaseView
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();

            Find<Button>("startBtn").onClick.AddListener(onStartGameBtn);
            Find<Button>("mapBtn").onClick.AddListener(onMapCustomBtn);
            Find<Button>("loginBtn").onClick.AddListener(onLoginBtn);
            Find<Button>("setBtn").onClick.AddListener(onSetBtn);
            Find<Button>("quitBtn").onClick.AddListener(onQuitGameBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onStartGameBtn()
        {
            LoadingModel loadingModel = new LoadingModel();
            loadingModel.SceneName = "map";
            loadingModel.callback = delegate ()
            {

            };

            GameApp.ViewManager.Close(ViewId);

            // 
            Controller.ApplyControllerFunc(ControllerType.Loading, Defines.LoadingScene, loadingModel);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onMapCustomBtn()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void onLoginBtn()
        {
            /// TODO: open the login view
        }

        /// <summary>
        /// 
        /// </summary>
        private void onSetBtn()
        {
            ApplyFunc(Defines.OpenView, (int)ViewType.SetView);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onQuitGameBtn()
        {
            Controller.ApplyControllerFunc(ControllerType.UI, Defines.OpenView, (int)ViewType.MessageView,
                new MessageInfo()
                {
                    okCallback = delegate ()
                    {
                        Application.Quit();
                    },
                    MsgTxt = "确定要退出游戏吗？"
                }
            );
        }
    }
}
