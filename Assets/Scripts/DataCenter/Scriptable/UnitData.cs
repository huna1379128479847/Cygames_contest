using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
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
