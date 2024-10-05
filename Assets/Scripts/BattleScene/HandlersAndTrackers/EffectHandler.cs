using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// ユニットに付与される効果 (バフ・デバフ) を管理するクラス。
    /// 効果はタイミングごとに分類され、それに応じて実行、追加、削除される。
    /// </summary>
    public class EffectHandler
    {
        // 親ユニット
        public UnitBase parent;

        // 付与されている効果を管理するフラグ (バフ・デバフの種別)
        private EffectFlgs haveflgs = EffectFlgs.None;
        // フラグが変更されたかどうかを追跡
        private bool flgsDirty = false;

        /// <summary>
        /// タイミングごとに効果を分類して管理する辞書。
        /// キーはEffectTiming、値は固有IDと効果のペア。
        /// </summary>
        public Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>> effectsByTiming = new Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>>();

        // 現在のフラグを取得するプロパティ。フラグが変更された場合は再計算する。
        public EffectFlgs HaveFlgs
        {
            get
            {
                if (flgsDirty)
                {
                    MakeFlgs();
                    flgsDirty = false;
                }
                return haveflgs;
            }
        }

        /// <summary>
        /// 付与されている効果フラグを計算して更新する。
        /// </summary>
        public void MakeFlgs()
        {
            haveflgs = EffectFlgs.None;
            foreach (var effects in effectsByTiming.Values)
            {
                foreach (var effect in effects.Values)
                {
                    EffectFlgs effectFlgs = effect.Flgs;
                    if (!FLG.FLGCheck((uint)haveflgs, (uint)effectFlgs))
                    {
                        FLG.FLGUp((uint)haveflgs, (uint)effectFlgs);
                    }
                }
            }
            Debug.Log(haveflgs.ToString());
        }

        /// <summary>
        /// 指定されたタイミングの効果を実行する。
        /// </summary>
        /// <param name="effectTiming">効果を発動するタイミング。</param>
        public void EffectExecution(EffectTiming effectTiming)
        {
            // 指定されたタイミングの効果を実行
            if (effectsByTiming.ContainsKey(effectTiming))
            {
                var effectsToExecute = new List<StatusEffect>(effectsByTiming[effectTiming].Values);
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
        public void GiveEffect(StatusEffect effect)
        {
            if (effect == null) return; // 無効な効果は無視

            // 効果のタイミングごとに分類されているか確認し、なければ新規作成
            if (!effectsByTiming.ContainsKey(effect.Timing))
            {
                effectsByTiming[effect.Timing] = new Dictionary<Guid, StatusEffect>();
            }

            // 指定されたタイミングの効果グループを取得
            var effectGroup = effectsByTiming[effect.Timing];

            // 既存の効果がある場合は更新、ない場合は新規追加
            if (effectGroup.ContainsKey(effect.ID))
            {
                var existingEffect = effectGroup[effect.ID];

                // 既存の効果に対して、マージが必要か確認し、必要なら更新
                if (FLG.FLGCheck((uint)existingEffect.Flgs, (uint)EffectFlgs.ShouldMerge))
                {
                    existingEffect.UpdateStatsEffect();
                }
            }
            else
            {
                // 新しい効果を追加し、Parentとしてこのオブジェクトを設定
                effectGroup[effect.ID] = effect;
                effect.Parent = this;
            }

            // フラグの再計算を要求
            flgsDirty = true;
        }

        /// <summary>
        /// 指定された効果をユニットから削除する。
        /// </summary>
        /// <param name="effect">削除する効果。</param>
        public void RemoveEffect(StatusEffect effect)
        {
            if (effect == null) return;

            // タイミングごとの効果グループから効果を削除
            if (effectsByTiming.TryGetValue(effect.Timing, out var effectGroup) && effectGroup.Remove(effect.ID))
            {
                if (effectGroup.Count == 0)
                {
                    effectsByTiming.Remove(effect.Timing);
                }
            }

            // フラグの再計算を要求
            flgsDirty = true;
        }

        /// <summary>
        /// すべての効果を削除する。
        /// </summary>
        public void ClearAllEffects()
        {
            effectsByTiming.Clear();
        }

        /// <summary>
        /// 指定されたタグを持つ最初の効果を取得する。
        /// </summary>
        /// <param name="tag">検索するタグ。</param>
        /// <returns>一致する効果があれば返す。なければnull。</returns>
        public StatusEffect GetFirstEffect(string tag)
        {
            foreach (var effectUnique in effectsByTiming.Values)
            {
                foreach (var effect in effectUnique.Values)
                {
                    if (effect._data.Tags.Contains(tag))
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
        /// <returns>一致する効果があれば返す。なければnull。</returns>
        public StatusEffect GetStatusEffect(StatusEffect statusEffect)
        {
            foreach (var effectUnique in effectsByTiming.Values)
            {
                foreach (var effect in effectUnique.Values)
                {
                    if (effect._data.Name == statusEffect.Name)
                    {
                        return effect;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// コンストラクタ。ユニットを指定して初期化。
        /// </summary>
        /// <param name="unit">管理するユニット。</param>
        public EffectHandler(UnitBase unit)
        {
            parent = unit;
        }
    }
}
