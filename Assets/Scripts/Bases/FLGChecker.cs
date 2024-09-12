using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public static class FLGChecker
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
    }
}