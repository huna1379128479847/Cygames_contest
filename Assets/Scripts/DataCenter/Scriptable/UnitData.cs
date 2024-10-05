using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// ユニットデータを管理するScriptableObjectクラス。
    /// ユニットのHP、MP、攻撃力、防御力、スピードなどの基本ステータスを保持し、
    /// スキルのリストやユニットの種類 (UnitType) も管理する。
    /// </summary>
    [CreateAssetMenu(menuName = "ユニット")]
    public class UnitData : HasTags
    {
        // ユニットの最大HP
        [SerializeField, Min(0)] private int hp;
        // ユニットの最大MP
        [SerializeField, Min(0)] private int mp;
        // ユニットの攻撃力
        [SerializeField, Min(0)] private int atk;
        // ユニットの防御力
        [SerializeField, Min(0)] private int def;
        // ユニットのスピード
        [SerializeField, Min(0)] private int speed;
        // ユニットの種類 (例: 味方、敵、AIなど)
        [SerializeField] private UnitType unitType;
        // ユニットが使用できるスキルのリスト
        [SerializeField] private List<SkillData> skillDatas;

        /// <summary>
        /// ユニットの最大HPを取得するプロパティ。
        /// </summary>
        public int HP => hp;

        /// <summary>
        /// ユニットの最大MPを取得するプロパティ。
        /// </summary>
        public int MP => mp;

        /// <summary>
        /// ユニットの攻撃力を取得するプロパティ。
        /// </summary>
        public int Atk => atk;

        /// <summary>
        /// ユニットの防御力を取得するプロパティ。
        /// </summary>
        public int Def => def;

        /// <summary>
        /// ユニットのスピードを取得するプロパティ。
        /// </summary>
        public int Speed => speed;

        /// <summary>
        /// ユニットの種類 (UnitType) を取得するプロパティ。
        /// </summary>
        public UnitType UnitType => unitType;

        /// <summary>
        /// ユニットが持つスキルのリストを取得するプロパティ。
        /// </summary>
        public List<SkillData> SkillDatas => skillDatas;
    }
}
