using System.Collections.Generic;
using UnityEngine;
using System;

namespace Contest
{
    [CreateAssetMenu(menuName = "スキル")]
    public class SkillData : HasTags
    {
        [SerializeField] private string className = "Skill";
        [SerializeField, Min(0)] private int cost;            // 必要なコスト
        [SerializeField] private int amount;                  // ダメージや回復など各種数値を管理。
        [SerializeField] private float magnification;           // 倍率などの数字を格納する用。
        [SerializeField] private bool isAttack;
        [SerializeField] private bool isBad;
        [SerializeField] private List<TargetingPattern> patterns;
        [SerializeField] private StatusEffectData statusEffectData = null; //追加のバフデバフがあるなら追加すること
        [SerializeField] private AnimationType animationType;
        private TargetingPattern targetingPattern = 0;
        public string ClassName => className;
        public int Cost => cost;
        public int Amount => amount;
        public float Magnification => magnification;
        public bool IsAttack => isAttack;
        public bool IsBad => isBad;
        public TargetingPattern Pateren
        {
            get
            {
                if (targetingPattern != 0) { return targetingPattern; }
                TargetingPattern pattern = 0;
                foreach(var item in patterns)
                {
                    pattern |= item;
                }
                return pattern;
            }
        }
        public StatusEffectData StatusEffectData => statusEffectData;
        public AnimationType AnimationType => animationType;
    }
}
