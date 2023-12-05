
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class StartView : BaseView
    {
        public GameObject loginBtn;
        public GameObject nicknameText;
        public GameObject logoutBtn;

        protected override void OnAwake()
        {
            base.OnAwake();
            loginBtn = GameObject.Find("loginBtn");
            nicknameText=GameObject.Find("nicknameText");
            logoutBtn=GameObject.Find("logoutBtn");
            InitializeUIElements();
            Find<Button>("startBtn").onClick.AddListener(onStartGameBtn);
            Find<Button>("mapBtn").onClick.AddListener(onMapCustomBtn);
            Find<Button>("loginBtn").onClick.AddListener(onLoginBtn);
            Find<Button>("logoutBtn").onClick.AddListener(onLogoutBtn);
            Find<Button>("setBtn").onClick.AddListener(onSetBtn);
            Find<Button>("quitBtn").onClick.AddListener(onQuitGameBtn);
        }

        public void InitializeUIElements()
        {
            // 根据 isLogin 值显示或隐藏元素
            if (GameApp.isLogin)
            {
                // 用户已登录，显示昵称和登出按钮
                loginBtn.SetActive(false);
                nicknameText.SetActive(true);
                logoutBtn.SetActive(true);
                Text nicknameUI = nicknameText.GetComponent<Text>();
                nicknameUI.text = $"欢迎    {GameApp.Nickname}";
            }
            else
            {
                // 用户未登录，显示登录按钮
                loginBtn.SetActive(true);
                nicknameText.SetActive(false);
                logoutBtn.SetActive(false);
            }
        }

        public void onAwakefuc()
        {
            InitializeUIElements();
        }
        /// <summary>
        /// 
        /// </summary>
        private void onStartGameBtn()
        {
            if (!GameApp.isLogin)
            {
                //message
                Debug.Log("unlogin");
                return;
            }
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
            ApplyFunc(Defines.OpenView, (int)ViewType.LoginView);
        }

        private void onLogoutBtn()
        {
            GameApp.uid=0;
            GameApp.Nickname="";
            GameApp.Username="";
            GameApp.isLogin=false;
            GameApp.ViewManager.GetView<StartView>(ViewType.StartView).onAwakefuc();

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
