using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Contest
{
    [CreateAssetMenu(menuName = "バフデバフ")]
    public class StatusEffectData : DataBase
    {
        [SerializeField] private string className;
        [SerializeField, Min(0)] private int duration;        // 初期の持続時間
        [SerializeField] private int amount;                  // ダメージや回復など各種数値を管理。
        [SerializeField] private float magnification;           // 倍率などの数字を格納する用。
        public StatusEffect effect;
        public int Duration => duration;
        public int Amount => amount;
        public float Magnification => magnification;
        private void Awake()
        {
            Type type = Type.GetType(className) ?? null;
            if (type != null)
            {
                effect = (StatusEffect)Activator.CreateInstance(type);
            }
            else
            {
                Debug.LogError($"class:{className}が見つかりませんでした。");
            }
        }
    }
}
