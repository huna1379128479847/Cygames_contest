using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Contest
{
    /// <summary>
    /// フラグ管理用のユーティリティクラス。
    /// ビット演算を使ってフラグを確認、設定、解除、反転などを行う。
    /// </summary>
    public static class FLG
    {
        /// <summary>
        /// 指定されたフラグ (checkFlg) が flg に含まれているかを確認する。
        /// ビット演算で完全一致をチェック。
        /// </summary>
        /// <param name="flg">元のフラグ。</param>
        /// <param name="checkFlg">確認するフラグ。</param>
        /// <returns>全てのビットが一致していれば true、そうでなければ false。</returns>
        public static bool FLGCheck(uint flg, uint checkFlg)
        {
            // ビット演算でフラグが設定されているかを確認
            return (flg & checkFlg) == checkFlg;
        }

        /// <summary>
        /// 指定されたフラグ (checkHaving) が flg に含まれているかを部分的に確認する。
        /// 完全一致ではなく、いずれかのビットが立っていれば true。
        /// </summary>
        /// <param name="flg">元のフラグ。</param>
        /// <param name="checkHaving">確認するフラグ。</param>
        /// <returns>いずれかのビットが一致していれば true。</returns>
        public static bool FLGCheckHaving(uint flg, uint checkHaving)
        {
            return (flg & checkHaving) != 0;
        }

        /// <summary>
        /// フラグを立てる（ビットを1にする）。指定したフラグを追加。
        /// </summary>
        /// <param name="flg">元のフラグ。</param>
        /// <param name="checkFlg">追加するフラグ。</param>
        /// <returns>フラグが立てられた新しい値。</returns>
        public static uint FLGUp(uint flg, uint checkFlg)
        {
            if (checkFlg != 0)
            {
                return flg | checkFlg; // フラグを立てる (ビットを1にする)
            }
            return flg;
        }

        /// <summary>
        /// フラグを下げる（ビットを0にする）。指定したフラグを解除。
        /// </summary>
        /// <param name="flg">元のフラグ。</param>
        /// <param name="checkFlg">解除するフラグ。</param>
        /// <returns>フラグが下げられた新しい値。</returns>
        public static uint FLGDown(uint flg, uint checkFlg)
        {
            if (checkFlg != 0)
            {
                flg &= ~checkFlg; // フラグを下げる (ビットを0にする)
            }
            return flg;
        }

        /// <summary>
        /// フラグを反転させる（1→0、0→1）。
        /// </summary>
        /// <param name="flg">元のフラグ。</param>
        /// <returns>フラグが反転された新しい値。</returns>
        public static uint FLGReverse(uint flg)
        {
            return ~flg;
        }

        public static uint FLGToggle(uint flg, uint checkFlg)
        {
            return flg ^ checkFlg;  // 指定したフラグをトグルする
        }

        public static List<TEnum> FLGSeparator<TEnum>(uint flg) where TEnum : Enum
        {
            List<TEnum> enums = new List<TEnum>();
            int i = 0;

            // Enumのビットフラグに沿って処理
            while (flg != 0)
            {
                uint mask = (uint)(1 << i);  // iビット目のフラグ
                if (FLGCheck(flg, mask))
                {
                    enums.Add((TEnum)Enum.ToObject(typeof(TEnum), mask));
                    flg = FLGDown(flg, mask); // フラグを降ろす（bitをオフにする）
                }
                i++;
            }

            return enums;
        }

    }
}
