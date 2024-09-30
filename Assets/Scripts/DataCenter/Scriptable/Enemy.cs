using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    [CreateAssetMenu(menuName = "敵ユニット")]
    public class Enemy : UnitData
    {
        [SerializeField] private BehaviorPattern enemyPattern;

        [SerializeField] private List<SkillData> skillDatas;

        public BehaviorPattern EnemyPattern => enemyPattern;
        public List<SkillData> SkillDatas => skillDatas;
    }
}
