using UnityEngine;

namespace Contest
{
    public class EnemyBase : UnitBase, IEnemy
    {
        protected BehaviorPattern pattern;

        public BehaviorPattern BehaviorPattern => pattern;
        SkillHandler skillTracker;
        protected override void Awake()
        {
            base.Awake();
            if (Helpers.TryChangeType(unitData, out Enemy enemy))
            {
                pattern = enemy.EnemyPattern;
            }
            else
            {
                Debug.LogError($"{unitData.Name}の型が不正です。:{gameObject.name}");
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
