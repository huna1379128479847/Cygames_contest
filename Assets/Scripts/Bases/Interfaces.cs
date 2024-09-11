using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit//���j�b�g�p
    {
        UnitType UnitType { get; }
        string Name { get; }
        void Update();
    }

    public interface IEnemy//�G���j�b�g�p
    {
        BehaviorPattern BehaviorPattern { get; }
        void EnemyBehavior();
    }

    public interface IStats//���j�b�g�p
    {
        int MaxHP { get; }
        int CurrentHP { get; }//����HP
        int MaxMP { get; }
        int CurrentMP { get; }
        int MaxSpeed { get; }
        int CurrentSpeed { get; }
        int Atk { get; }
        int Def { get; }
    }
}
