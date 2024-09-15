using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    public class DataBase : ScriptableObject
    {
        [SerializeField] private string _name; // 名前
        [SerializeField] private string description; // 説明

        /// <summary>
        /// データの名前
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// データの説明
        /// </summary>
        public string Description
        {
            get => description;
            set => description = value;
        }
    }
    //-----------------------------------------//

    [CreateAssetMenu(menuName = "バフデバフ")]
    public class StatusEffectData : DataBase
    {
        [SerializeField, Min(0)] private int duration;        // 初期の持続時間
        [SerializeField] private int amount;                  // ダメージや回復など各種数値を管理。
        [SerializeField] private int magnification;           // 倍率などの数字を格納する用。

        public int Duration => duration;
        public int Amount => amount;
        public int Magnification => magnification;
    }
    [CreateAssetMenu(menuName = "スキル")]
    public class SkillData : DataBase
    {
        [SerializeField, Min(0)] private int cost;            // 必要なコスト
        [SerializeField] private int amount;                  // ダメージや回復など各種数値を管理。
        [SerializeField] private int magnification;           // 倍率などの数字を格納する用。

        public int Cost => cost;
        public int Amount => amount;
        public int Magnification => magnification;
    }

    [CreateAssetMenu(menuName = "ユニット")]
    public class UnitData : DataBase
    {
        [SerializeField, Min(0)] private int hp;
        [SerializeField, Min(0)] private int mp;
        [SerializeField, Min(0)] private int atk;
        [SerializeField, Min(0)] private int def;
        [SerializeField, Min(0)] private int speed;
        [SerializeField] private UnitType unitType;

        /// <summary>
        /// ユニットの最大HP
        /// </summary>
        public int HP => hp;

        /// <summary>
        /// ユニットの最大MP
        /// </summary>
        public int MP => mp;

        /// <summary>
        /// ユニットの攻撃力
        /// </summary>
        public int Atk => atk;

        /// <summary>
        /// ユニットの防御力
        /// </summary>
        public int Def => def;

        /// <summary>
        /// ユニットのスピード
        /// </summary>
        public int Speed => speed;

        /// <summary>
        /// ユニットの種類
        /// </summary>
        public UnitType UnitType => unitType;
    }

}
