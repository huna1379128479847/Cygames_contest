using JetBrains.Annotations;
using System;
using System.Collections;
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
        protected override void Awake()
        {
            base.Awake();
            if (unitData.GetType() == typeof(Enemy))
            {
                pattern = (unitData as Enemy).EnemyPattern;
            }
        }
        //適宜オーバーライドして書き換えてね
        public override void TurnBehavior()
        {
            if (pattern == BehaviorPattern.Aggressive)
            {
                Execute(0.5f, 0.3f, 0.2f);
            }
            if (pattern == BehaviorPattern.Defensive)
            {
                Execute(0.2f, 0.45f, 0.35f);
            }
            if (pattern == BehaviorPattern.Support)
            {
                Execute(0.25f, 0.3f, 0.45f);
            }
        }

        public virtual void Execute(float aggressive, float defensive, float support)
        {
            float r = UnityEngine.Random.value;
            if (r <= aggressive)
            {

            }
            else if (r <= defensive)
            {

            }
            else if (r <= support)
            {

            }
        }
    }
}
