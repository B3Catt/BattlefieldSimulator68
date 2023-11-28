
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
        private string gameObjectName;

        /// <summary>
        /// the cash dictionary of the music
        /// </summary>
        private Dictionary<string, AudioClip> clips;

        /// <summary>
        /// 
        /// </summary>
        public SoundManager(string gameObjectName)
        {
            this.gameObjectName = gameObjectName;
            clips = new Dictionary<string, AudioClip>();
            bgmSource = GameObject.Find(gameObjectName).GetComponent<AudioSource>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        public void PlayBGM(string res)
        {
            // if there's no music which's name is the 'res'
            if (clips.ContainsKey(res) == false)
            {
                // then add the music into the cash
                AudioClip clip = Resources.Load<AudioClip>("Sounds/" + res);
                clips.Add(res, clip);
            }
            bgmSource.clip = clips[res];
            bgmSource?.Play();  // play the music
        }
    }
}
