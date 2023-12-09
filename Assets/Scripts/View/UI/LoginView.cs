using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginView : BaseView
    {

        public TMP_InputField Username;
        public TMP_InputField Password;
        protected override void OnAwake()
        {
            base.OnAwake();
            Username = GameObject.Find("bg/LoginUsername").GetComponent<TMP_InputField>();
            Password = GameObject.Find("bg/LoginPassword").GetComponent<TMP_InputField>();
            Find<Button>("bg/loginBtn").onClick.AddListener(onLoginBtn);
            Find<Button>("bg/registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void onLoginBtn()
        {
            if(GameApp.isLogin)
            {
                //message
                Debug.Log($"无法再次登录，以成功登录用户id={GameApp.uid},username={GameApp.Username}");
                return;
            }
            string newUsername = Username.text;
            string newPassword = Password.text;
            GameApp.ModelManager.ReadData<User>();
            Dictionary<int, User> userDictionary = GameApp.ModelManager.GetData<User>();

            foreach (var kvp in userDictionary)
            {
                if (kvp.Value._username == newUsername && kvp.Value._password == newPassword)
                {
                    // 如果用户名已存在，设置标志并中断循环
                    GameApp.isLogin = true;
                    GameApp.Username = newUsername;
                    GameApp.Nickname = kvp.Value._nickname;
                    GameApp.uid = kvp.Value._id;
                    break;
                }
            }
            if (GameApp.isLogin)
            {
                //messgqe
                Debug.Log($"登录成功，欢迎{GameApp.Nickname}");
                GameApp.UIViewManager.GetView<StartView>(ViewType.StartView).onAwakefuc();
            }
            else
            {
                //messgae
                Debug.Log("用户名或密码错误");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void onRegisterBtn()
        {
            ApplyFunc(Defines.OpenUIView, (int)ViewType.RegisterView);
        }

        /// <summary>
        /// 
        /// </summary>
        public void onCloseBtn()
        {
            GameApp.UIViewManager.Close(ViewId);
        }
    }
}
