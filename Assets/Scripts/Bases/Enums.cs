using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    //敵Aiの思考パターンを示す
    public enum BehaviorPattern
    {
        Aggressive,//攻撃的
        Defensive,//防御的
        Support//支援型
    }

    //そのユニットの種別を示す
    [Flags]
    public enum UnitType
    {
        None = 0,           // なし
        Friend = 1 << 0,    // 味方
        Enemy = 1 << 1,     // 敵
        FriendAI = 1 << 2,  // 味方AI
        EnemyAI = 1 << 3,   // 敵AI
    }

    // BattleSceneで使用:現在のターンを示す
    public enum Turn
    {
        None = 0,                              // ターンなし
        Friend = UnitType.Friend | UnitType.FriendAI,  // 味方のターン
        Enemy = UnitType.Enemy | UnitType.EnemyAI      // 敵のターン
    }
}
