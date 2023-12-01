
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
            //Find<Button>("mapBtn").onClick.AddListener(onMapCustomBtn);
            //Find<Button>("registerBtn").onClick.AddListener(onRegisterBtn);
            Find<Button>("setBtn").onClick.AddListener(onSetBtn);
            Find<Button>("quitBtn").onClick.AddListener(onQuitGameBtn);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onStartGameBtn()
        {

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
        private void onRegisterBtn()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        private void onSetBtn()
        {
            ApplyFunc(Defines.OpenSetView);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onQuitGameBtn()
        {

        }
    }
}
