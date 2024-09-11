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
    public enum UnitType
    {
        Friend,//味方
        Enemy,//敵
        FriendAI,//味方AI
        EnemyAI,//敵AI
    }

    //BattleSceneで使用:現在のターンを示す
    public enum Tarn
    {
        Friend,//味方のターン
        Enemy,//敵のターン
    }


}
