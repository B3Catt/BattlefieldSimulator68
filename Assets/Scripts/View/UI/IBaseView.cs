
namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsInit();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsShow();

        /// <summary>
        /// 
        /// </summary>
        void InitUI();

        void InitData();
        
        /// <summary>
        /// open the view
        /// </summary>
        /// <param name="args"></param>
        void Open(params object[] args);

        /// <summary>
        /// close the view
        /// </summary>
        /// <param name="args"></param>
        void Close(params object[] args);

        /// <summary>
        /// 
        /// </summary>
        void DestroyView();

        /// <summary>
        /// trigger the event in self model
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void ApplyFunc(string eventName, params object[] args);

        /// <summary>
        /// trigger the event from other controller
        /// </summary>
        /// <param name="controllerKey"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void ApplyControllerFunc(int controllerKey, string eventName, params object[] args);

        /// <summary>
        /// set the visible of the view
        /// </summary>
        /// <param name="value"></param>
        void SetVisible(bool value);

        /// <summary>
        /// 
        /// </summary>
        int ViewId { get; set; }

        /// <summary>
        /// the controller to which view is belong
        /// </summary>
        BaseController Controller { get; set; }
    }
}