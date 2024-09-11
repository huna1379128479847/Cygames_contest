using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class BattleScene : SingletonBehavior<BattleScene>
    {
        public Tarn tarn;
        public Tarn Tarn 
        {
            get
            {
                return tarn;
            }
        }
        public void EnterBattle()
        {

        }
    }
}
