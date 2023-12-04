using System;
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
            GameApp.ViewManager.Register(ViewType.StartView, new ViewInfo()
            {
                PrefabName = "StartView",
                controller = this,
                Sorting_Order = 0,
                parentTf = GameApp.ViewManager.canvasTf
            });

            GameApp.ViewManager.Register(ViewType.SetView, new ViewInfo()
            {
                PrefabName = "SetView",
                controller = this,
                Sorting_Order = 1,
                parentTf = GameApp.ViewManager.canvasTf
            });

            GameApp.ViewManager.Register(ViewType.LoginView, new ViewInfo()
            {
                PrefabName = "LoginView",
                controller = this,
                Sorting_Order = 2,
                parentTf = GameApp.ViewManager.canvasTf
            });

            GameApp.ViewManager.Register(ViewType.MessageView, new ViewInfo()
            {
                PrefabName = "MessageView",
                controller = this,
                Sorting_Order = 999,
                parentTf = GameApp.ViewManager.canvasTf
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
        }
    }
}
