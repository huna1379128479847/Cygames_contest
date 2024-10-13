using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// KeyTypeとKeyCodeのマッピングを管理し、特定のキーが押されたかどうかを確認するためのクラス。
    /// ゲーム内でのキー入力の確認やカスタムマッピングに使用する。
    /// </summary>
    public static class KeyMappingHolder
    {
        // KeyTypeとKeyCodeのマッピング (複数のKeyCodeを1つのKeyTypeに関連付ける)
        private static Dictionary<KeyType, List<KeyCode>> inputKeyType = new Dictionary<KeyType, List<KeyCode>>();
        private readonly static Dictionary<KeyType, KeyCode> defaultKeyType = new Dictionary<KeyType, KeyCode>() 
        {
            { KeyType.Confirm, KeyCode.Z },
            { KeyType.Cancel, KeyCode.X},
            { KeyType.Up, KeyCode.UpArrow },
            { KeyType.Down, KeyCode.DownArrow },
            { KeyType.Right, KeyCode.RightArrow },
            { KeyType.Left, KeyCode.LeftArrow }
        };

        public static Dictionary<KeyType, List<KeyCode>> InputKeyType => inputKeyType;
        /// <summary>
        /// Confirmキーが押されているかどうかを確認するメソッド。
        /// KeyType.Confirmに関連付けられたキーが押されていればtrueを返す。
        /// </summary>
        public static bool IsPressConfirmKey()
        {
            return Check(KeyType.Confirm);
        }

        /// <summary>
        /// Cancelキーが押されているかどうかを確認するメソッド。
        /// KeyType.Cancelに関連付けられたキーが押されていればtrueを返す。
        /// </summary>
        public static bool IsPressCancelKey()
        {
            return Check(KeyType.Cancel);
        }

        /// <summary>
        /// 指定されたKeyTypeに関連するキーが押されているかどうかを確認する内部メソッド。
        /// </summary>
        /// <param name="type">確認するKeyType (例: Confirm, Cancel)</param>
        /// <returns>対応するKeyCodeが押されていればtrue。</returns>
        private static bool Check(KeyType type)
        {
            // 指定されたKeyTypeに関連するKeyCodeのリストを取得
            List<KeyCode> keys = inputKeyType[type];

            if (keys == null)
            {
                return Input.GetKeyDown(defaultKeyType[type]);
            }
            // リスト内のすべてのキーに対して、キーが押されたかどうかを確認
            foreach (var key in keys)
            {
                if (Input.GetKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
