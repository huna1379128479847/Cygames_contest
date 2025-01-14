﻿using System;

namespace Contest
{
    /// <summary>
    /// ダメージ情報を管理し、ダメージ計算を行うクラス。
    /// どのスキルが使用され、どのユニットがダメージを与え、どのユニットがダメージを受けるかなどを管理する。
    /// </summary>
    public class DamageInfo
    {
        // 
        public IUniqueThing source;
        // ダメージを与えるユニット
        public UnitBase damageWorker;
        // ダメージを受けるユニット
        public UnitBase damageTaker;

        public DamageOptions damageOptions;

        public int amount;

        public bool isCrit = false;

        public bool isHit = false;
        // 攻撃者がいるダメージかどうか(持続ダメージなどの場合False)
        public bool isAttack = false;

        public bool isOccurredDamage = true;

        /// <summary>
        /// コンストラクタ。ダメージを与えるユニット、受けるユニットを指定して初期化する。
        /// </summary>
        /// <param name="damageWorker">ダメージを与えるユニット。</param>
        /// <param name="damageTaker">ダメージを受けるユニット。</param>
        public DamageInfo(IUniqueThing source,UnitBase damageWorker, UnitBase damageTaker, DamageOptions damageOptions = DamageOptions.None)
        {
            this.source = source;
            this.damageWorker = damageWorker;
            if (damageTaker == null)
            {
                damageTaker = damageWorker;
            }
            else
            {
                this.damageTaker = damageTaker;
            }

        }
    }
}
