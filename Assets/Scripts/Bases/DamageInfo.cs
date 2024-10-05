using System;

namespace Contest
{
    /// <summary>
    /// ダメージ情報を管理し、ダメージ計算を行うクラス。
    /// どのスキルが使用され、どのユニットがダメージを与え、どのユニットがダメージを受けるかなどを管理する。
    /// </summary>
    public class DamageInfo
    {
        // 基本ダメージ量
        private float baseDamage;
        // 最終的なダメージ量
        private float finalDamage;
        // クリティカルヒットかどうか (nullの場合は未計算)
        private bool? isCrit = null;
        // ダメージに関連するアクション (オプション)
        public Action action;
        // 使用されたスキル
        public Skill skill;
        // ダメージを与えるユニット
        public UnitBase damageWorker;
        // ダメージを受けるユニット
        public UnitBase damageTaker;
        // ダメージを与えるか回復かを判定 (trueならダメージ)
        public bool isBad;
        // 攻撃かどうかを示すフラグ
        public bool isAttack;

        /// <summary>
        /// 外部から最終的なダメージ量を取得するためのプロパティ。
        /// </summary>
        public float Damage => finalDamage;

        /// <summary>
        /// クリティカルヒットかどうかを判定するプロパティ。
        /// 一度判定されたらその結果が返される。
        /// </summary>
        public bool IsCrit
        {
            get
            {
                if (isCrit == null)
                {
                    // 20%の確率でクリティカルヒットとする (0.2f)
                    isCrit = UnityEngine.Random.value <= 0.2f;
                }
                return isCrit.Value;
            }
        }

        /// <summary>
        /// コンストラクタ。スキル、ダメージを与えるユニット、受けるユニットを指定して初期化する。
        /// </summary>
        /// <param name="skill">使用されたスキル。</param>
        /// <param name="damageWorker">ダメージを与えるユニット。</param>
        /// <param name="damageTaker">ダメージを受けるユニット。</param>
        public DamageInfo(Skill skill, UnitBase damageWorker, UnitBase damageTaker)
        {
            this.skill = skill;
            this.damageWorker = damageWorker;
            this.damageTaker = damageTaker;
        }
    }
}
