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
        // フィールド
        [SerializeField] private BehaviorPattern enemyPattern;

        // プロパティ
        public BehaviorPattern EnemyPattern => enemyPattern;
    }
}
