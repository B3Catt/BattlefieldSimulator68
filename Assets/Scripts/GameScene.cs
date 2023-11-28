
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
        }

        private void Update()
        {
            dt = Time.deltaTime;
            GameApp.Instance.Update(dt);
        }
    }
}
