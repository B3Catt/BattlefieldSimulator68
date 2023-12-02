
using UnityEditor.Build;
using UnityEngine;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class GameScene : MonoBehaviour
    {
        /// <summary>
        /// Time.deltaTime
        /// </summary>
        float dt;

        /// <summary>
        /// 
        /// </summary>
        public string bgmName = "login";

        /// <summary>
        /// 
        /// </summary>
        private static bool isLoaded = false;

        private void Awake()
        {
            if (isLoaded == true)
            {
                Destroy(gameObject);
            }
            else
            {
                isLoaded = true;
                DontDestroyOnLoad(gameObject);
                GameApp.Instance.Init();
            }
        }

        private void Start()
        {
            GameApp.SoundManager.PlayBGM("login");

            RegisterMoudle();

            InitMoudle();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RegisterMoudle()
        {
            GameApp.ControllerManager.Register(ControllerType.UIController, new UIController());
            GameApp.ControllerManager.Register(ControllerType.GameController, new GameController());
            GameApp.ControllerManager.Register(ControllerType.LoadingController, new LoadingController());
        }

        private void InitMoudle()
        {
            GameApp.ControllerManager.InitAllMoudles();
        }

        private void Update()
        {
            dt = Time.deltaTime;
            GameApp.Instance.Update(dt);
        }
    }
}
