using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// バフやデバフのデータを管理するScriptableObjectクラス。
    /// 効果の持続時間、数値や倍率、適用タイミングなどの情報を保持する。
    /// </summary>
    [CreateAssetMenu(menuName = "バフデバフ")]
    public class StatusEffectData : HasTags, IUseCustamClassData
    {
        // ステータス効果に対応するカスタムクラス名を設定
        [SerializeField] private string className;
        // 効果の初期持続時間
        [SerializeField, Min(0)] private int duration;
        // ダメージや回復などの数値を管理
        [SerializeField] private int amount;
        // 効果の倍率や影響度を管理
        [SerializeField] private float magnification;
        // バフ・デバフの特徴を表すフラグ (複数の効果を組み合わせ可能)
        [SerializeField] private EffectFlgs effect;
        // 効果が発動するタイミング (ターンの開始時、終了時など)
        [SerializeField] private EffectTiming timing;
        //
        [SerializeField] private DamageOptions damageOptions;
        // ゲーム上から隠す
        [SerializeField] private bool hideData;

        [SerializeField] private List<StatusEffectData> childdatas; 

        // プロパティ
        public string ClassName => className;
        public int Duration => duration;
        public int Amount => amount;
        public float Magnification => magnification;
        public EffectFlgs Flags => effect;
        public EffectTiming Timing => timing;
        public DamageOptions DamageOptions => damageOptions;
        public bool HideData => hideData;
        public List<StatusEffectData> Childdatas => childdatas;
    }
}
