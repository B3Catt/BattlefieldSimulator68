
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

        void ApplyFunc(string eventName, params object[] args);
    }
}