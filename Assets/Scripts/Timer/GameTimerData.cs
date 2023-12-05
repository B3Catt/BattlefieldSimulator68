namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class GameTimerData
    {
        /// <summary>
        /// 
        /// </summary>
        private float timer;

        /// <summary>
        /// 
        /// </summary>
        private System.Action callback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="callback"></param>
        public GameTimerData(float timer, System.Action callback)
        {
            this.timer = timer;
            this.callback = callback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool OnUpdate(float dt)
        {
            timer -= dt;
            if (timer < 0)
            {
                callback.Invoke();
                return true;
            }
            return false;
        }
    }
}
