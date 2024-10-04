using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public static class TypeHolder
    {
        // フィールド
        private static Dictionary<Type, SkillData> skillDataSet;
        private static Dictionary<Type, StatusEffectData> statusEffectDataSet;

        // プロパティ
        public static Dictionary<Type, SkillData> SkillDataSet => skillDataSet;
        public static Dictionary<Type, StatusEffectData> StatusDataSet => statusEffectDataSet;

        // メソッド

        public static void SetDictionary(SkillData skillData)
        {
            Type type = Type.GetType(skillData.ClassName);
            if (type != null)
            {
                skillDataSet.Add(type, skillData);
            }
        }
        public static void SetDictionary(StatusEffectData statusEffectData)
        {
            Type type = Type.GetType(statusEffectData.ClassName);
            if (type != null)
            {
                statusEffectDataSet.Add(type, statusEffectData);
            }
        }

        public static Type GetClass<T>()
        {
            return typeof(T);
        }
    }
}
