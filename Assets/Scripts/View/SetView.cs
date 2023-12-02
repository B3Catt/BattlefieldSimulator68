using UnityEngine.UI;

namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class SetView : BaseView
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void OnAwake()
        {
            base.OnAwake();
            Find<Button>("bg/closeBtn").onClick.AddListener(onCloseBtn);
            Find<Toggle>("bg/IsOpnSound").onValueChanged.AddListener(onIsStopBtn);
            Find<Slider>("bg/soundCount").onValueChanged.AddListener(onSliderBgmBtn);
            Find<Slider>("bg/effectCount").onValueChanged.AddListener(onSliderSoundEffectBtn);

            Find<Toggle>("bg/IsOpnSound").isOn = GameApp.SoundManager.IsStop;
            Find<Slider>("bg/soundCount").value = GameApp.SoundManager.BgmVolume;
            Find<Slider>("bg/effectCount").value = GameApp.SoundManager.EffectVolume;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isStop"></param>
        private void onIsStopBtn(bool isStop)
        {
            GameApp.SoundManager.IsStop = isStop;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void onSliderBgmBtn(float val)
        {
            GameApp.SoundManager.BgmVolume = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void onSliderSoundEffectBtn(float val)
        {
            GameApp.SoundManager.EffectVolume = val;
        }

        /// <summary>
        /// 
        /// </summary>
        private void onCloseBtn()
        {
            GameApp.ViewManager.Close(ViewId);
        }
    }
}
