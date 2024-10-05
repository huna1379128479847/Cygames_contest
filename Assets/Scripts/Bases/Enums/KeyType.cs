using System;

namespace Contest
{
    /// <summary>
    /// キー入力を管理するための列挙型。
    /// フラグを使用して、複数のキー入力を組み合わせて扱えるようにする。
    /// </summary>
    [Flags]
    public enum KeyType
    {
        None = 0,            // キー入力なし
        Confirm = 1 << 0,    // 決定キー
        Cancel = 1 << 1,     // キャンセルキー
        Right = 1 << 2,      // 右キー
        Left = 1 << 3,       // 左キー
        Up = 1 << 4,         // 上キー
        Down = 1 << 5,       // 下キー
    }
}
