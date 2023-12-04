
using System;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseController Controller { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseController"></param>
        public BaseModel(BaseController baseController) 
        {
            this.Controller = baseController;
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseModel()
        {

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
        public virtual void Destroy()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update() 
        {

        }
    }
}
