using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Contest
{
    public class InBattleFactoryHolder : IFactoryHolders
    {
        // フィールド
        private Dictionary<Type, IFactoryHolder<IUseCustamClassData>> factoryHolders = new Dictionary<Type, IFactoryHolder<IUseCustamClassData>>();

        // プロパティ
        public Dictionary<Type, IFactoryHolder<IUseCustamClassData>> FactoryHolders => factoryHolders;

        public bool FinishedInit { get; private set; } = false;

        public InBattleFactoryHolder()
        {
            var skillRegister = new SkillRegister() as IFactoryHolder<IUseCustamClassData>;
            if (skillRegister != null)
            {
                factoryHolders[typeof(SkillData)] = skillRegister;
            }
            else
            {
                Debug.LogError("SkillRegisterがIFactoryHolder<IUseCutomClass>としてキャストできませんでした。");
            }

            var effectRegister = new EffectRegister() as IFactoryHolder<IUseCustamClassData>;
            if (effectRegister != null)
            {
                factoryHolders[typeof(StatusEffectData)] = effectRegister;
            }
            else
            {
                Debug.LogError("EffectRegisterがIFactoryHolder<IUseCutomClass>としてキャストできませんでした。");
            }
        }



        // メソッド
        public void SetData(List<UnitBase> unitBases)
        {
            foreach (UnitBase unitBase in unitBases)
            {
                if (unitBase.UnitData.SkillDatas == null)
                {
                    UnityEngine.Debug.LogError("");
                    continue;
                }
                SetData(unitBase.UnitData.SkillDatas);
            }
            FinishedInit = true;
        }

        public void SetData(List<SkillData> skillDatas)
        {

            foreach (var skillData in skillDatas)
            {
                factoryHolders[typeof(SkillData)].RegisterFactory(skillData);// ここ
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
                if (effectData.ClassName == "none") return;
                factoryHolders[typeof(StatusEffectData)].RegisterFactory(effectData);
                if (effectData.Childdatas != null)
                {
                    SetData(effectData.Childdatas);
                }
            }
        }

        public IFactory GetFactory(IUseCustamClassData data)
        {
            IFactory factory = null;
            if (data.ClassName == "none") return  factory;
            Type type = data.GetType();
            if (!factoryHolders.ContainsKey(type))
            {
                UnityEngine.Debug.LogError($"{data.GetType()}");
            }
            try
            {
                factory = factoryHolders[type].GetFactoryForKey(data);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(data.ClassName + ex.ToString());
            }
            return factory;
        }
    }
}
