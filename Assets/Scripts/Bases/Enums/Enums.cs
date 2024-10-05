using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    // 敵AIの思考パターンを示す列挙型。攻撃的か防御的か、支援型かを判定。
    public enum BehaviorPattern
    {
        Aggressive,     // 攻撃的
        Defensive,      // 防御的
        Support         // 支援型
    }

    // ユニットの種別をビットフラグで示す列挙型。ビット論理演算を使って、複数の状態を簡単に管理できる。
    [Flags]
    public enum UnitType
    {
        None = 0,           // なし
        Friend = 1 << 0,    // 味方
        Enemy = 1 << 1,     // 敵
        FriendAI = 1 << 2,  // 味方AI
        EnemyAI = 1 << 3,   // 敵AI
    }

    // ターンを示す列挙型。味方や敵のターンを区別し、全体ターンも表現できる。
    public enum Turn
    {
        None = 0,                                           // ターンなし
        Friend = UnitType.Friend | UnitType.FriendAI,       // 味方のターン
        Enemy = UnitType.Enemy | UnitType.EnemyAI,          // 敵のターン
        All = 0b1111                                        // 全体のターン (味方と敵の両方)
    }

    /// <summary>
    /// バフやデバフの各条件を管理するためのフラグ。エフェクトの状態や特徴を定義。
    /// </summary>
    [Flags]
    public enum EffectFlgs
    {
        None = 0,
        ShouldMerge = 1 << 0,        // 複数のエフェクトが付与されたとき、1つのエフェクトに統合する
        IsDebuff = 1 << 1,           // デバフである
        Stackable = 1 << 2,          // エフェクトがスタック可能
        Controll = 1 << 3,           // エフェクトがユニットの行動を妨害
        Interruptible = 1 << 4,      // 他の効果で中断される
        RemoveOnHit = 1 << 5,        // 攻撃を受けるとエフェクトが消える
        SelfOnly = 1 << 6,           // 自分にのみ適用される
        Duration = 1 << 7,           // 持続時間がある
        CanRemove = 1 << 8,          // スキルで取り除ける
        IsDot = 1 << 9,              // 持続ダメージ (DoT)
        IsHot = 1 << 10,             // 継続回復 (HoT)
    }

    // エフェクトが発生するタイミングを定義するフラグ。ビット演算で複数のタイミングを同時に持てる。
    [Flags]
    public enum EffectTiming
    {
        None = 0,
        Before = 1 << 0,    // ターン開始時
        After = 1 << 1,     // ターン終了時
        Passive = 1 << 2,   // 常時適用
    }

    // スキルの種類を定義するフラグ。攻撃、防御、バフ、デバフなどをビット演算で管理。
    [Flags]
    public enum SkillFlgs
    {
        None = 0,
        Attack = 1 << 0,            // 攻撃スキル
        Defense = 1 << 1,           // 防御スキル
        Buff = 1 << 2,              // バフスキル
        Debuff = 1 << 3,            // デバフスキル
        Support = Buff | Debuff,    // サポートスキル (バフ・デバフの両方を持つ)
    }

    // ターゲティングパターンを定義するフラグ。味方や敵、単体・複数、ランダムターゲットなどをビット演算で表現。
    [Flags]
    public enum TargetingPattern
    {
        None = 0,           // なし
        Friend = 1 << 0,    // 味方
        Enemy = 1 << 1,     // 敵
        Solo = 1 << 2,      // 単体
        Duo = 1 << 3,       // 二体
        Trio = 1 << 4,      // 三体
        All = 1 << 5,       // 全て
        Random = 1 << 6,    // ランダム
        Select = 1 << 7,    // 選択
    }

    // エフェクトの種類を定義。ビジュアルエフェクトとしてスタンやハイライトを示す。
    public enum ParticleType
    {
        HighLight,  // ハイライト
        Stun,       // スタン
    }

    // アニメーションオプションを定義。アニメーションの挙動をフラグで管理。
    [Flags]
    public enum AnimationOptions
    {
        None = 0,           // なし
        Fitting = 1 << 0,   // 大きさを合わせる
        Once = 1 << 1,      // 一度のみ再生
    }

    /// <summary>
    /// AnimationHandlerへの引数を定義する列挙型。攻撃やダメージ受けアニメーションを指定。
    /// </summary>
    public enum AnimationType
    {
        NormalAttack,       // 通常攻撃
        SpecialAttack,      // 特殊攻撃
        TakeDamage,         // ダメージを受ける
        TakeHeavyDamage,    // 大ダメージを受ける
    }

    // カスタムフィルターのオプションを定義。特定の条件を満たすフィルタリングに使用。
    public enum CustomFilterOptions
    {
        NeedTags,           // 特定のタグが必要
        UpToParam,          // パラメータが上限まで
        DownToParam,        // パラメータが下限まで
    }
}
