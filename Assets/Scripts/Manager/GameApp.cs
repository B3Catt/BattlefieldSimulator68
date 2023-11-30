
namespace BattlefieldSimulator
{
    /// <summary>
    /// this class is the main management of the game, by which we do initialization
    /// </summary>
    public class GameApp : Singleton<GameApp>
    {
        /// <summary>
        /// 
        /// </summary>
        public static SoundManager SoundManager;

        /// <summary>
        /// 
        /// </summary>
        public static ControllerManager ControllerManager;

        /// <summary>
        /// 
        /// </summary>
        public static ViewManager ViewManager;

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            SoundManager = new SoundManager("game");
            ControllerManager = new ControllerManager();
            ViewManager = new ViewManager("Canvas", "WorldCanvas");
        }
    }
}
