using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Contest
{
    /// <summary>
    /// ゲーム内で使う汎用的なヘルパーメソッドをまとめたクラス。
    /// リストや辞書操作、ランダム要素の選択に便利。
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// リストからランダムに1つの要素を選ぶ。
        /// </summary>
        /// <typeparam name="T">リストの要素の型。</typeparam>
        /// <param name="values">要素を持つリスト。</param>
        /// <returns>ランダムに選ばれた要素。リストが空またはnullの場合はデフォルト値を返す。</returns>
        public static T RandomPick<T>(List<T> values)
        {
            if (values == null || values.Count == 0) return default;
            return values[UnityEngine.Random.Range(0, values.Count)];
        }

        /// <summary>
        /// 辞書の値からランダムに1つの要素を選ぶ。
        /// </summary>
        /// <typeparam name="T">辞書のキーの型。</typeparam>
        /// <typeparam name="V">辞書の値の型。</typeparam>
        /// <param name="values">要素を持つ辞書。</param>
        /// <returns>ランダムに選ばれた値。辞書が空またはnullの場合はデフォルト値を返す。</returns>
        public static V RandomPick<T, V>(Dictionary<T, V> values)
        {
            if (values == null || values.Count == 0) return default;
            return values.Values.ElementAt(UnityEngine.Random.Range(0, values.Count));
        }

        /// <summary>
        /// 辞書の値をリストに変換する。
        /// </summary>
        /// <typeparam name="T">辞書のキーの型。</typeparam>
        /// <typeparam name="V">辞書の値の型。</typeparam>
        /// <param name="values">辞書。</param>
        /// <returns>辞書の全ての値を持つリスト。</returns>
        public static List<V> DictionaryToList<T, V>(Dictionary<T, V> values)
        {
            return values.Values.ToList();
        }
    }
}
