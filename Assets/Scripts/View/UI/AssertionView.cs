using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace BattlefieldSimulator
{
    public class AssertionInfo
    {
        public string AssertionTextType;
        public string AssertionTextContent;
    }
    public class AssertionView : BaseView
    {
        AssertionInfo info;
        protected override void OnAwake()
        {
            base.OnAwake();
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
        }

        public override void Open(params object[] args)
        {
            info = args[0] as AssertionInfo;
            Find<Text>("bg/TextType").text = info.AssertionTextType;
            Find<Text>("bg/TextContent").text = info.AssertionTextContent;
        }

        public void onCloseBtn()
        {
            GameApp.UIViewManager.Close(ViewId);
        }
    }
}
