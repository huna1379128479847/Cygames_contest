using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public static class SelectingTarget
    {
        [Flags]
        public enum TargetingPateren
        {
            None    = 0,        //
            Friend  = 1 << 0,   // 味方
            Enemy   = 1 << 1,   // 敵
            Solo    = 1 << 2,   // 単体
            Duo     = 1 << 3,   // 二体まで
            Trio    = 1 << 4,   // 三体まで
            All     = 1 << 5,   // 全て
            Random  = 1 << 6,   // ランダム
            Select  = 1 << 7,   // 選ぶ
        }
        public static IUniqueThing SelectingTargets(TargetingPateren pateren)
        {
            if ()
        }
    }
}
