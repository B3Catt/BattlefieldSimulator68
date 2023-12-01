using UnityEngine.UI;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class SetView : BaseView
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
        }

        private void onCloseBtn()
        {
            GameApp.ViewManager.Close(ViewId);
        }
    }
}
