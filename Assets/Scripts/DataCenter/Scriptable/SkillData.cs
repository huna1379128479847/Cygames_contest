using System.Collections.Generic;
using UnityEngine;
using System;

namespace Contest
{
    /// <summary>
    /// スキルデータを管理するScriptableObjectクラス。
    /// スキルに必要なコスト、ダメージ、倍率、アニメーションなどの情報を保持し、
    /// ターゲットパターンや追加効果 (バフ・デバフ) も設定可能。
    /// </summary>
    [CreateAssetMenu(menuName = "スキル")]
    public class SkillData : HasTags, IUseCustamClassData
    {
        // IFactoryを継承したクラス名を格納。特定のスキルに対応するクラス名を設定。
        [SerializeField] private string className = "Skill";
        // スキルに必要なコスト
        [SerializeField, Min(0)] private int cost;
        // ダメージや回復量などの数値を管理
        [SerializeField] private int amount;
        // 倍率など、スキルの効果を調整するための倍率を管理
        [SerializeField] private float magnification;
        // スキルが攻撃かどうかを示すフラグ
        [SerializeField] private bool isAttack;
        // スキルがデバフや悪影響を与えるかどうかを示すフラグ
        [SerializeField] private bool isBad;
        // スキルのターゲティングパターン (単体、範囲、ランダムなど)
        [SerializeField] private TargetingPattern pattern = TargetingPattern.None;
        // スキルにバフ・デバフ効果がある場合、そのデータを保持
        [SerializeField] private List<StatusEffectData> statusEffectDatas = new List<StatusEffectData>();
        // スキル発動時のアニメーションタイプを管理
        [SerializeField] private AnimationType animationType = AnimationType.NormalAttack;
        // スキルに関連するフラグ (攻撃、防御、バフなどの種類を示す)
        [SerializeField] private SkillTypes skillTypes = SkillTypes.None;
        // 
        [SerializeField] private DamageOptions damageOptions = DamageOptions.None;
        // スキルに対するフィルタリング (例: 特定の条件でスキルが適用されるなど)
        [SerializeField] private List<SkillFilter> filter = new List<SkillFilter>();


        // プロパティ
        public string ClassName => className;
        public int Cost => cost;
        public int Amount => amount;
        public float Magnification => magnification;
        public bool IsAttack => isAttack;
        public bool IsBad => isBad;
        public TargetingPattern Pattern => pattern;
        public List<StatusEffectData> StatusEffectDatas => statusEffectDatas;
        public SkillTypes SkillTypes => skillTypes;
        public DamageOptions DamageOptions => damageOptions;
        public AnimationType AnimationType => animationType;
    }
}
