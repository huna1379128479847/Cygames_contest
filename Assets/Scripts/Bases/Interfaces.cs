using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public interface IUnit // ユニット用
    {
        string Name { get; }
        UnitType MyUnitType { get; }
        SkillHandler SkillHandler { get; }
        void TurnBehavior();
    }

    public interface IUniqueThing
    {
        Guid ID { get; }
    }
    public interface IPlayerHandler // プレイヤー用の選択肢生成
    {
        // プレイヤーの選択肢を生成するメソッドの例
        void GeneratePlayerOptions();

        // プレイヤーの選択肢が選ばれた時の処理
        void HandlePlayerSelection(int selectionId);
    }

    public interface IDoAction
    {
        bool InAction { get; }
    }

    public interface IEnemy//敵ユニット用
    {
        BehaviorPattern BehaviorPattern { get; }
    }

    public interface IStatus//ユニット用
    {
        StatusBase MaxHP { get;}
        StatusBase CurrentHP { get; }//current = 現在
        StatusBase MaxMP { get; }
        StatusBase CurrentMP { get; }
        StatusBase MaxSpeed { get; }
        StatusBase CurrentSpeed { get; }
        StatusBase Atk { get; }
        StatusBase Def { get; }
    }

    public interface IManager
    {
        /// <summary>
        /// このマネージャーまたはシーンが稼働中かどうかを管理します。
        /// 稼働中でない場合、すべての動作が一時停止します。
        /// </summary>
        bool IsRunning { get; set; }
    }
    public interface ISceneChanger
    {
        string FromSceneName { get; }
        string ToSceneName { get; }
        void Execute(string sceneName);
    }

    public interface IDataContainer
    {
        // 子データコンテナや、関連するコンテナを持つ場合
        IDataContainer Parent { get; }

        // コンテナ内のデータを取得するためのメソッド
        object GetData();

        // コンテナにデータをセットするためのメソッド
        void SetData(object data);
    }
    public interface IEffect
    {
        string Name { get; }
        int Duration { get; }
        EffectTiming Timing { get; }
        EffectFlgs Flgs { get; }
        void Apply();
        void Remove();
        void UpdateStatsEffect();
        void DecreaseDuration(int time = 1);
        void ExecuteEffect(Action action = null);
    }

    public interface IHandler
    {

    }
}
