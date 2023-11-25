
using UnityEngine;

namespace BattlefieldSimulator
{
    class MainController : MonoBehaviour
    {
        public enum Option
        {
            game,
            quit,
        };

        private GameController gameController;

        private void Start()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int main()
        {
            /// Log in

            /// main body
            while (true)
            {
                /// chose the option
                var op = GetOption();

                switch(op)
                {
                    case Option.game:
                        gameController.Game();
                        break;
                }
                break;
            }

            /// main
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Option GetOption()
        {
            return Option.quit;
        }
    }
}
