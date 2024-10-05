using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// スキルやステータス効果の型を保持し、辞書で管理するクラス。
    /// デリゲートやイベントを使用して型情報を設定する機能も持つ。
    /// </summary>
    public static class TypeHolder
    {
        private const string namespacehead = "Contest.";
        // デリゲート＆イベント
        public delegate void SetSkillClass(List<Type> types);  // スキルクラスを設定するためのデリゲート
        public static event SetSkillClass SetSkill;            // スキルクラス設定用のイベント
        public delegate void SetEffectClass(List<Type> types); // ステータス効果クラスを設定するためのデリゲート
        public static event SetEffectClass SetEffect;          // ステータス効果クラス設定用のイベント

        // スキルやステータス効果のデータと、それに対応する型を保持する辞書
        private static Dictionary<SkillData, Type> skillDataSet = new Dictionary<SkillData, Type>();
        private static Dictionary<StatusEffectData, Type> statusEffectDataSet = new Dictionary<StatusEffectData, Type>();

        // プロパティで辞書へのアクセスを提供
        public static Dictionary<SkillData, Type> SkillDataSet => skillDataSet;
        public static Dictionary<StatusEffectData, Type> StatusDataSet => statusEffectDataSet;

        // メソッド

        /// <summary>
        /// SkillDataに基づいて辞書にTypeを追加する。
        /// 既に辞書に存在する場合は何もしない。
        /// </summary>
        public static void SetDictionary(SkillData skillData)
        {
            if (skillDataSet.ContainsKey(skillData)) { return; }
            if (skillData == null || string.IsNullOrEmpty(skillData.ClassName))
            {
                Debug.LogError("SkillDataまたはそのClassNameが不正です。");
                return;
            }

            Type type = Type.GetType(namespacehead + skillData.ClassName);
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

        /// <summary>
        /// StatusEffectDataに基づいて辞書にTypeを追加する。
        /// 既に辞書に存在する場合は何もしない。
        /// </summary>
        public static void SetDictionary(StatusEffectData statusEffectData)
        {
            if (statusEffectDataSet.ContainsKey(statusEffectData)) { return; }
            if (statusEffectData == null || string.IsNullOrEmpty(statusEffectData.ClassName))
            {
                Debug.LogError("StatusEffectDataまたはそのClassNameが不正です。");
                return;
            }

            Type type = Type.GetType(namespacehead + statusEffectData.ClassName);
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

        /// <summary>
        /// SkillDataに対応するTypeを取得する。
        /// </summary>
        /// <param name="skillData">型を取得したいSkillData</param>
        /// <returns>対応するType。存在しない場合はnull。</returns>
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

        /// <summary>
        /// StatusEffectDataに対応するTypeを取得する。
        /// </summary>
        /// <param name="statusEffectData">型を取得したいStatusEffectData</param>
        /// <returns>対応するType。存在しない場合はnull。</returns>
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
