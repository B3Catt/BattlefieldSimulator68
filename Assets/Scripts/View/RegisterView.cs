using UnityEngine.UI;

namespace BattlefieldSimulator
{
    public class RegisterView : BaseView
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            Find<Button>("registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("closeBtn").onClick.AddListener(onCloseBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void onRegisterBtn()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void onCloseBtn()
        {

        }
    }
}
