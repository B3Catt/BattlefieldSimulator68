
namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    class RoundController
    {
        /// <summary>
        /// 
        /// </summary>
        private User user;

        /// <summary>
        /// 
        /// </summary>
        private GameController gameController;

        /// <summary>
        /// 
        /// </summary>
        private ActController actController;

        /// <summary>
        /// 
        /// </summary>
        public RoundController() { }

        public RoundController(GameController gameController)
        {
            this.gameController = gameController;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool Round(Player player)
        {
            /**
             * before the round
             */
            Unit[] units = gameController.GetAllUnitsByPlayer(player);

            /**
             * main body of the round
             */
            while (RoundEndCheck())
            {
                /// TODO: waiting the selection
                Unit unit = GetUnitByClick();

                actController.Act(unit);
            }

            /**
             * before the round
             */

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool RoundEndCheck()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Unit GetUnitByClick()
        {
            return null;
        }
    }
}
