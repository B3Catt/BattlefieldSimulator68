using UnityEngine.UI;

namespace BattlefieldSimulator
{
    public class RegisterView : BaseView
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            Find<Button>("bg/registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
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
            GameApp.ViewManager.Close(ViewId);
        }
    }
}
