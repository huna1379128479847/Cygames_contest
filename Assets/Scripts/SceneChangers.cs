using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SearchService;

namespace Contest
{
    public abstract class SceneChanger : ISceneChanger
    {
        protected string fromSceneName;
        protected string toSceneName;
        public string FromSceneName { get { return fromSceneName; } }
        public string ToSceneName { get { return toSceneName; } }

        /// <param name="sceneName">遷移先のシーン名</param>
        public abstract void Execute(string sceneName);

        /// <summary>
        /// シーンへ移行します。
        /// </summary>
        public virtual void EnterScene()
        {
            // シーンが読み込めるかチェック
            if (SceneManager.GetSceneByName(toSceneName) != null)
            {
                SceneManager.LoadScene(toSceneName);
            }
            else
            {
                // シーンが存在しない場合の処理
                Debug.LogError($"シーン '{toSceneName}' が見つかりません。");
            }
        }

        /// <summary>
        /// 現在のシーンから退出し、元のシーンへ移動します
        /// </summary>
        public virtual void ExitScene()
        {
            if (SceneManager.GetSceneByName(fromSceneName) != null) 
            {
                SceneManager.LoadScene(fromSceneName);
            }
            else
            {
                // シーンが存在しない場合の処理
                Debug.LogError($"シーン '{fromSceneName}' が見つかりません。");
            }
        }
    }

    /// <summary>
    /// 戦闘シーンへの突入または退出を管理する
    /// </summary>
    public class MovingBetweenBattleScene : SceneChanger
    {
        public override void Execute(string sceneName)
        {
            fromSceneName = SceneManager.GetActiveScene().ToString();
            toSceneName = sceneName;
            EnterScene();
        }
    }
}
