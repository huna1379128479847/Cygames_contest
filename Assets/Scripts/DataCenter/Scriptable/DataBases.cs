using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// 基本データを持つクラス。名前と説明を管理。
    /// ScriptableObjectを継承しているので、Unityエディタからインスタンスを作成して管理できる。
    /// </summary>
    public class DataBase : ScriptableObject
    {
        // データの名前
        [SerializeField] private string _name; // 名前
        // データの説明
        [SerializeField] private string description; // 説明

        /// <summary>
        /// データの名前
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// データの説明
        /// </summary>
        public string Description => description;
    }

    /// <summary>
    /// タグを持つデータを表現するクラス。
    /// 基本データに加えて、タグのリストを持つ。
    /// </summary>
    public class HasTags : DataBase
    {
        // タグのリストをシリアライズしてUnityエディタで管理
        [SerializeField] private HashSet<string> tags;

        /// <summary>
        /// データに紐づくタグのリスト
        /// </summary>
        public HashSet<string> Tags => tags;

        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }
    }
}
