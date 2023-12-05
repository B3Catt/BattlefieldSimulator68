using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginView : BaseView
    {

        public TMP_InputField inputFieldUsername;
        public TMP_InputField inputFieldPassword;
        protected override void OnAwake()
        {
            base.OnAwake();
            inputFieldUsername = GameObject.Find("bg/LoginUsername").GetComponent<TMP_InputField>();
            inputFieldPassword = GameObject.Find("bg/LoginPassword").GetComponent<TMP_InputField>();
            Find<Button>("bg/loginBtn").onClick.AddListener(onLoginBtn);
            Find<Button>("bg/registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void onLoginBtn()
        {
            if (inputFieldUsername != null)
                Debug.Log(inputFieldUsername.text);
            else
                Debug.Log("null");
        }

        /// <summary>
        /// 
        /// </summary>
        public void onRegisterBtn()
        {
            ApplyFunc(Defines.OpenView, (int)ViewType.RegisterView);

        }

        /// <summary>
        /// 
        /// </summary>
        public void onCloseBtn()
        {
            GameApp.ViewManager.Close(ViewId);
        }
    }
}
