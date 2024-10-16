using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// プレイヤーキャラクターを管理するクラス。
    /// プレイヤーのターンでの挙動を制御します。
    /// </summary>
    public class Player : UnitBase, IPlayerHandler
    {
        /// <summary>
        /// プレイヤーのターン中の挙動を実行します。
        /// </summary>
        /// <returns>コルーチンの列挙子。</returns>
        public override IEnumerator TurnBehavior()
        {
            yield return StartCoroutine(GeneratePlayerOptions());
        }

        /// <summary>
        /// プレイヤーにスキル選択オプションを生成し、選択を待機します。
        /// </summary>
        /// <returns>コルーチンの列挙子。</returns>
        public virtual IEnumerator GeneratePlayerOptions()
        {
            Debug.Log("プレイヤーのターン");

            // Nullチェック
            if (SettingPlayerSkillSelection.instance == null)
            {
                Debug.LogError("SettingPlayerSkillSelectionのインスタンスが見つかりません。");
                yield break;
            }

            // イベントハンドラーの登録
            SettingPlayerSkillSelection.instance.OnSkillSelected += HandlePlayerSelection;

            // スキルリストが存在するか確認
            var availableSkills = SkillHandler.skills?.Values.ToList();
            if (availableSkills == null || availableSkills.Count == 0)
            {
                Debug.LogWarning("利用可能なスキルが存在しません。");
                SettingPlayerSkillSelection.instance.OnSkillSelected -= HandlePlayerSelection;
                yield break;
            }

            // スキル選択UIを表示
            SettingPlayerSkillSelection.instance.StartSkillSelection(availableSkills);

            // スキル選択またはスキル選択マネージャーが選択中になるまで待機
            yield return new WaitUntil(() => SkillSelectManager.instance.InSelecting || SettingPlayerSkillSelection.instance.IsSelecting);
        }

        /// <summary>
        /// プレイヤーがスキルを選択した際に呼び出されるコールバック。
        /// </summary>
        /// <param name="skill">選択されたスキル。</param>
        public virtual void HandlePlayerSelection(Skill skill)
        {
            SkillHandler.currentSkill = skill;
            // イベントハンドラーの解除
            if (SettingPlayerSkillSelection.instance != null)
            {
                SettingPlayerSkillSelection.instance.OnSkillSelected -= HandlePlayerSelection;
            }

            // SkillSelectManagerが存在するか確認
            if (SkillSelectManager.instance == null)
            {
                Debug.LogError("SkillSelectManagerのインスタンスが見つかりません。");
                return;
            }

            // スキル選択マネージャーを初期化
            SkillSelectManager.instance.Init(skill);
        }

        /// <summary>
        /// プレイヤーオブジェクトが破棄される際に呼び出されます。
        /// イベントハンドラーの解除を行います。
        /// </summary>
        private void OnDestroy()
        {
            if (SettingPlayerSkillSelection.instance != null)
            {
                SettingPlayerSkillSelection.instance.OnSkillSelected -= HandlePlayerSelection;
            }
        }
    }
}
