using System;

namespace Contest
{
    /// <summary>
    /// ダメージの情報を管理するクラス。ダメージ計算もここで行う。
    /// </summary>
    public class DamageInfo
    {
        private float baseDamage;
        private float finalDamage;
        private bool? isCrit = null;
        public Action action;
        public Skill skill;
        public UnitBase damageWorker;
        public UnitBase damageTaker; 
        public bool isBad; //ダメージか否か trueの場合ダメージ
        public bool isAttack;

        public float Damage => finalDamage;  // 外部からダメージ値を取得するためのプロパティ
        public bool IsCrit
        {// クリティカルヒットかどうかの判定
            get
            {
                if (isCrit == null)
                {
                    isCrit = UnityEngine.Random.value <= 0.2f;
                }
                return isCrit.Value;
            }
        }
        public DamageInfo(Skill skill, UnitBase damageWorker, UnitBase damageTaker)
        {
            this.skill = skill;
            this.damageWorker = damageWorker;
            this.damageTaker = damageTaker;
        }
    }
}
