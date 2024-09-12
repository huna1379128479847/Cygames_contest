using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit // ユニット用
    {
        string Name { get; }
        UnitType MyUnitType { get; }
        UnitBase MyUnitBase { get; }
        SkillTracker SkillTracker { get; }
        void TurnBehavior();
    }

    public interface IPlayerHandler // プレイヤー用の選択肢生成
    {
        // プレイヤーの選択肢を生成するメソッドの例
        void GeneratePlayerOptions();

        // プレイヤーの選択肢が選ばれた時の処理
        void HandlePlayerSelection(int selectionId);
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

    public interface IManager
    {
        /// <summary>
        /// このマネージャーまたはシーンが稼働中かどうかを管理します。
        /// 稼働中でない場合、すべての動作が一時停止します。
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// このマネージャーがインスタンス化されたとき初めに呼ばれるメソッドです。
        /// 初期化やセットアップ処理をここで行います。
        /// </summary>
        void Execute();
    }

}
