using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class SkillHandler
    {
        public Dictionary<SkillFlgs, Dictionary<string, Skill>> skills = new Dictionary<SkillFlgs, Dictionary<string, Skill>>();
        public UnitBase parent;

    }
}
