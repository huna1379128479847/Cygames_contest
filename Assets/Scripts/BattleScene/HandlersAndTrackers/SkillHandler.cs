using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Contest
{
    public class SkillHandler
    {
        public Dictionary<SkillFlgs, Dictionary<Guid, Skill>> skills = new Dictionary<SkillFlgs, Dictionary<Guid, Skill>>();
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
            currentSkill.SetAction();
        }
        public void SetSkill(Guid guid)
        {
            foreach (var skill in skills.Values)
            {
                if (skill.ContainsKey(guid))
                {
                    currentSkill = skill[guid];
                }
            }
        }

        public SkillHandler(UnitBase parent)
        {
            this.parent = parent;
        }
    }
}
