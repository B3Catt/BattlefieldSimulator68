﻿
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
        public static ModelManager ModelManager;

        /// <summary>
        /// 
        /// </summary>
        public static CameraManager CameraManager;

        /// <summary>
        /// 
        /// </summary>
        public static MessageCenter MessageCenter;

        /// <summary>
        /// 
        /// </summary>
        public static bool isLogin;
        public static string Username;
        public static string Nickname;
        public static int uid;

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            SoundManager = new SoundManager("game");
            ControllerManager = new ControllerManager();
            ViewManager = new ViewManager("Canvas", "WorldCanvas");
            ModelManager = new ModelManager();
            CameraManager = new CameraManager();
            MessageCenter = new MessageCenter();
            isLogin = false;
            Username = "";
            Nickname = "";
            uid=0;
        }
    }
}
