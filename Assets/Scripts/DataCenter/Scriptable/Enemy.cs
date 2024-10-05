using UnityEngine;

namespace Contest
{
    /// <summary>
    /// 敵ユニットのデータを定義するScriptableObjectクラス。
    /// 敵ユニットは行動パターン (BehaviorPattern) を持ち、エディタから設定できる。
    /// </summary>
    [CreateAssetMenu(menuName = "敵ユニット")]
    public class Enemy : UnitData
    {
        // 敵の行動パターン (攻撃的、防御的、支援型など)
        [SerializeField] private BehaviorPattern enemyPattern;

        /// <summary>
        /// 敵の行動パターンを取得するプロパティ。
        /// </summary>
        public BehaviorPattern EnemyPattern => enemyPattern;
    }
}
