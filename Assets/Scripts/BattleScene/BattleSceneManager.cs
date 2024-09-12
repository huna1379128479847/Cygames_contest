using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class BattleSceneManager : SingletonBehavior<BattleSceneManager>, IManager
    {
        public Turn turn;
        private bool isRunning;
        List<EnemyBase> enemies;
        List<EnemyBase> enemiess;
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                }
            }
        }
        public Turn Turn
        {
            get
            {
                return turn;
            }
        }
        public void Execute()
        {

        }
    }
}
