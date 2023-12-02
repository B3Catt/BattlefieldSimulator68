
namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseModel
    {
        public BaseController Controller { get; set; }

        public BaseModel(BaseController baseController) 
        {
            this.Controller = baseController;
        }

        public BaseModel()
        {

        }

        public virtual void Init()
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual void Update() 
        {

        }
    }
}
