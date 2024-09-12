using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Contest
{
    /// <summary>
    /// 戦闘シーンへの突入または退出を管理する
    /// </summary>
    public static class MovingBetweenBattleScene
    {
        private static string fromSceneName;
        private static string toSceneName;  
        /// <summary>
        /// 戦闘シーンへ移行します。
        /// </summary>
        /// <param name="sceneName">遷移先のシーン名</param>
        public static void EnterBattleScene()
        {
            // シーンが読み込めるかチェック
            if (SceneManager.GetSceneByName(toSceneName) != null)
            {
                // 戦闘シーンへ遷移
                SceneManager.LoadScene(toSceneName);
            }
            else
            {
                // シーンが存在しない場合の処理
                Debug.LogError($"シーン '{toSceneName}' が見つかりません。");
            }
        }

        public static void Execute(string sceneName)
        {
            fromSceneName = SceneManager.GetActiveScene().ToString();
            toSceneName = sceneName;
            EnterBattleScene();
        }
    }
}
