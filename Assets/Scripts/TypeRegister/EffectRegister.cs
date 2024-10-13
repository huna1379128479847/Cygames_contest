using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    public class EffectRegister : IFactoryHolder<StatusEffectData>
    {
        private static Dictionary<StatusEffectData, IFactory> factoryHolder = new Dictionary<StatusEffectData, IFactory>();

        // SkillDataを登録するメソッド。
        // データがnullでない場合、ClassNameプロパティを使用してクラスをNamespaceHeadと連結して登録
        public void RegisterFactory(StatusEffectData data)
        {
            if (data.ClassName == null)
            {
                Debug.LogError($"入力値が不正です{data.Name}");
            }
            Type type = Type.GetType(data.ClassName);
            if (type != null)
            {
                factoryHolder[data] = Activator.CreateInstance(type) as IFactory;
            }
        }

        // SkillDataに対応するTypeを取得するメソッド
        public IFactory GetFactoryForKey(StatusEffectData data)
        {
            if (factoryHolder.TryGetValue(data, out IFactory factory))
            {
                return factory;
            }
            else
            {
                Debug.Log($"入力値が不正です{data.Name}");
                return null;
            }
        }

        // SkillDataに対応する型情報を削除するメソッド
        public bool RemoveFactory(StatusEffectData data)
        {
            if (factoryHolder.ContainsKey(data))
            {
                return factoryHolder.Remove(data);
            }
            return false;
        }

        // SkillDataが既に登録されているか確認するメソッド
        public bool ContainsKey(StatusEffectData data)
        {
            return factoryHolder.ContainsKey(data);
        }
    }
}
