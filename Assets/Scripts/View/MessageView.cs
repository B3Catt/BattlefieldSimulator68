
using UnityEngine.UI;

namespace BattlefieldSimulator
{

    /// <summary>
    /// 
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string MsgTxt;

        /// <summary>
        /// 
        /// </summary>
        public System.Action okCallback;

        /// <summary>
        /// 
        /// </summary>
        public System.Action noCallback;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MessageView : BaseView
    {
        MessageInfo info;

        protected override void OnAwake()
        {
            base.OnAwake();
            Find<Button>("okBtn").onClick.AddListener(onOkBtn);
            Find<Button>("noBtn").onClick.AddListener(onNoBtn);
        }

        public override void Open(params object[] args)
        {
            info = args[0] as MessageInfo;
            Find<Text>("content/txt").text = info.MsgTxt;
        }

        private void onOkBtn()
        {
            info.okCallback?.Invoke();
        }

        private void onNoBtn()
        {
            info.noCallback?.Invoke();
            GameApp.ViewManager.Close(ViewId);
        }
    }
}
