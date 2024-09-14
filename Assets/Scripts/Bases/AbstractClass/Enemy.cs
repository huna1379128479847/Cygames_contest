using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Contest
{
    public abstract class EnemyBase : UnitBase, IEnemy
    {
        BehaviorPattern pattern;
        SkillTracker skillTracker;
        public BehaviorPattern BehaviorPattern
        {
            get
            {
                return pattern;
            }
        }
    }
}
