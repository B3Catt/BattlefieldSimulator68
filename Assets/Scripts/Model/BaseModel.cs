
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

        public virtual void Init()
        {

        }
    }
}
