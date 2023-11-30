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
            RegisterFunc(Defines.OpenStartView, openStartView);
            base.InitModuleEvent();
        }

        private void openStartView(System.Object[] args)
        {
            GameApp.ViewManager.Open(ViewType.StartView, args);
        }
    }
}
