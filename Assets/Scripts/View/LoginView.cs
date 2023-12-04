using UnityEngine.UI;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginView : BaseView
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();

            Find<Button>("loginBtn").onClick.AddListener(onLoginBtn);
            Find<Button>("registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("closeBtn").onClick.AddListener(onCloseBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void onLoginBtn()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void onRegisterBtn()
        {
            /// TODO: open the register view
        }

        /// <summary>
        /// 
        /// </summary>
        public void onCloseBtn()
        {

        }
    }
}
