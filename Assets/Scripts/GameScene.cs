
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

        private void Awake()
        {
            GameApp.Instance.Init();
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
