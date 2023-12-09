using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;


namespace BattlefieldSimulator
{
    public class RegisterView : BaseView
    {
        public TMP_InputField inputFieldUsername;
        public TMP_InputField inputFieldPassword;
        public TMP_InputField inputFieldMail;
        public TMP_InputField inputFieldNickname;

        protected override void OnAwake()
        {
            base.OnAwake();
            inputFieldUsername = GameObject.Find("bg/RegisterUsername").GetComponent<TMP_InputField>();
            inputFieldPassword = GameObject.Find("bg/RegisterPassword").GetComponent<TMP_InputField>();
            inputFieldMail = GameObject.Find("bg/RegisterMail").GetComponent<TMP_InputField>();
            inputFieldNickname = GameObject.Find("bg/RegisterNickname").GetComponent<TMP_InputField>();
            Find<Button>("bg/registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void onRegisterBtn()
        {
            GameApp.ModelManager.ReadData<User>();
            Dictionary<int, User> userDictionary = GameApp.ModelManager.GetData<User>();
            string newUsername = inputFieldUsername.text; // 获取新注册的用户名
            bool isUsernameTaken = false;
            // 遍历userDictionary，检查用户名是否已经存在
            foreach (var kvp in userDictionary)
            {
                if (kvp.Value._username == newUsername)
                {
                    // 如果用户名已存在，设置标志并中断循环
                    isUsernameTaken = true;
                    break;
                }
            }

            if (isUsernameTaken)
            {
                // 用户名已被使用，弹入MessgaeView
                Debug.Log("用户名重复");
            }
            else
            {
                // 用户名可用，可以执行注册流程
                User newuser = new User
                {
                    _mail = inputFieldMail.text,
                    _nickname = inputFieldNickname.text,
                    _password = inputFieldPassword.text,
                    _username = inputFieldUsername.text,
                };
                GameApp.ModelManager.AddData<User>(newuser);
                GameApp.ModelManager.Flush<User>();
                //弹出注册成功
                Debug.Log("注册成功");
            }

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
