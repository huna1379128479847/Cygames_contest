using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Contest
{
    public static class Helpers
    {
        // リストからランダムに1つ選ぶ
        public static T RandomPick<T>(List<T> values)
        {
            if (values == null || values.Count == 0) return default;
            return values[Random.Range(0, values.Count)];
        }

        // 辞書の値からランダムに1つ選ぶ
        public static V RandomPick<T, V>(Dictionary<T, V> values)
        {
            if (values == null || values.Count == 0) return default;
            return values.Values.ElementAt(Random.Range(0, values.Count));
        }

        // 辞書の値をリストに変換する
        public static List<V> DictionaryToList<T, V>(Dictionary<T, V> values)
        {
            return values.Values.ToList();
        }
    }
}
