using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Contest
{
    public class InBattleFactoryHolder : IFactoryHolders
    {
        // フィールド
        private Dictionary<Type, IFactoryHolder<IUseCutomClass>> factoryHolders = new Dictionary<Type, IFactoryHolder<IUseCutomClass>>();

        // プロパティ
        public Dictionary<Type, IFactoryHolder<IUseCutomClass>> FactoryHolders => factoryHolders;

        public bool FinishedInit { get; private set; } = false;

        // コンストラクタ
        public InBattleFactoryHolder(List<UnitBase> unitBases)
        {
            factoryHolders[typeof(SkillData)] = new SkillRegister() as IFactoryHolder<IUseCutomClass>;
            factoryHolders[typeof(StatusEffectData)] = new EffectRegister() as IFactoryHolder<IUseCutomClass>;
            SetData(unitBases);
        }

        // メソッド
        public void SetData(List<UnitBase> unitBases)
        {
            foreach (UnitBase unitBase in unitBases)
            {
                if (unitBase.unitData.SkillDatas == null)
                {
                    UnityEngine.Debug.LogError("");
                    continue;
                }
                SetData(unitBase.unitData.SkillDatas);
            }
            FinishedInit = true;
        }

        public void SetData(List<SkillData> skillDatas)
        {

            foreach (var skillData in skillDatas)
            {
                factoryHolders[typeof(SkillData)].RegisterFactory(skillData);
                if (skillData.StatusEffectDatas != null && skillData.StatusEffectDatas.Count > 0)
                {
                    SetData(skillData.StatusEffectDatas);
                }
            }
        }

        public void SetData(List<StatusEffectData> effectDatas)
        {
            foreach (var effectData in effectDatas)
            {
                factoryHolders[typeof(StatusEffectData)].RegisterFactory(effectData);
            }
        }

        public IFactory GetFactory(IUseCutomClass data)
        {
            IFactory factory = null;
            try
            {
                factory = factoryHolders[data.GetType()].GetFactoryForKey(data);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
            }
            return factory;
        }
    }
}
