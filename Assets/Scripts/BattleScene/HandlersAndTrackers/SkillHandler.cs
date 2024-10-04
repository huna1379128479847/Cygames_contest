using System.Collections.Generic;
using System;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// Unitのスキルを保持するクラス。
    /// </summary>
    public class SkillHandler
    {
        public Dictionary<Guid, Skill> skills = new Dictionary<Guid, Skill>();
        public UnitBase parent;
        public Skill currentSkill;

        public bool InAnimation
        {
            get
            {
                return currentSkill != null && currentSkill.InAction;
            }
        }

        public void ExecuteSkill()
        {
            if (currentSkill != null)
            {
                currentSkill.SetAction();
            }
        }

        public void SetSkill(Skill skill)
        {
            if (skills.ContainsKey(skill.ID))
            {
                currentSkill = skills[skill.ID];
            }
            else
            {
                Debug.LogError($"{skill}が見つかりませんでした。");
            }
        }

        public void InitSkillList(List<SkillData> skillDatas)
        {
            foreach (var skillData in skillDatas)
            {
                Type type = null;

                // SkillData から型を取得
                if (TypeHolder.SkillDataSet.TryGetValue(skillData, out type))
                {
                    // Skillのインスタンスを作成
                    Skill skill = Activator.CreateInstance(type, skillData, this) as Skill;

                    // スキルの辞書に追加
                    if (skill != null)
                    {
                        skills.TryAdd(skill.ID, skill);
                    }
                }
            }
        }

        public SkillHandler(UnitBase parent)
        {
            this.parent = parent;
        }
    }
}