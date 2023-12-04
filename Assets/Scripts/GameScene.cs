
using System;
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
            GameApp.ControllerManager.Register(ControllerType.UserController, new UserController());
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

        private void testMysql()
        {
            GameApp.ModelManager.AddData<ArmType>(new ArmType()
            {
                _id = 2,
                _auther = "1",
                _information = "1",
                _updateby = "1",
                _value = 1,
                _isable = false,
                _attack_distance = 1,
                _createtime = DateTime.Now.TimeOfDay,
                _updatetime = DateTime.Now.TimeOfDay,
                _name = "1"
            });
            GameApp.ModelManager.ReadData<ArmType>();
            GameApp.ModelManager.DeleteData<ArmType>(1);
            GameApp.ModelManager.ReadData<ArmType>();
        }
    }
}
