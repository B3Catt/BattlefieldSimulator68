using System;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly T instance = Activator.CreateInstance<T>();
        
        /// <summary>
        /// 
        /// </summary>
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(float dt)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnDestroy()
        {

        }
    }
}
