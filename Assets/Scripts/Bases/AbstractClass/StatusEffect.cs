using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public abstract class StatusEffect : IUniqueThing
    {
        protected UnitBase parent;
        public string Name { get; protected set; }
        public int Duration { get; protected set; } // 状態異常の持続時間（ターン数）
        public string ID { get; protected set; }

        public StatsEffect effect;

        // 状態異常が持続しているかどうかを確認するプロパティ    
        public virtual bool IsExpired
        {
            get { return Duration <= 0; }
        }

        public UnitBase Parent
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

        // 状態異常が付与された時に呼び出されるメソッド
        public abstract void Apply();

        // 状態異常が更新される時に呼び出されるメソッド
        public abstract void UpdateStatsEffect();

        // 状態異常が解除された時に呼び出されるメソッド
        public abstract void Remove();

        // 状態異常の残り時間を減少させるメソッド
        protected virtual void DecreaseDuration(int time = 1)
        {
            Duration -= time;
            if (Duration < 0) Duration = 0;
        }

        public abstract void ExecuteEffect(Action action = null);
    }
}
