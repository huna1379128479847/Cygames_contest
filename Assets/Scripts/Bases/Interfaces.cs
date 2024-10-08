using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    // ユニットを表現する基本的なインターフェース。ユニットの種類や行動を定義する。
    public interface IUnit
    {
        string Name { get; } // ユニットの名前
        UnitType MyUnitType { get; } // ユニットの種類
        SkillHandler SkillHandler { get; } // スキルを管理するハンドラー
        void TurnBehavior(); // ターン中の行動を定義
    }

    // ユニークな存在を表すインターフェース。各インスタンスに一意のIDを持たせる。
    public interface IUniqueThing
    {
        Guid ID { get; } // 一意のID
    }

    // プレイヤー選択肢を管理するためのインターフェース。選択肢の生成と選択処理を担当。
    public interface IPlayerHandler
    {
        void GeneratePlayerOptions(); // プレイヤーの選択肢を生成
        void HandlePlayerSelection(int selectionId); // 選択されたオプションの処理
    }

    // アクションを実行中かどうかを示すインターフェース。アクションの進行状況を管理する。
    public interface IDoAction
    {
        bool InAction { get; } // アクション中かどうか
    }

    // 敵ユニットを表現するインターフェース。行動パターンを定義。
    public interface IEnemy
    {
        BehaviorPattern BehaviorPattern { get; } // 敵の行動パターン
    }

    // ユニットのステータスを表現するインターフェース。HPやMPなどのステータスを持つ。
    public interface IStatus
    {
        StatusBase MaxHP { get; } // 最大HP
        StatusBase CurrentHP { get; } // 現在のHP
        StatusBase MaxMP { get; } // 最大MP
        StatusBase CurrentMP { get; } // 現在のMP
        StatusBase MaxSpeed { get; } // 最大速度
        StatusBase CurrentSpeed { get; } // 現在の速度
        StatusBase Atk { get; } // 攻撃力
        StatusBase Def { get; } // 防御力
    }

    // システムやシーンの動作を管理するためのインターフェース。実行状態を管理。
    public interface IManager
    {
        bool IsRunning { get; set; } // システムが稼働中かどうか
    }

    // シーンを切り替えるためのインターフェース。シーンの遷移を管理。
    public interface ISceneChanger
    {
        string FromSceneName { get; } // 現在のシーン名
        string ToSceneName { get; } // 遷移先のシーン名
        void Execute(string sceneName); // シーンを実行するメソッド
    }

    // データを格納し、取得・設定を行うためのインターフェース。親コンテナとの連携も可能。
    public interface IDataContainer
    {
        IDataContainer Parent { get; } // 親コンテナ
        object GetData(); // データを取得
        void SetData(object data); // データを設定
    }

    // 効果（バフやデバフなど）を定義するインターフェース。効果の適用や管理を行う。
    public interface IEffect
    {
        string Name { get; } // 効果の名前
        int Duration { get; } // 効果の持続時間
        EffectTiming Timing { get; } // 効果が発動するタイミング
        EffectFlgs Flgs { get; } // 効果のフラグ（特徴や条件）
        void Apply(); // 効果を適用
        void Remove(); // 効果を取り除く
        void UpdateStatsEffect(); // ステータスの影響を更新
        void DecreaseDuration(int time = 1); // 効果の残り時間を減らす
        void ExecuteEffect(Action action = null); // 効果を実行
    }

    // バトル中のイベントを定義するインターフェース。特定タイミングでの処理を管理。
    public interface IBattleEvent
    {
        int Priority { get; } // イベントの優先度
        void Invoke(); // イベントを発動
    }
}
