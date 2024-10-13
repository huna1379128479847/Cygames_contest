using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public class FireBall : Skill
    {
        public FireBall(SkillData skillData, IHandler skillHandler)
            : base(skillData, skillHandler) { }
    }
}
