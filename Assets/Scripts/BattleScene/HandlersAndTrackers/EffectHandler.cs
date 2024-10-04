using System;
using System.Collections.Generic;

namespace Contest
{
    public class EffectHandler
    {
        // フィールド
        public UnitBase parent;
        private EffectFlgs haveflgs = EffectFlgs.None;
        private bool flgsDirty = false;
        /// <summary>
        /// タイミングごとに効果を分類して管理するDictionary。
        /// EffectTimingと固有IDがキーになる。
        /// </summary>
        public Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>> effectsByTiming = new Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>>();

        // プロパティ
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

        // メソッド
        public void MakeFlgs()
        {
            haveflgs = EffectFlgs.None;
            foreach (var effects in effectsByTiming.Values)
            {
                foreach (var effect in effects.Values)
                {
                    EffectFlgs effectFlgs = effect.Flgs;
                    if (FLG.FLGCheck((uint)haveflgs, (uint)effectFlgs))
                    {
                        FLG.FLGUp((uint)haveflgs, (uint)effectFlgs);
                    }
                }
            }
        }
        public void EffectExecution(EffectTiming effectTiming)
        {
            // 指定されたタイミングの効果だけを実行
            if (effectsByTiming.ContainsKey(effectTiming))
            {
                var effectsToExecute = new List<StatusEffect>(effectsByTiming[effectTiming].Values);
                foreach (var effect in effectsToExecute)
                {
                    effect.ExecuteEffect();
                }
            }
        }

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

            if (effectGroup.ContainsKey(effect.ID))
            {
                var existingEffect = effectGroup[effect.ID];

                // 既存の効果に対して、マージが必要か確認
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
            flgsDirty = true;
        }
        public void RemoveEffect(StatusEffect effect)
        {
            if (effect == null) return;

            if (effectsByTiming.TryGetValue(effect.Timing, out var effectGroup) && effectGroup.Remove(effect.ID))
            {
                if (effectGroup.Count == 0)
                {
                    effectsByTiming.Remove(effect.Timing);
                }
            }

            flgsDirty = true;
        }


        public void ClearAllEffects()
        {
            effectsByTiming.Clear();
        }

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

        public EffectHandler(UnitBase unit)
        {
            parent = unit;
        }
    }
}
