using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Contest
{
    /// <summary>
    /// 状態異常 (バフ・デバフ) を表現する抽象クラス。
    /// 効果の持続時間や、状態異常が付与・解除された時の処理を定義する。
    /// </summary>
    public abstract class StatusEffect : IUniqueThing, IEffect
    {
        // 状態異常のデータ (持続時間や効果など)
        public StatusEffectData _data;
        // 状態異常を管理する親ハンドラー
        protected EffectHandler parent;
        private string name;
        private string description;
        private int duration; // 状態異常の持続時間

        /// <summary>
        /// 状態異常の名前を取得するプロパティ。
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// 状態異常の持続時間 (ターン数) を取得するプロパティ。
        /// </summary>
        public int Duration
        {
            get
            {
                return duration;
            }
        }

        // ユニークなIDを返す
        public Guid ID { get; protected set; }

        /// <summary>
        /// 状態異常が持続しているかどうかを確認するプロパティ。
        /// 持続時間が0以下であれば期限切れと判断する。
        /// </summary>
        public virtual bool IsExpired
        {
            get
            {
                return Duration <= 0;
            }
        }

        /// <summary>
        /// 状態異常を管理する親ハンドラーを取得または設定する。
        /// 一度設定した後は変更できない。
        /// </summary>
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

        /// <summary>
        /// 状態異常の効果が発動するタイミングを取得する。
        /// </summary>
        public EffectTiming Timing
        {
            get
            {
                return _data.Timing;
            }
        }

        /// <summary>
        /// 状態異常のフラグ (バフ・デバフの特徴) を取得する。
        /// </summary>
        public EffectFlgs Flgs
        {
            get
            {
                return _data.Effect;
            }
        }

        /// <summary>
        /// 状態異常が付与された時に呼び出されるメソッド。
        /// 派生クラスでオーバーライドして特定の処理を追加する。
        /// </summary>
        public virtual void Apply()
        {
        }

        /// <summary>
        /// 状態異常が更新される時に呼び出されるメソッド。
        /// デフォルトでは持続時間を`_data`から設定。
        /// </summary>
        public virtual void UpdateStatsEffect()
        {
            duration = _data.Duration;
        }

        /// <summary>
        /// 状態異常が解除された時に呼び出されるメソッド。
        /// 派生クラスでオーバーライドして特定の処理を追加する。
        /// </summary>
        public virtual void Remove()
        {
        }

        /// <summary>
        /// 状態異常の持続時間を減少させるメソッド。
        /// 残りの持続時間が0以下になった場合、状態異常を削除する。
        /// </summary>
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

        /// <summary>
        /// 状態異常の効果を実行する抽象メソッド。
        /// 派生クラスで具体的な処理を実装する。
        /// </summary>
        /// <param name="action">状態異常に関連するアクション (オプション) 。</param>
        public abstract void ExecuteEffect(Action action = null);
    }
}
