using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Contest
{
    public class PowerBarrirer : Skill
    {
        private int maxCount = 2;
        private int count = 1;
        public PowerBarrirer(SkillData skillData, IHandler skillHandler)
            : base(skillData, skillHandler) { }

        public override List<DamageInfo> ApplySkill(List<UnitBase> units, MiniGameResult result)
        {
            UnitBase unit = parent.Parent;
            List<DamageInfo> infos = new List<DamageInfo>();
            unit.StatusTracker.Def.AddStackedEffect(new Guid(), skillData.Amount, count, maxCount);
            DamageInfo info = new DamageInfo(this, unit, unit);
            info.isOccurredDamage = false;
            infos.Add(info);
            count++;
            Debug.Log($"{unit.Name}の防御力が増加した！ 初期値：{unit.StatusTracker.Def.DefaultAmount}　現在値：{unit.StatusTracker.Def.CurrentAmount}");
            return infos;
        }
    }
}
