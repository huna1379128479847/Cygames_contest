using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public static class TypeHolder
    {
        // デリゲート＆イベント
        public delegate void SetSkillClass(List<Type> types);
        public static event SetSkillClass SetSkill;
        public delegate void SetEffectClass (List<Type> types);
        public static event SetEffectClass SetEffect;
        // フィールド
        private static Dictionary<SkillData, Type> skillDataSet = new Dictionary<SkillData, Type>();
        private static Dictionary<StatusEffectData, Type> statusEffectDataSet = new Dictionary<StatusEffectData, Type>();

        // プロパティ
        public static Dictionary<SkillData, Type> SkillDataSet => skillDataSet;
        public static Dictionary<StatusEffectData, Type> StatusDataSet => statusEffectDataSet;

        // メソッド

        // SkillDataに基づいて辞書にTypeを追加
        public static void SetDictionary(SkillData skillData)
        {
            if (skillDataSet.ContainsKey(skillData)) { return; }
            if (skillData == null || string.IsNullOrEmpty(skillData.ClassName))
            {
                Debug.LogError("SkillDataまたはそのClassNameが不正です。");
                return;
            }

            Type type = Type.GetType(skillData.ClassName);
            if (type != null)
            {
                if (!skillDataSet.ContainsKey(skillData))
                {
                    skillDataSet.Add(skillData, type);
                }
                else
                {
                    Debug.LogWarning($"SkillData {skillData.ClassName} は既に辞書に存在します。");
                }
            }
            else
            {
                Debug.LogError($"クラス名 '{skillData.ClassName}' に対応する型が見つかりません。");
            }
        }

        // StatusEffectDataに基づいて辞書にTypeを追加
        public static void SetDictionary(StatusEffectData statusEffectData)
        {
            if (statusEffectDataSet.ContainsKey(statusEffectData)) { return; }
            if (statusEffectData == null || string.IsNullOrEmpty(statusEffectData.ClassName))
            {
                Debug.LogError("StatusEffectDataまたはそのClassNameが不正です。");
                return;
            }

            Type type = Type.GetType(statusEffectData.ClassName);
            if (type != null)
            {
                if (!statusEffectDataSet.ContainsKey(statusEffectData))
                {
                    statusEffectDataSet.Add(statusEffectData, type);
                }
                else
                {
                    Debug.LogWarning($"StatusEffectData {statusEffectData.ClassName} は既に辞書に存在します。");
                }
            }
            else
            {
                Debug.LogError($"クラス名 '{statusEffectData.ClassName}' に対応する型が見つかりません。");
            }
        }

        // SkillDataの型を取得
        public static Type GetTypeDictionary(SkillData skillData)
        {
            if (skillDataSet.ContainsKey(skillData))
            {
                return skillDataSet[skillData];
            }
            else
            {
                Debug.LogError($"SkillData {skillData.ClassName} に対応する型が辞書に存在しません。");
                return null;
            }
        }

        // StatusEffectDataの型を取得
        public static Type GetTypeDictionary(StatusEffectData statusEffectData)
        {
            if (statusEffectDataSet.ContainsKey(statusEffectData))
            {
                return statusEffectDataSet[statusEffectData];
            }
            else
            {
                Debug.LogError($"StatusEffectData {statusEffectData.ClassName} に対応する型が辞書に存在しません。");
                return null;
            }
        }
    }
}
