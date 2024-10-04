using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Contest
{
    public abstract class StatusEffect : IUniqueThing, IEffect
    {
        public StatusEffectData _data;
        protected EffectHandler parent;
        private string name;
        private string description;
        private int duration;
        public string Name
        {
            get
            {
                return name;
            }

        }
        public int Duration // 状態異常の持続時間（ターン数）
        {
            get
            {
                return duration;
            }
        }
        public Guid ID { get; protected set; }


        // 状態異常が持続しているかどうかを確認するプロパティ    
        public virtual bool IsExpired
        {
            get 
            { 
                return Duration <= 0; 
            }
        }

        public EffectHandler Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent == null)
                {
                    parent = value;
                }
            }
        }

        public EffectTiming Timing
        {
            get
            {
                return _data.Timing;
            }
        }
        public EffectFlgs Flgs
        {
            get
            {
                return _data.Effect;
            }
        }

        // 状態異常が付与された時に呼び出されるメソッド
        public virtual void Apply()
        {
        }

        // 状態異常が更新される時に呼び出されるメソッド
        public virtual void UpdateStatsEffect()
        {
            duration = _data.Duration;
        }

        // 状態異常が解除された時に呼び出されるメソッド
        public virtual void Remove()
        {
        }

        // 状態異常の残り時間を減少させるメソッド
        public virtual void DecreaseDuration(int time = 1)
        {
            if (FLG.FLGCheck((uint)Flgs, (uint)EffectFlgs.Duration))
            {
                duration -= time;
                if (Duration < 0) duration = 0;
                if (IsExpired)
                {
                    parent.RemoveEffect(this);
                }
            }

        }

        public abstract void ExecuteEffect(Action action = null);
    }
}
