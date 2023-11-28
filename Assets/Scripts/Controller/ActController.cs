
namespace BattlefieldSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public class ActController
    {
        GameController gameController;

        public bool Act(Unit unit)
        {
            /**
             * before the act
             */

            /**
             * main body the act
             */
            while(IsMovable())
            {
                ///TODO: get target positon
                
                ///TODO: check the act mode
                
                ///TODO: act

                ///TODO: settle accounts; calculate the results of the act
            }

            /**
             * after the act
             */

            return true;
        }

        private bool IsMovable()
        {
            return true;
        }
    }
}
