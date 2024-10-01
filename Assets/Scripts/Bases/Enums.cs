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
        Aggressive,     // 攻撃的
        Defensive,      // 防御的
        Support         // 支援型
    }

    //ビットの論理演算を使って管理している

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

    public enum Turn
    {
        None = 0,                                           // ターンなし
        Friend = UnitType.Friend | UnitType.FriendAI,       // 1: 味方のターン
        Enemy = UnitType.Enemy | UnitType.EnemyAI,          // 2: 敵のターン
        All = 0b1111                                        // 3: 反転用 (味方と敵の両方)
    }

    /// <summary>
    /// バフやデバフの各条件を管理するフラグ
    /// </summary>
    [Flags]
    public enum EffectFlgs
    {
        None = 0,
        ShouldMerge = 1 << 0,        // 複数のエフェクトが付与されたとき、1つのエフェクトに統合するかどうか。false (0) の場合、複数のエフェクトが個別に存在できる。
        IsDebuff = 1 << 1,           // デバフであるかどうか。
        Stackable = 1 << 2,          // エフェクトがスタック可能かどうか。
        Controll = 1 << 3,           // エフェクトがユニットの行動を妨害するかどうか。
        Interruptible = 1 << 4,      // エフェクトが他の効果によって中断されるかどうか。
        RemoveOnHit = 1 << 5,        // 攻撃を受けた際にエフェクトが取り除かれるかどうか。
        SelfOnly = 1 << 6,           // エフェクトが自分自身にのみ適用されるかどうか。
        Duration = 1 << 7,           // エフェクトに持続時間があるかどうか。
        CanRemove = 1 << 8,          // 持続時間や特定の条件以外、プレイヤーや敵のスキルにより取り除けるかどうか。
        IsDot = 1 << 9,              // 持続ダメージ
        IsHot = 1 << 10,             // 継続回復
    }

    [Flags]
    public enum EffectTiming
    {
        None = 0,
        Before = 1 << 0,    // ターン開始時
        After = 1 << 1,     // ターン終了時
        Passive = 1 << 2,   // 常時効果適用
    }

    [Flags]
    public enum SkillFlgs
    {
        None = 0,
        Attack = 1 << 0,            // 攻撃スキル
        Defense = 1 << 1,           // 防御スキル
        Buff = 1 << 2,              // バフスキル
        Debuff = 1 << 3,            // デバフスキル
        Support = Buff | Debuff,    // サポートスキル
    }

    [Flags]
    public enum TargetingPateren
    {
        None = 0,           //
        Friend = 1 << 0,    // 味方
        Enemy = 1 << 1,     // 敵
        Solo = 1 << 2,      // 単体
        Duo = 1 << 3,       // 二体まで
        Trio = 1 << 4,      // 三体まで
        All = 1 << 5,       // 全て
        Random = 1 << 6,    // ランダム
        Select = 1 << 7,    // 選ぶ
    }
    // エフェクトの種類
    public enum ParticleType
    {
        HighLight,  // ハイライト
        Stun,       // スタン
    }

    [Flags]
    public enum ParticleOptions
    {
        None = 0,           //
        Fitting = 1 << 0,   // 大きさを合わせる
        Once = 1 << 1,      // 一度のみ再生
    }
}