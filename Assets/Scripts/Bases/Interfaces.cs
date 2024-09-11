using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit
    {
        UnitType UnitType { get; }
        string Name { get; }
        void Update();
    }

    public interface IEnemy
    {
        BehaviorPattern BehaviorPattern { get; }
        void EnemyBehavior();
    }

    public interface IStats
    {
        int MaxHP { get; }
        int CurrentHP { get; }
        int MaxMP { get; }
        int CurrentMP { get; }
        int MaxSpeed { get; }
        int CurrentSpeed { get; }
        int Atk { get; }
        int Def { get; }
    }
}
