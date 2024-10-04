using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    [CreateAssetMenu(menuName = "バフデバフ")]
    public class StatusEffectData : HasTags
    {
        [SerializeField] private string className;
        [SerializeField, Min(0)] private int duration;        // 初期の持続時間
        [SerializeField] private int amount;                  // ダメージや回復など各種数値を管理。
        [SerializeField] private float magnification;           // 倍率などの数字を格納する用。
        [SerializeField] private EffectFlgs effect;
        [SerializeField] private EffectTiming timing;
        public string ClassName => className;
        public int Duration => duration;
        public int Amount => amount;
        public float Magnification => magnification;
        public EffectFlgs Effect => effect;
        public EffectTiming Timing => timing;
    }
}
