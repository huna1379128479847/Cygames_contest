using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    /// <summary>
    /// サンプルコード
    /// </summary>
    public class Burning : StatusEffect
    {
        // フィールド
        private StatusBase hp;
        private UnitBase parent;
        private int DefDecreaseStack;

        // プロパティ

        // コンストラクタ
        public Burning(StatusEffectData data, IHandler handler)
            : base(data, handler)
        {
            DefDecreaseStack = 0;
        }

        // メソッド
        public override void Apply()
        {
            parent = ParentHandler.ParentUnit;
            hp = parent.StatusTracker.CurrentHP;
        }
        public override void DecreaseDuration(int time = 1)
        {
            base.DecreaseDuration(time);
            if (DefDecreaseStack < Data.Amount)
            {
                StatusEffect decreaseDef = new DecreaseDefence(Data.Childdatas[0], ParentHandler as IHandler);
                parent.EffectHandler.AddEffect(decreaseDef);
            }
            DefDecreaseStack++;
        }
        public override void ExecuteEffect()
        {
            DamageInfo info = new DamageInfo(this, null, parent, DamageOptions.IsDamage | DamageOptions.IsFix | DamageOptions.IsDot)
            {
                amount = Math.Clamp((int)(Data.Magnification * hp.CurrentAmount), 1, 100)
            };
            BattleSceneManager.instance.ApplyDamageInfo(info);
        }
    }
}
