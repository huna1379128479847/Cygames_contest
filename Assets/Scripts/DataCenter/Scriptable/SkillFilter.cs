using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// スキル用カスタムフィルターを簡単に作成するための補助的なクラス
    /// </summary>
    public class SkillFilter : DataBase
    {
        [SerializeField] private CustomFilterOptions customFilterOptions;
        /// <summary>
        /// フィルターオプションがNeedTagsの場合必須
        /// 書き方は「(ユニットorエフェクト).タグ名」
        /// </summary>
        [SerializeField] private List<string> needTags;

        /// <summary>
        /// フィルターオプションがUpToParamまたはDownToParamの時必須。
        /// 書き方は「（パラメータ名）」のみ。変数名を正確に入れる必要がある
        /// </summary>
        [SerializeField] private string param;

        /// <summary>
        /// フィルターオプションがUpToParamまたはDownToParamの時必須。
        /// 単位は％
        /// </summary>
        [SerializeField] private float num;

        public CustomFilterOptions Filter => customFilterOptions;
        public List<string> NeedTags => needTags;
        public string Param => param;
        public float Num => num;
    }
}
