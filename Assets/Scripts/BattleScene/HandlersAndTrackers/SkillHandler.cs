using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// ユニットのスキルを管理するクラス。
    /// スキルの追加、選択、実行、アニメーションの管理を行う。
    /// </summary>
    public class SkillHandler : MonoBehaviour, IHandler
    {
        // スキルの辞書。キーはスキルのユニークID、値はスキルオブジェクト
        public Dictionary<Guid, Skill> skills = new Dictionary<Guid, Skill>();

        // スキルを持つユニット
        private UnitBase parent;

        // 現在選択されているスキル
        public Skill currentSkill;

        /// <summary>
        /// 現在アニメーションが実行中かどうかを判定するプロパティ。
        /// </summary>
        public bool InAnimation
        {
            get
            {
                return currentSkill != null && currentSkill.InAction;
            }
        }

        public UnitBase Parent => parent;

        /// <summary>
        /// スキルを実行するメソッド。
        /// </summary>
        /// <param name="skill">実行するスキル。</param>
        /// <param name="targets">スキルの対象となるユニットのリスト。</param>
        public void ExecuteSkill(Skill skill, List<UnitBase> targets)
        {
            if (skill != null && skill.CanUse)
            {
                skill.InvokeSkill(targets);
            }
            else
            {
                Debug.Log($"Skill {skill?.skillData.Name ?? "Unknown"} cannot be used.");
            }
        }

        /// <summary>
        /// スキルを設定し、選択状態にする。
        /// </summary>
        /// <param name="skill">選択するスキル。</param>
        public void SetSkill(Skill skill)
        {
            if (skills.ContainsKey(skill.ID))
            {
                currentSkill = skills[skill.ID]; // 選択されたスキルを現在のスキルに設定
            }
            else
            {
                Debug.LogError($"Skill with ID {skill.ID} not found.");
            }
        }

        /// <summary>
        /// スキルデータのリストからスキルを初期化し、辞書に登録する。
        /// </summary>
        /// <param name="skillDatas">スキルデータのリスト。</param>
        public void InitSkillList(List<SkillData> skillDatas)
        {
            foreach (var skillData in skillDatas)
            {
                IFactory skillFactory = BattleSceneManager.instance.FactoryHolders.GetFactory(skillData);
                if (skillFactory != null)
                {
                    Skill skill = skillFactory.CreateClass(skillData, this) as Skill;
                    if (!skills.ContainsKey(skill.ID))
                    {
                        skills.Add(skill.ID, skill);
                    }
                    else
                    {
                        Debug.LogWarning($"Skill with ID {skill.ID} already exists. Skipping.");
                    }
                }
                else
                {
                    Debug.LogError($"Failed to create skill for SkillData {skillData.ClassName}.");
                }
            }
        }

        /// <summary>
        /// 初期化処理。
        /// </summary>
        private void Awake()
        {
            // parent がアタッチされていない場合、UnitBase コンポーネントを取得
            if (parent == null)
            {
                parent = GetComponent<UnitBase>();
                if (parent == null)
                {
                    Debug.LogError("SkillHandler requires a UnitBase component.");
                }
            }

            // スキルリストを初期化
            InitSkillList(parent.unitData.SkillDatas);
        }

        /// <summary>
        /// 現在選択されているスキルを実行するメソッド。
        /// 主に SkillSelectManager から呼び出される。
        /// </summary>
        /// <param name="targets">スキルの対象となるユニットのリスト。</param>
        public void ExecuteCurrentSkill(List<UnitBase> targets)
        {
            if (currentSkill != null)
            {
                ExecuteSkill(currentSkill, targets);
            }
            else
            {
                Debug.LogWarning("No skill is currently selected.");
            }
        }
    }
}
