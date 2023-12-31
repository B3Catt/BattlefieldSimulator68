﻿using System;
using System.Linq;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class UIController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public UIController() : base()
        {
            InstanceManager.UIViewManager.Register(ViewType.StartView, new ViewInfo()
            {
                PrefabName = "StartView",
                controller = this,
                Sorting_Order = 0,
                parentTf = InstanceManager.UIViewManager.canvasTf
            });

            InstanceManager.UIViewManager.Register(ViewType.SetView, new ViewInfo()
            {
                PrefabName = "SetView",
                controller = this,
                Sorting_Order = 1,
                parentTf = InstanceManager.UIViewManager.canvasTf
            });

            InstanceManager.UIViewManager.Register(ViewType.LoginView, new ViewInfo()
            {
                PrefabName = "LoginView",
                controller = this,
                Sorting_Order = 2,
                parentTf = InstanceManager.UIViewManager.canvasTf
            });

            InstanceManager.UIViewManager.Register(ViewType.RegisterView, new ViewInfo()
            {
                PrefabName = "RegisterView",
                controller = this,
                Sorting_Order = 3,
                parentTf = InstanceManager.UIViewManager.canvasTf
            });

            InstanceManager.UIViewManager.Register(ViewType.AssertionView, new ViewInfo()
            {
                PrefabName = "AssertionView",
                controller = this,
                Sorting_Order = 4,
                parentTf = InstanceManager.UIViewManager.canvasTf
            });

            InstanceManager.UIViewManager.Register(ViewType.MessageView, new ViewInfo()
            {
                PrefabName = "MessageView",
                controller = this,
                Sorting_Order = 999,
                parentTf = InstanceManager.UIViewManager.canvasTf
            });

            InitModuleEvent();
            InitGlobalEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void InitModuleEvent()
        {
            base.InitModuleEvent();
            RegisterFunc(Defines.OpenUIView, openUIView);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void openUIView(System.Object[] args)
        {
            InstanceManager.UIViewManager.Open(int.Parse(args[0].ToString()), args.Skip(1).ToArray());
        }
    }
}
