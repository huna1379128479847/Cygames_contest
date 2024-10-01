using System;
using System.Collections.Generic;

namespace Contest
{
    public class EffectHandler
    {
        public UnitBase parent;

        /// <summary>
        /// タイミングごとに効果を分類して管理するDictionary。
        /// EffectTimingと固有IDがキーになる。
        /// </summary>
        public Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>> effectsByTiming = new Dictionary<EffectTiming, Dictionary<Guid, StatusEffect>>();

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
        }


        public void RemoveEffect(StatusEffect effect)
        {
            if (effect == null) return;

            // 効果のタイミングを確認し、該当するタイミングの効果グループから削除
            if (effectsByTiming.ContainsKey(effect.Timing))
            {
                var effectGroup = effectsByTiming[effect.Timing];
                if (effectGroup.ContainsKey(effect.ID))
                {
                    effectGroup[effect.ID].Remove();
                    effectGroup.Remove(effect.ID);

                    // 効果グループが空になった場合はタイミングのDictionaryから削除
                    if (effectGroup.Count == 0)
                    {
                        effectsByTiming.Remove(effect.Timing);
                    }
                }
            }
        }

        public void ClearAllEffects()
        {
            effectsByTiming.Clear();
        }
    }
}
