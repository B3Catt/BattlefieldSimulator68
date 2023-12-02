﻿
using UnityEngine;
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
            LoadingModel loadingModel = new LoadingModel();
            loadingModel.SceneName = "map";

            GameApp.ViewManager.Close(ViewId);

            Controller.ApplyControllerFunc(ControllerType.LoadingController, Defines.LoadingScene, loadingModel);
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
            ApplyFunc(Defines.OpenView, (int)ViewType.SetView);
        }

        /// <summary>
        /// 
        /// </summary>
        private void onQuitGameBtn()
        {
            Controller.ApplyControllerFunc(ControllerType.UIController, Defines.OpenView, (int)ViewType.MessageView,
                new MessageInfo()
                {
                    okCallback = delegate ()
                    {
                        Application.Quit();
                    },
                    MsgTxt = "确定要退出游戏吗？"
                }
            );
        }
    }
}
