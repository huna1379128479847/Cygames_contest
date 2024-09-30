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
}
