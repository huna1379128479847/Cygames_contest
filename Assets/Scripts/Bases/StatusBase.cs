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
        // スレッドセーフなコレクションを使用
        // キーのIDはGUIDを使う
        protected Dictionary<string, float> magnification;
        protected Dictionary<string, int> effectAmount;
        protected int defaultAmount;
        protected int currentAmount;
        private bool isDirty; // 変更フラグ

        public virtual int CurrentAmount
        {
            get
            {
                // Return the latest computed amount
                if (isDirty)
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
        public StatusBase(int amount)
        {
            defaultAmount = amount;
            currentAmount = amount; // 初期値設定
            magnification = new Dictionary<string, float>();
            effectAmount = new Dictionary<string, int>();
            isDirty = true; // 初期状態は変更があると設定
            id = Guid.NewGuid();
        }

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
        public void AddEffect(string id, float amount)
        {
            magnification[id] = amount; // 値を更新または追加
            isDirty = true; // 変更があったことをフラグで示す
        }

        public void AddEffect(string id, int amount)
        {
            effectAmount[id] = amount; // 値を更新または追加
            isDirty = true; // 変更があったことをフラグで示す
        }

        // エフェクトを削除
        public void RemoveEffect(string id)
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
    }
}
