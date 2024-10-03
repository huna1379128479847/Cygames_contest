using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    /// <summary>
    /// 選択クラスで使用するデリゲート用のメソッドを定義するユーティリティクラス
    /// </summary>
    public static class Target
    {
        public static bool IsEnemy(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Enemy | UnitType.EnemyAI));
        }
        public static bool IsFriend(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI));
        }

        public static bool IsAI(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.FriendAI | UnitType.EnemyAI));
        }

        public static bool IsHuman(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Friend | UnitType.Enemy));
        }

        public static bool NotFullHP(UnitBase unit)
        {
            return unit.statusTracker.CurrentHP.CurrentAmount != unit.statusTracker.MaxHP.CurrentAmount;
        }
    }
}
