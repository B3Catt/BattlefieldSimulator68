
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
            if (InstanceManager.isLogin)
            {
                // 用户已登录，显示昵称和登出按钮
                loginBtn.SetActive(false);
                nicknameText.SetActive(true);
                logoutBtn.SetActive(true);
                Text nicknameUI = nicknameText.GetComponent<Text>();
                nicknameUI.text = $"欢迎    {InstanceManager.Nickname}";
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
            if (!InstanceManager.isLogin)
            {
                //message
                Debug.Log("unlogin");
                return;
            }
            LoadingModel loadingModel = new LoadingModel();
            loadingModel.SceneName = "Map";
            loadingModel.callback = delegate ()
            {

            };

            InstanceManager.UIViewManager.Close(ViewId);

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
            ApplyFunc(Defines.OpenUIView, (int)ViewType.LoginView);
        }

        private void onLogoutBtn()
        {
            InstanceManager.uid=0;
            InstanceManager.Nickname="";
            InstanceManager.Username="";
            InstanceManager.isLogin=false;
            InstanceManager.UIViewManager.GetView<StartView>(ViewType.StartView).onAwakefuc();

        }

        /// <summary>
        /// 
        /// </summary>
        private void onSetBtn()
        {
            ApplyFunc(Defines.OpenUIView, (int)ViewType.SetView);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onQuitGameBtn()
        {
            Controller.ApplyControllerFunc(ControllerType.UI, Defines.OpenUIView, (int)ViewType.MessageView,
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
