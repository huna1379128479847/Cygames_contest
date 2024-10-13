using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Contest
{
    public class StatusBase : IUniqueThing
    {
        // フィールド
        public string name;  // ステータス名
        protected Guid id;  // ユニークID
        protected Dictionary<Guid, float> magnification;  // 倍率の効果
        protected Dictionary<Guid, int> effectAmount;  // 直接的な加算効果
        protected int defaultAmount;  // 基本値
        protected int currentAmount;  // 現在の値
        protected bool needDirty;  // 値を強制的に再計算させるフラグ
        private bool isDirty;  // 値に変更があったかどうかを示すフラグ

        // イベント：死亡時に発火する
        public delegate void OnDead();
        public event OnDead OnDeadEvent;

        // プロパティ
        public UnitBase Parent { get; private set; }

        // ステータスの現在値を返すプロパティ。変更がある場合は再計算する。
        public virtual int CurrentAmount
        {
            get
            {
                // 値に変更があれば再計算する
                if (isDirty && !needDirty)
                {
                    RecalculateAmount();
                }
                return currentAmount;
            }
            set
            {
                currentAmount = value;

                // 現在の値が0以下であれば死亡イベントを発火
                if (IsDead && OnDeadEvent != null)
                {
                    OnDeadEvent.Invoke();
                }
            }
        }

        // ステータスが死亡状態かどうかを返すプロパティ
        public virtual bool IsDead
        {
            get
            {
                return currentAmount <= 0;
            }
        }

        // 初期設定されたステータスの基本値
        public int DefaultAmount => defaultAmount;

        // ユニークIDを返す
        public Guid ID => id;

        // コンストラクタ
        public StatusBase(int amount, bool needDirty = false)
        {
            defaultAmount = amount;
            currentAmount = amount;  // 初期値を設定
            magnification = new Dictionary<Guid, float>();
            effectAmount = new Dictionary<Guid, int>();
            isDirty = true;  // 初期状態では変更があると仮定
            id = Guid.NewGuid();
            this.needDirty = needDirty;
        }

        // ステータスの値を再計算する
        private void RecalculateAmount()
        {
            // 効果量と倍率の合計を計算
            float totalEffectAmount = effectAmount.Values.Sum();
            float totalMagnification = magnification.Values.Sum();

            // ステータスの現在値を再計算して設定
            currentAmount = (int)((defaultAmount + totalEffectAmount) * (1 + totalMagnification));

            // 再計算後、フラグをリセット
            isDirty = false;
        }

        // エフェクトを追加する（倍率を扱う）
        public void AddEffect(Guid id, float amount)
        {
            magnification[id] = amount;  // 値を追加または更新
            isDirty = true;  // 変更フラグをセット
        }

        // エフェクトを追加する（直接的な加算効果を扱う）
        public void AddEffect(Guid id, int amount)
        {
            effectAmount[id] = amount;  // 値を追加または更新
            isDirty = true;  // 変更フラグをセット
        }

        // エフェクトを更新する（加算効果）
        public void UpdateEffect(Guid id, int amount)
        {
            if (effectAmount.ContainsKey(id))
            {
                effectAmount[id] = amount;
            }
            else
            {
                AddEffect(id, amount);  // 存在しない場合は自動で追加
            }
        }

        // エフェクトを更新する（倍率効果）
        public void UpdateEffect(Guid id, float amount)
        {
            if (magnification.ContainsKey(id))
            {
                magnification[id] = amount;
            }
            else
            {
                AddEffect(id, amount);  // 存在しない場合は自動で追加
            }
        }

        // エフェクトを削除する（加算効果と倍率効果の両方を対象）
        public void RemoveEffect(Guid id)
        {
            bool removed = false;

            // 倍率効果の削除
            if (magnification.Remove(id))
            {
                removed = true;
            }

            // 加算効果の削除
            if (effectAmount.Remove(id))
            {
                removed = true;
            }

            // いずれかのエフェクトが削除された場合、再計算フラグをセット
            if (removed)
            {
                isDirty = true;
            }
        }

        // 特定のエフェクトが倍率に含まれているかをチェック
        public bool MagnificationContainsKey(Guid id)
        {
            return magnification.ContainsKey(id);
        }

        // 特定のエフェクトが加算効果に含まれているかをチェック
        public bool AmountContainsKey(Guid id)
        {
            return effectAmount.ContainsKey(id);
        }

        // スタック可能な加算効果を追加する。スタック数を考慮して加算。
        public void AddStackedEffect(Guid id, int baseAmount, int stackCount, int maxStack = int.MaxValue)
        {
            // スタック上限を考慮した合計効果量を計算
            int totalEffect = Math.Min(baseAmount * stackCount, baseAmount * maxStack);

            // すでにエフェクトが存在する場合は更新、存在しない場合は追加
            if (AmountContainsKey(id))
            {
                UpdateEffect(id, totalEffect);
            }
            else
            {
                AddEffect(id, totalEffect);
            }
        }

        // スタック可能な倍率効果を追加する。スタック数を考慮して加算。
        public void AddStackedEffect(Guid id, float baseAmount, int stackCount, int maxStack = int.MaxValue)
        {
            // スタック上限を考慮した合計効果量を計算
            float totalEffect = Math.Min(baseAmount * stackCount, baseAmount * maxStack);

            // すでにエフェクトが存在する場合は更新、存在しない場合は追加
            if (MagnificationContainsKey(id))
            {
                UpdateEffect(id, totalEffect);
            }
            else
            {
                AddEffect(id, totalEffect);
            }
        }

        // 全てのエフェクトを削除する
        public void ClearEffects()
        {
            // 倍率効果と加算効果の両方をクリア
            magnification.Clear();
            effectAmount.Clear();

            // 再計算フラグをセット
            isDirty = true;
        }
    }
}
