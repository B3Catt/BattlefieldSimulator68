
using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    /// <summary>
    /// manage the sound
    /// </summary>
    public class SoundManager
    {
        /// <summary>
        /// 
        /// </summary>
        private AudioSource bgmSource;

        /// <summary>
        /// 
        /// </summary>
        public string gameObjectName { get; set; }

        /// <summary>
        /// the cache dictionary of the music
        /// </summary>
        private Dictionary<string, AudioClip> clips;

        /// <summary>
        /// 
        /// </summary>
        private bool isStop;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStop
        {
            get { return isStop; }
            set
            {
                isStop = value;
                if(isStop == true)
                {
                    bgmSource.Pause();
                }
                else
                {
                    bgmSource.Play();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SoundManager(string gameObjectName)
        {
            this.gameObjectName = gameObjectName;
            clips = new Dictionary<string, AudioClip>();
            bgmSource = GameObject.Find(gameObjectName).GetComponent<AudioSource>();

            IsStop = false;
            BgmVolume = 1;
            EffectVolume = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        private float bgmVolume;

        /// <summary>
        /// 
        /// </summary>
        public float BgmVolume
        { 
            get { return bgmVolume; }
            set
            {
                bgmVolume = value;
                bgmSource.volume = bgmVolume;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private float effectVolume;

        /// <summary>
        /// 
        /// </summary>
        public float EffectVolume
        {
            get { return effectVolume; }
            set
            {
                effectVolume = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        public void PlayBGM(string res)
        {
            if (isStop == true)
            {
                return;
            }

            // if there's no music which's name is the 'res'
            if (clips.ContainsKey(res) == false)
            {
                // then add the music into the cache
                AudioClip clip = Resources.Load<AudioClip>("Sounds/" + res);
                clips.Add(res, clip);
            }
            bgmSource.clip = clips[res];
            bgmSource?.Play();  // play the music
        }
    }
}
