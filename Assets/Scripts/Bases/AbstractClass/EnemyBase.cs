using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Contest
{
    /// <summary>
    /// 敵ユニットの基本動作を定義するクラス。
    /// ビヘイビアパターンに基づいて行動を制御します。
    /// </summary>
    public class EnemyBase : UnitBase, IEnemy
    {
        // ビヘイビアパターンを保持するフィールド
        [SerializeField]
        private BehaviorPattern _behaviorPattern;

        /// <summary>
        /// ユニットのビヘイビアパターンを取得します。
        /// </summary>
        public BehaviorPattern BehaviorPattern => _behaviorPattern;

        /// <summary>
        /// ユニットの初期化処理を行います。
        /// </summary>
        public override void InitUnit(BattleSceneManager battleSceneManager)
        {
            base.InitUnit(battleSceneManager);
            if (Helpers.TryChangeType(UnitData, out Enemy enemy))
            {
                _behaviorPattern = enemy.EnemyPattern;
            }
            else
            {
                Debug.LogError($"{Name}の型が不正です。: {gameObject.name}");
            }
        }

        /// <summary>
        /// ユニットのターン中の行動を定義するコルーチン。
        /// ビヘイビアパターンに応じた行動を実行します。
        /// </summary>
        /// <returns>コルーチンの列挙子。</returns>
        public override IEnumerator TurnBehavior()
        {
            switch (_behaviorPattern)
            {
                case BehaviorPattern.Aggressive:
                    yield return StartCoroutine(ExecuteAggressiveBehavior(0.5f, 0.3f, 0.2f));
                    break;
                case BehaviorPattern.Defensive:
                    yield return StartCoroutine(ExecuteDefensiveBehavior(0.2f, 0.45f, 0.35f));
                    break;
                case BehaviorPattern.Support:
                    yield return StartCoroutine(ExecuteSupportBehavior(0.25f, 0.3f, 0.45f));
                    break;
                default:
                    Debug.LogWarning($"未知のビヘイビアパターン: {_behaviorPattern}");
                    break;
            }
            yield return null;
        }

        /// <summary>
        /// 攻撃的なビヘイビアを実行するコルーチン。
        /// </summary>
        /// <param name="aggressive">攻撃的行動の確率。</param>
        /// <param name="defensive">防御的行動の確率。</param>
        /// <param name="support">サポート行動の確率。</param>
        /// <returns>コルーチンの列挙子。</returns>
        private IEnumerator ExecuteAggressiveBehavior(float aggressive, float defensive, float support)
        {
            float r = UnityEngine.Random.value;
            if (r <= aggressive)
            {
                // 攻撃行動を実装
                Debug.Log($"{Name}は攻撃的な行動を選択しました。");
                // 例: 攻撃スキルの実行
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Attack));
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Attack));
            }
            else if (r <= aggressive + defensive)
            {
                // 防御行動を実装
                Debug.Log($"{Name}は防御的な行動を選択しました。");
                // 例: 防御スキルの実行
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Defense));
            }
            else
            {
                // サポート行動を実装
                Debug.Log($"{Name}はサポート行動を選択しました。");
                // 例: 回復スキルの実行
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Support));
            }
        }

        /// <summary>
        /// 防御的なビヘイビアを実行するコルーチン。
        /// </summary>
        /// <param name="aggressive">攻撃的行動の確率。</param>
        /// <param name="defensive">防御的行動の確率。</param>
        /// <param name="support">サポート行動の確率。</param>
        /// <returns>コルーチンの列挙子。</returns>
        private IEnumerator ExecuteDefensiveBehavior(float aggressive, float defensive, float support)
        {
            float r = UnityEngine.Random.value;
            if (r <= aggressive)
            {
                // 攻撃行動を実装
                Debug.Log($"{Name}は攻撃的な行動を選択しました。");
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Attack));
            }
            else if (r <= aggressive + defensive)
            {
                // 防御行動を実装
                Debug.Log($"{Name}は防御的な行動を選択しました。");
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Defense));
            }
            else
            {
                // サポート行動を実装
                Debug.Log($"{Name}はサポート行動を選択しました。");
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Support));
            }
        }

        /// <summary>
        /// サポート的なビヘイビアを実行するコルーチン。
        /// </summary>
        /// <param name="aggressive">攻撃的行動の確率。</param>
        /// <param name="defensive">防御的行動の確率。</param>
        /// <param name="support">サポート行動の確率。</param>
        /// <returns>コルーチンの列挙子。</returns>
        private IEnumerator ExecuteSupportBehavior(float aggressive, float defensive, float support)
        {
            float r = UnityEngine.Random.value;
            if (r <= aggressive)
            {
                // 攻撃行動を実装
                Debug.Log($"{Name}は攻撃的な行動を選択しました。");
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Attack));
            }
            else if (r <= aggressive + defensive)
            {
                // 防御行動を実装
                Debug.Log($"{Name}は防御的な行動を選択しました。");
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Defense));
            }
            else
            {
                // サポート行動を実装
                Debug.Log($"{Name}はサポート行動を選択しました。");
                yield return StartCoroutine(UseSkillCoroutine(SkillTypes.Support));
            }
        }

        /// <summary>
        /// 指定されたスキルを使用するコルーチン。
        /// </summary>
        /// <param name="skillName">使用するスキルの名前。</param>
        /// <returns>コルーチンの列挙子。</returns>
        private IEnumerator UseSkill(Skill skill)
        {
            SkillSelectManager.instance.Init(skill, false, true);
            SkillHandler.currentSkill = skill;
            // スキルの実行
            yield return new WaitWhile(() => SkillSelectManager.instance.InSelecting || skill.InAction);

            // 必要に応じて追加の処理をここに実装
        }

        /// <summary>
        /// スキルを使用する際のヘルパーメソッド。
        /// </summary>
        /// <param name="skillName">使用するスキルの名前。</param>
        /// <returns>スキルの実行コルーチン。</returns>
        protected virtual IEnumerator UseSkillCoroutine(SkillTypes skillTypes)
        {
            
            List<Skill> skills = SkillHandler.skills.Values.ToList();
            List<Skill> canUseSkills = new List<Skill>();
            foreach (Skill skill in skills)
            {
                if (FLG.FLGCheckHaving((uint)skill.skillData.SkillTypes, (uint)skillTypes) && skill.CanUse)
                {
                    canUseSkills.Add(skill);
                }
            }
            if (canUseSkills.Count < 0)
            {
                Skill skill = Helpers.RandomPick(skills);
                Debug.Log(skill.skillData.Name);
                return UseSkill(skill);
            }
            return UseSkill(Helpers.RandomPick(canUseSkills));
        }
    }
}
