using Contest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    [CreateAssetMenu(menuName = "スキル")]
    public class SkillData : DataBase
    {
        [SerializeField, Min(0)] private int cost;            // 必要なコスト
        [SerializeField] private int amount;                  // ダメージや回復など各種数値を管理。
        [SerializeField] private float magnification;           // 倍率などの数字を格納する用。
        [SerializeField] private bool isAttack;
        [SerializeField] private bool isBad;
        [SerializeField] private StatusEffectData statusEffectData = null;

        public int Cost => cost;
        public int Amount => amount;
        public float Magnification => magnification;
        public bool IsAttack => isAttack;
        public bool IsBad => isBad;
        public StatusEffectData StatusEffectData => statusEffectData;
    }
}
