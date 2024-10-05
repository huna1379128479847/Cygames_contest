using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Contest
{
    public class StatusBase : IUniqueThing
    {
        public string name;
        public UnitBase Parent { get; private set; }
        protected Guid id;
        // キーのIDはGUIDを使う
        protected Dictionary<Guid, float> magnification;
        protected Dictionary<Guid, int> effectAmount;
        protected int defaultAmount;
        protected int currentAmount;
        protected bool needDirty;
        private bool isDirty; // 変更フラグ

        public virtual int CurrentAmount
        {
            get
            {
                // Return the latest computed amount
                if (isDirty && !needDirty)
                {
                    RecalculateAmount();
                }
                return currentAmount;
            }
            set
            {
                currentAmount = value;
            }
        }
        public Guid ID => id;
        public virtual bool IsDead
        {
            get
            {
                return currentAmount <= 0;
            }
        }

        public int DefaultAmount => defaultAmount;
        

        // ステータスの値を再計算
        private void RecalculateAmount()
        {
            float totalEffectAmount = effectAmount.Values.Sum();
            float totalMagnification = magnification.Values.Sum();

            // 計算後に現在のAmountを設定
            currentAmount = (int)((defaultAmount + totalEffectAmount) * (1 + totalMagnification));
            isDirty = false; // 再計算後にフラグをリセット
        }

        // エフェクトを追加
        public void AddEffect(Guid id, float amount)
        {
            magnification[id] = amount; // 値を更新または追加
            isDirty = true; // 変更があったことをフラグで示す
        }

        public void AddEffect(Guid id, int amount)
        {
            effectAmount[id] = amount; // 値を更新または追加
            isDirty = true; // 変更があったことをフラグで示す
        }

        // エフェクトを削除
        public void RemoveEffect(Guid id)
        {
            bool removed = false;
            if (magnification.Remove(id))
            {
                removed = true;
            }
            if (effectAmount.Remove(id))
            {
                removed = true;
            }
            if (removed)
            {
                isDirty = true; // 変更があったことをフラグで示す
            }

        }

        public StatusBase(int amount, bool needDirty = false)
        {
            defaultAmount = amount;
            currentAmount = amount; // 初期値設定
            magnification = new Dictionary<Guid, float>();
            effectAmount = new Dictionary<Guid, int>();
            isDirty = true; // 初期状態は変更があると設定
            id = Guid.NewGuid();
            this.needDirty = needDirty;
        }
    }
}
