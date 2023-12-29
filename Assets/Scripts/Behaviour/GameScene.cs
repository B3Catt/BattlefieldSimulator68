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
                InstanceManager.Instance.Init();
                DontDestroyOnLoad(gameObject);
                isLoaded = true;
            }
        }

        private void Start()
        {
            InstanceManager.SoundManager.PlayBGM(bgmName);

            testMysql();

            RegisterMoudle();

            InitMoudle();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RegisterMoudle()
        {
            InstanceManager.ControllerManager.Register(ControllerType.UI, new UIController());
            InstanceManager.ControllerManager.Register(ControllerType.Game, new GameController());
            InstanceManager.ControllerManager.Register(ControllerType.Loading, new LoadingController());
            InstanceManager.ControllerManager.Register(ControllerType.User, new UserController());
        }

        private void InitMoudle()
        {
            InstanceManager.ControllerManager.InitAllMoudles();
        }

        private void Update()
        {
            dt = Time.deltaTime;
            InstanceManager.Instance.Update(dt);
        }

        private void testMysql()
        {
            //GameApp.ModelManager.AddData<ArmType>(new ArmType()
            //{
            //    _id = 2,
            //    _auther = "1",
            //    _information = "1",
            //    _updateby = "1",
            //    _value = 1,
            //    _isable = false,
            //    _attack_distance = 1,
            //    _createtime = DateTime.Now.TimeOfDay,
            //    _updatetime = DateTime.Now.TimeOfDay,
            //    _name = "1"
            //});
            //GameApp.ModelManager.ReadData<ArmType>();
            //GameApp.ModelManager.DeleteData<ArmType>(1);
            //GameApp.ModelManager.ReadData<ArmType>();
        }
    }
}
