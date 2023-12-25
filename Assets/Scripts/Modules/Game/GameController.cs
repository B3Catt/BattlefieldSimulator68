namespace BattlefieldSimulator
{
    public class GameController : BaseController
    {
        public override void Init()
        {
            ApplyControllerFunc(ControllerType.UI, Defines.OpenUIView, (int)ViewType.StartView);
        }
    }
}
