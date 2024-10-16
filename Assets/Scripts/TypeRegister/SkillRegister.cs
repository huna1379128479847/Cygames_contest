using System;
using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    // SkillData型のファクトリーを管理するクラス
    public class SkillRegister : IFactoryHolder<IUseCustamClassData>
    {
        private static Dictionary<IUseCustamClassData, IFactory> factoryHolder = new Dictionary<IUseCustamClassData, IFactory>();

        // SkillDataを登録するメソッド。
        // データがnullでない場合、ClassNameプロパティを使用してクラスをNamespaceHeadと連結して登録
        public void RegisterFactory(IUseCustamClassData data)
        {
            if (data.ClassName == null)
            {
                Debug.LogError($"入力値が不正です: ClassNameがnull {data.Name}");
                return;  // 早期リターン
            }

            try
            {
                Type type = Type.GetType(Constants.GetFactory(data.ClassName, true));
                if (type != null)
                {
                    var factoryInstance = Activator.CreateInstance(type) as IFactory;
                    if (factoryInstance != null)
                    {
                        factoryHolder[data] = factoryInstance;
                    }
                    else
                    {
                        Debug.LogError($"Activator.CreateInstance failed: {data.ClassName} に対応するファクトリがnullです");
                    }
                }
                else
                {
                    Debug.LogError($"Type not found for: {data.ClassName}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception in RegisterFactory: {e.Message} for SkillData: {data.Name}");
            }
        }


        // SkillDataに対応するTypeを取得するメソッド
        public IFactory GetFactoryForKey(IUseCustamClassData data)
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
        public bool RemoveFactory(IUseCustamClassData data)
        {
            if (factoryHolder.ContainsKey(data))
            {
                return factoryHolder.Remove(data);
            }
            return false;
        }

        // SkillDataが既に登録されているか確認するメソッド
        public bool ContainsKey(IUseCustamClassData data)
        {
            return factoryHolder.ContainsKey(data);
        }
    }
}
