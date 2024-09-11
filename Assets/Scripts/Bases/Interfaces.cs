using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit//ユニット用
    {
        UnitType UnitType { get; }
        string Name { get; }
        void Update();
    }

    public interface IEnemy//敵ユニット用
    {
        BehaviorPattern BehaviorPattern { get; }
        void EnemyBehavior();
    }

    public interface IStats//ユニット用
    {
        int MaxHP { get; }
        int CurrentHP { get; }//現在HP
        int MaxMP { get; }
        int CurrentMP { get; }
        int MaxSpeed { get; }
        int CurrentSpeed { get; }
        int Atk { get; }
        int Def { get; }
    }
}
