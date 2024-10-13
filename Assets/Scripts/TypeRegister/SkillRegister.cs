using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    // SkillData型のファクトリーを管理するクラス
    public class SkillRegister : IFactoryHolder<SkillData>
    {        
        private static Dictionary<SkillData, IFactory> factoryHolder = new Dictionary<SkillData, IFactory>();

        // SkillDataを登録するメソッド。
        // データがnullでない場合、ClassNameプロパティを使用してクラスをNamespaceHeadと連結して登録
        public void RegisterFactory(SkillData data)
        {
            if (data.ClassName == null)
            {
                Debug.LogError($"入力値が不正です{data.Name}");
            }
            Type type = Type.GetType(data.ClassName + Constants.FactoryPostfix);
            if (type != null)
            {
                factoryHolder[data] = Activator.CreateInstance(type) as IFactory;
            }
        }

        // SkillDataに対応するTypeを取得するメソッド
        public IFactory GetFactoryForKey(SkillData data)
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
        public bool RemoveFactory(SkillData data)
        {
            if (factoryHolder.ContainsKey(data))
            {
                return factoryHolder.Remove(data);
            }
            return false;
        }

        // SkillDataが既に登録されているか確認するメソッド
        public bool ContainsKey(SkillData data)
        {
            return factoryHolder.ContainsKey(data);
        }
    }
}
