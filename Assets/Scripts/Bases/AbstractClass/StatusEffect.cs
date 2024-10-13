using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Contest
{
    /// <summary>
    /// 状態異常 (バフ・デバフ) を表現する抽象クラス。
    /// 効果の持続時間や、状態異常が付与・解除された時の処理を定義する。
    /// </summary>
    public abstract class StatusEffect : IUniqueThing, IEffect
    {
        public delegate void RemoveEffect(Guid guid);
        public event RemoveEffect RemoveEvent;

        // 状態異常のデータ (持続時間や効果など)
        public StatusEffectData Data { get; protected set; }
        // 状態異常を管理する親ハンドラー
        protected EffectHandler ParentHandler { get; private set; }
        // ユニークID
        public Guid ID { get; protected set; }

        // 状態異常の名前
        public string Name => Data.Name;
        // 状態異常の説明
        public string Description => Data.Description;
        // 状態異常の持続時間 (ターン数)
        public int Duration { get; protected set; }

        /// <summary>
        /// 状態異常が持続しているかどうかを確認するプロパティ。
        /// 持続時間が0以下であれば期限切れと判断する。
        /// </summary>
        public virtual bool IsExpired => Duration <= 0;

        /// <summary>
        /// 状態異常の効果が発動するタイミングを取得するプロパティ。
        /// </summary>
        public EffectTiming Timing => Data.Timing;

        /// <summary>
        /// 状態異常のフラグ (バフ・デバフの特徴) を取得するプロパティ。
        /// </summary>
        public EffectFlgs Flags => Data.Flags;

        /// <summary>
        /// コンストラクタ。状態異常データと親ハンドラーを指定して初期化する。
        /// </summary>
        /// <param name="data">状態異常データ。</param>
        /// <param name="parentHandler">親の効果ハンドラー。</param>
        public StatusEffect(StatusEffectData data, IHandler parentHandler)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            ParentHandler = parentHandler as EffectHandler ?? throw new ArgumentNullException(nameof(parentHandler));
            ID = Guid.NewGuid();
            Duration = Data.Duration;
        }

        /// <summary>
        /// 状態異常が付与された時に呼び出されるメソッド。
        /// 派生クラスでオーバーライドして特定の処理を追加する。
        /// </summary>
        public virtual void Apply()
        {
            // 例
            // StatusBase speed = ParentHandler.ParentUnit.statusTracker.CurrentSpeed;
            // speed.AddEffect(ID, +0.1f);
            // RemoveEvent += speed.RemoveEffect;
        }

        /// <summary>
        /// 状態異常が更新される時に呼び出されるメソッド。
        /// デフォルトでは持続時間を`Data`から設定。
        /// </summary>
        public virtual void UpdateEffect()
        {
            // 持続時間をリセットまたは更新
            Duration = Data.Duration;
        }

        /// <summary>
        /// 状態異常が解除された時に呼び出されるメソッド。
        /// 派生クラスでオーバーライドして特定の処理を追加する。
        /// </summary>
        public virtual void Remove()
        {
            RemoveEvent?.Invoke(ID);
        }

        /// <summary>
        /// 状態異常の持続時間を減少させるメソッド。
        /// 残りの持続時間が0以下になった場合、状態異常を削除する。
        /// </summary>
        /// <param name="time">減少させるターン数。</param>
        public virtual void DecreaseDuration(int time = 1)
        {
            if (Flags.HasFlag(EffectFlgs.Duration))
            {
                Duration -= time;
                if (Duration < 0)
                    Duration = 0;

                if (IsExpired)
                {
                    ParentHandler.RemoveEffect(this);
                }
            }
        }

        /// <summary>
        /// 状態異常の効果を実行する抽象メソッド。
        /// 派生クラスで具体的な処理を実装する。
        /// </summary>
        /// <param name="action">状態異常に関連するアクション (オプション) 。</param>
        public abstract void ExecuteEffect();
    }
}
