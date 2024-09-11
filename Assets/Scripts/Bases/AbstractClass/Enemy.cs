using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Contest
{
    public abstract class Enemy : UnitBase, IEnemy
    {
        BehaviorPattern pattern;
        public BehaviorPattern BehaviorPattern
        {
            get
            {
                return pattern;
            }
        }

        public abstract void EnemyBehavior();
    }
}
