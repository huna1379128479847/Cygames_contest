using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// シーンの遷移を管理する抽象クラス。
    /// 各シーン間の移行を実装するために、このクラスを継承して具体的な処理を定義する。
    /// </summary>
    public abstract class SceneChanger : ISceneChanger
    {
        // シーン名を保持するフィールド
        protected string fromSceneName;
        protected string toSceneName;

        // プロパティでシーン名を取得
        public string FromSceneName { get { return fromSceneName; } }
        public string ToSceneName { get { return toSceneName; } }

        /// <summary>
        /// シーンを指定して実行する抽象メソッド。具体的なシーン遷移のロジックは派生クラスで定義。
        /// </summary>
        /// <param name="sceneName">遷移先のシーン名</param>
        public abstract void Execute(string sceneName);

        /// <summary>
        /// 指定したシーンへ移行する。
        /// シーンが存在しない場合はエラーメッセージを表示。
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
        /// 現在のシーンから退出し、元のシーンへ移動する。
        /// シーンが存在しない場合はエラーメッセージを表示。
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
    /// 戦闘シーンへの移行や元のシーンへの移動を管理する具体的なシーンチェンジャー。
    /// 戦闘シーンに移行する際のロジックを実装。
    /// </summary>
    public class MovingBetweenBattleScene : SceneChanger
    {
        /// <summary>
        /// 現在のシーンを記憶し、指定された戦闘シーンへ移行する。
        /// </summary>
        /// <param name="sceneName">移行先のシーン名。</param>
        public override void Execute(string sceneName)
        {
            fromSceneName = SceneManager.GetActiveScene().ToString();
            toSceneName = sceneName;
            EnterScene(); // 戦闘シーンに移行
        }
    }
}
