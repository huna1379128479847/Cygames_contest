using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Contest
{
    public static class FLG
    {
        /// <summary>
        /// 指定されたフラグ (checkFlg) が flg に含まれているかを確認
        /// 「(uint)変数名」とキャストする必要がある
        /// </summary>
        public static bool FLGCheck(uint flg, uint checkFlg)
        {
            // ビット演算でフラグが設定されているかを確認
            return (flg & checkFlg) == checkFlg;
        }

        public static bool FLGCheckHaving(uint flg, uint checkHaving)
        {
            return (flg & checkHaving) != 0;
        }
        public static uint FLGUp(uint flg, uint checkFlg)
        {
            if (checkFlg != 0)
            {
                return flg | checkFlg; // フラグを立てる (ビットを1にする)
            }
            return flg;
        }

        public static uint FLGDown(uint flg, uint checkFlg)
        {
            if (checkFlg != 0)
            {
                flg &= ~checkFlg; // フラグを下げる (ビットを0にする)
            }
            return flg;
        }

        public static uint FLGReverse(uint flg)
        {
            return ~flg;
        }
    }
}