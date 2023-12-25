using System.Collections.Generic;
using System.Timers;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class GameTimer
    {
        /// <summary>
        /// 
        /// </summary>
        private List<GameTimerData> timers;

        /// <summary>
        /// 
        /// </summary>
        public GameTimer()
        {
            timers = new List<GameTimerData>();
        }

        public void Register(float timer, System.Action callback)
        {
            GameTimerData data = new GameTimerData(timer, callback);
            timers.Add(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public void OnUpdate(float dt)
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                if (timers[i].OnUpdate(dt) == true)
                {
                    timers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Break()
        {
            timers.Clear();
        }

        public int Count() { return timers.Count; }
    }
}
