using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlefieldSimulator
{
    /// <summary>
    /// this class is the main management of the game, by which we do initialization
    /// </summary>
    public class GameApp : Singleton<GameApp>
    {
        /// <summary>
        /// 
        /// </summary>
        public static SoundManager SoundManager;

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            SoundManager = new SoundManager("game");
        }
    }
}
