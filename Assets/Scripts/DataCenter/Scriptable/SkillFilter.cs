using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// スキル用カスタムフィルターを簡単に作成するための補助的なクラス。
    /// スキルが特定の条件を満たすかどうかを判定し、発動条件や効果範囲を制御する。
    /// </summary>
    public class SkillFilter : DataBase
    {
        // カスタムフィルターオプション (NeedTags、UpToParam、DownToParamなど)
        [SerializeField] private CustomFilterOptions customFilterOptions;

        /// <summary>
        /// フィルターオプションがNeedTagsの場合に必須となるタグのリスト。
        /// 書き方は「(ユニットorエフェクト).タグ名」。
        /// </summary>
        [SerializeField] private List<string> needTags;

        /// <summary>
        /// フィルターオプションがUpToParamまたはDownToParamの時に必須となるパラメータ名。
        /// 書き方は「パラメータ名」のみで、正確な変数名を使用する必要がある。
        /// </summary>
        [SerializeField] private string param;

        /// <summary>
        /// フィルターオプションがUpToParamまたはDownToParamの時に必須となる値。
        /// 単位は％で指定する。
        /// </summary>
        [SerializeField] private float num;

        // フィルターの種類を取得するプロパティ
        public CustomFilterOptions Filter => customFilterOptions;

        // 必須タグのリストを取得するプロパティ (NeedTags オプションの場合のみ使用)
        public List<string> NeedTags => needTags;

        // パラメータ名を取得するプロパティ (UpToParam または DownToParam オプションの場合のみ使用)
        public string Param => param;

        // 数値を取得するプロパティ (UpToParam または DownToParam オプションの場合のみ使用)
        public float Num => num;
    }
}
