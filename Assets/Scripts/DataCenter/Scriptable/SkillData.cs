using System.Collections.Generic;
using UnityEngine;
using System;

namespace Contest
{
    [CreateAssetMenu(menuName = "スキル")]
    public class SkillData : HasTags
    {
        // フィールド
        [SerializeField] private string className = "Skill";                // カスタムクラス名を入れる。
        [SerializeField, Min(0)] private int cost;                          // 必要なコスト
        [SerializeField] private int amount;                                // ダメージや回復など各種数値を管理。
        [SerializeField] private float magnification;                       // 倍率などの数字を格納する用。
        [SerializeField] private bool isAttack;                             // 
        [SerializeField] private bool isBad;                                // 
        [SerializeField] private TargetingPattern pattern;                  // 
        [SerializeField] private StatusEffectData statusEffectData = null;  // 追加のバフデバフがあるなら追加すること
        [SerializeField] private AnimationType animationType;               // 
        [SerializeField] private SkillFlgs skillFlgs;                       // 
        [SerializeField] private List<SkillFilter> filter;                  // 


        // プロパティ
        public string ClassName => className;
        public int Cost => cost;
        public int Amount => amount;
        public float Magnification => magnification;
        public bool IsAttack => isAttack;
        public bool IsBad => isBad;
        public TargetingPattern Pattern => pattern;
        public StatusEffectData StatusEffectData => statusEffectData;
        public SkillFlgs SkillFlgs => skillFlgs;
        public AnimationType AnimationType => animationType;
    }
}
