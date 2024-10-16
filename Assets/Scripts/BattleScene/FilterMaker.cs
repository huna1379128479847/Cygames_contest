using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Contest
{

    /// <summary>
    /// フィルター用のデリゲートを作成するクラス
    /// </summary>
    public class FilterMaker
    {
        SkillFilter skillFilter;

        public bool FilterInvoke(UnitBase unit)
        {
            bool flg = true;
            if (FLG.FLGCheck((uint)skillFilter.Filter, (uint)CustomFilterOptions.NeedTags))
            {
                flg &= CheckTags(unit);
            }
            if (FLG.FLGCheck((uint)skillFilter.Filter, (uint)CustomFilterOptions.UpToParam | (uint)CustomFilterOptions.DownToParam))
            {
                flg &= CheckParam(unit);
            }
            return flg;
        }
        private bool CheckTags(UnitBase unit)
        {
            if (skillFilter.NeedTags == null)
            {
                Debug.LogError($"{skillFilter.Name}にタグが設定されていません。");
                return true;
            }
            EffectHandler handler = unit.EffectHandler;
            foreach (var tag in skillFilter.NeedTags)
            {
                string[] item = tag.Split(".");
                if (item[0] == "ユニット")
                {
                    if (!unit.UnitData.HasTag(item[1]))
                    {
                        return false;
                    }
                }
                else if (item[0] == "エフェクト")
                {
                    if (unit.EffectHandler.GetFirstEffect(item[1]) != null)
                    {
                        return true;
                    }
                }
                else
                {
                    Debug.LogError($"{skillFilter.Name}の{tag}にターゲットが設定されていません。");
                    continue;
                }
            }
            return true;
        }

        private bool CheckParam(UnitBase unit)
        {
            PropertyInfo info = unit.StatusTracker.GetType().GetProperty(skillFilter.Param);
            StatusBase status = info?.GetValue(unit) as StatusBase;
            if (info == null || status == null)
            {
                Debug.LogError($"{skillFilter.Name}の{skillFilter.Param}が存在しません。");
                return false;
            }
            if (skillFilter.Filter == CustomFilterOptions.UpToParam)
            {
                return status.CurrentAmount / status.DefaultAmount > skillFilter.Num;
            }
            else
            {
                return status.CurrentAmount / status.DefaultAmount < skillFilter.Num;
            }
        }
    }
}
