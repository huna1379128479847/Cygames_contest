using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// ユニットに付与される効果 (バフ・デバフ) を管理するクラス。
    /// 効果はタイミングごとに分類され、それに応じて実行、追加、削除される。
    /// </summary>
    public class EffectHandler : MonoBehaviour
    {
        private Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>> effectByTiming 
            = new Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>>();
        // 現在のフラグを取得するプロパティ。フラグが変更された場合は再計算する。
        private EffectFlgs haveFlgs = EffectFlgs.None;
        private bool flgsDirty = false;
        // 親ユニット
        public UnitBase ParentUnit { get; private set; }
        public EffectFlgs HaveFlgs
        {
            get
            {
                if (flgsDirty)
                {
                    RecalculateFlags();
                    flgsDirty = false;
                }
                return haveFlgs;
            }
        }
        
        // 付与されている効果を管理するタイミングごとの辞書
        public Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>> EffectsByTiming { get; private set; }
                    = new Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>>();
        /// <summary>
        /// 初期化処理。親ユニットを設定する。
        /// </summary>
        /// <param name="unit">親ユニット。</param>
        public void Initialize(UnitBase unit)
        {
            ParentUnit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        /// <summary>
        /// 付与されている効果フラグを計算して更新する。
        /// </summary>
        private void RecalculateFlags()
        {
            haveFlgs = EffectFlgs.None;
            foreach (var timingEffects in EffectsByTiming.Values)
            {
                foreach (var effect in timingEffects.Values)
                {
                    haveFlgs |= effect.Flags;
                }
            }
            Debug.Log($"Updated Effect Flags: {haveFlgs}");
        }

        /// <summary>
        /// 指定されたタイミングの効果を実行する。
        /// </summary>
        /// <param name="effectTiming">効果を発動するタイミング。</param>
        public void ExecuteEffects(EffectTiming effectTiming)
        {
            if (EffectsByTiming.ContainsKey(effectTiming))
            {
                var effectsToExecute = new List<StatusEffect>(EffectsByTiming[effectTiming].Values);
                foreach (var effect in effectsToExecute)
                {
                    effect.ExecuteEffect();
                }
            }
        }

        /// <summary>
        /// 新しい効果をユニットに付与する。
        /// </summary>
        /// <param name="effect">付与する効果。</param>
        public void AddEffect(StatusEffect effect)
        {
            if (effect == null)
                throw new ArgumentNullException(nameof(effect));

            if (!EffectsByTiming.ContainsKey(effect.Timing))
            {
                EffectsByTiming[effect.Timing] = new Dictionary<Guid, StatusEffect>();
            }

            var effectGroup = EffectsByTiming[effect.Timing];

            if (effectGroup.ContainsKey(effect.ID))
            {
                var existingEffect = effectGroup[effect.ID];
                // 既存の効果とマージするかどうかの判定
                if (existingEffect.Flags.HasFlag(EffectFlgs.ShouldMerge))
                {
                    existingEffect.UpdateEffect();
                }
                else
                {
                    Debug.LogWarning($"Effect {effect.Name} already exists and cannot be merged.");
                }
            }
            else
            {
                effectGroup.Add(effect.ID, effect);
                effect.Apply();
            }

            flgsDirty = true;
        }

        /// <summary>
        /// 指定された効果をユニットから削除する。
        /// </summary>
        /// <param name="effect">削除する効果。</param>
        public void RemoveEffect(StatusEffect effect)
        {
            if (effect == null)
                throw new ArgumentNullException(nameof(effect));

            if (EffectsByTiming.TryGetValue(effect.Timing, out var effectGroup))
            {
                if (effectGroup.Remove(effect.ID))
                {
                    effect.Remove();
                    if (effectGroup.Count == 0)
                    {
                        EffectsByTiming.Remove(effect.Timing);
                    }
                    flgsDirty = true;
                }
            }
        }

        /// <summary>
        /// すべての効果を削除する。
        /// </summary>
        public void ClearAllEffects()
        {
            foreach (var timingEffects in EffectsByTiming.Values)
            {
                foreach (var effect in timingEffects.Values)
                {
                    effect.Remove();
                }
            }
            EffectsByTiming.Clear();
            flgsDirty = true;
        }

        /// <summary>
        /// 指定されたタグを持つ最初の効果を取得する。
        /// </summary>
        /// <param name="tag">検索するタグ。</param>
        /// <returns>一致する効果があれば返す。なければnull。</returns>
        public StatusEffect GetFirstEffect(string tag)
        {
            foreach (var timingEffects in EffectsByTiming.Values)
            {
                foreach (var effect in timingEffects.Values)
                {
                    if (effect.Data.Tags.Contains(tag))
                    {
                        return effect;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 指定された効果を検索して返す。
        /// </summary>
        /// <param name="statusEffect">検索する効果。</param>
        /// <returns>一致する効果があればtrue。なければfalse。</returns>
        public bool GetStatusEffect(StatusEffect statusEffect)
        {
            foreach (var effect in EffectsByTiming[statusEffect.Data.Timing].Values)
            {
                if (effect.Name == statusEffect.Name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
