using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    /// <summary>
    /// ユニットのターゲット選別に関するメソッドを提供するユーティリティクラス。
    /// </summary>
    public static class Target
    {
        /// <summary>
        /// 指定されたユニットが敵かどうかを判別する。
        /// </summary>
        /// <param name="unit">判別対象のユニット。</param>
        /// <returns>敵であればtrue、そうでなければfalse。</returns>
        public static bool IsEnemy(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Enemy | UnitType.EnemyAI));
        }

        /// <summary>
        /// 指定されたユニットが味方かどうかを判別する。
        /// </summary>
        /// <param name="unit">判別対象のユニット。</param>
        /// <returns>味方であればtrue、そうでなければfalse。</returns>
        public static bool IsFriend(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI));
        }

        /// <summary>
        /// 指定されたユニットがAIかどうかを判別する。
        /// </summary>
        /// <param name="unit">判別対象のユニット。</param>
        /// <returns>AIであればtrue、そうでなければfalse。</returns>
        public static bool IsAI(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.FriendAI | UnitType.EnemyAI));
        }

        /// <summary>
        /// 指定されたユニットが人間（プレイヤー操作）かどうかを判別する。
        /// </summary>
        /// <param name="unit">判別対象のユニット。</param>
        /// <returns>人間操作であればtrue、そうでなければfalse。</returns>
        public static bool IsHuman(UnitBase unit)
        {
            return FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Friend | UnitType.Enemy));
        }

        /// <summary>
        /// 指定されたユニットのHPが満タンではないかを判別する。
        /// </summary>
        /// <param name="unit">判別対象のユニット。</param>
        /// <returns>満タンでなければtrue、満タンならfalse。</returns>
        public static bool NotFullHP(UnitBase unit)
        {
            return unit.statusTracker.CurrentHP.CurrentAmount != unit.statusTracker.MaxHP.CurrentAmount;
        }
    }
}
