using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public class BattleData : DataBase
    {
        [SerializeField] private List<EnemyBase> enemies;
        [SerializeField] private List<UnitBase> otherUnits;
        [SerializeField] private List<InBattleEvent> inBattleEvents;

        public List<EnemyBase> Enemies => enemies;
        public List <UnitBase> OtherUnits => otherUnits;
        public List<InBattleEvent> InBattleEvents => inBattleEvents;
    }
}
