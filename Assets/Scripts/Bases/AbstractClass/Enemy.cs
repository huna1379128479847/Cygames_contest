using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Contest
{
    public class EnemyBase : UnitBase, IEnemy
    {
        BehaviorPattern pattern;
        SkillHandler skillTracker;
        public BehaviorPattern BehaviorPattern
        {
            get
            {
                return pattern;
            }
        }

        public override void TurnBehavior()
        {
            if (pattern == BehaviorPattern.Aggressive)
            {
                Execute(0.5f, 0.3f, 0.2f);
                return;
            }
            if (pattern == BehaviorPattern.Defensive)
            {
                Execute(0.2f, 0.45f, 0.35f);
                return;
            }
            if (pattern == BehaviorPattern.Support)
            {
                Execute(0.25f, 0.3f, 0.45f);
                return;
            }
        }

        public virtual void Execute(float aggressive, float defensive, float support)
        {

        }
    }
}
