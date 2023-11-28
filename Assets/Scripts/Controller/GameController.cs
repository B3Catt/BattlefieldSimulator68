
namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// 
        /// </summary>
        private Player[] players;

        /// <summary>
        /// 
        /// </summary>
        private Map map;

        /// <summary>
        /// 
        /// </summary>
        private RoundController roundController;

        public GameController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Game()
        {
            /**
             * before the game
             */

            /// TODO: get a game ready:
            ///       1. set the users and the camp etc.
            ///       2. set the map and the allocation of the units
            ///       3. etc.

            /**
             * main body of the game
             */
            while (!TerminateTargetCheck())
            {
                foreach (var player in players)
                {
                    roundController.Round(player);
                }
            }
            
            /**
             * after the game
             */

            /// TODO: get the game terminated
            ///       1. etc.

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool TerminateTargetCheck()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Unit[] GetAllUnitsByPlayer(Player player)
        {
            return null;
        }
    }
}
