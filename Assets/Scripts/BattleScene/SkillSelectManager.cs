using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Contest
{
    /// <summary>
    /// スキル選択およびターゲット選択を管理するクラス。
    /// ユニットをターゲットにスキルを発動する際のプロセスを制御し、スキルを適用します。
    /// プレイヤーとAIの選択プロセスを区別して管理します。
    /// </summary>
    public class SkillSelectManager : SingletonBehavior<SkillSelectManager>, IManager
    {
        // 現在選択されているスキル
        private Skill _currentSkill;

        // 選択されたユニットのリスト
        private readonly List<UnitBase> _selectedUnits = new List<UnitBase>();

        // 選択するユニットの数
        [SerializeField] private int _selectCount;

        // 選択を強制するかどうか
        private bool _forceSelect;

        // ユニット選択時のフィルタ
        public delegate bool SelectedUnitFilter(UnitBase unit, Skill skill);
        public event SelectedUnitFilter OnUnitFilter;

        // ユニット選択後のコールバック
        public delegate void SelectionCallback(List<UnitBase> unitBases);
        public event SelectionCallback OnSelectionComplete;

        // 選択が実行中かどうかを管理
        private bool _isRunning;

        // 定数
        private const int InvalidSelectionCount = -100;

        /// <summary>
        /// 選択が実行中かどうかを示します。
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    // 必要に応じてイベントを追加
                }
            }
        }

        /// <summary>
        /// 現在選択中かどうかを示します。
        /// </summary>
        public bool InSelecting { get; private set; }

        /// <summary>
        /// 選択できるユニットの数を返します。
        /// </summary>
        public int SelectCount
        {
            get
            {
                if (_currentSkill == null) return InvalidSelectionCount;
                List<TargetingPattern> targets = FLG.FLGSeparator<TargetingPattern>((uint)_currentSkill.skillData.Pattern);
                if (targets.Count == 0) return InvalidSelectionCount;

                if (targets.Contains(TargetingPattern.Solo)) return 1;
                if (targets.Contains(TargetingPattern.Duo)) return 2;
                if (targets.Contains(TargetingPattern.Trio)) return 3;
                if (targets.Contains(TargetingPattern.All))
                {
                    if (FLG.FLGCheckHaving((uint)_currentSkill.parent.Parent.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI)))
                    {
                        return BattleSceneManager.instance.EnemyCount;
                    }
                    return BattleSceneManager.instance.FriendCount;
                }
                return InvalidSelectionCount;
            }
        }

        /// <summary>
        /// 初期化処理。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            // 必要に応じて他の初期化処理を追加
        }

        /// <summary>
        /// ターゲット選択の初期化。
        /// スキルに基づいて選択プロセスを開始します。
        /// プレイヤー選択かAI選択かを指定します。
        /// </summary>
        /// <param name="skill">実行するスキル。</param>
        /// <param name="forceSelecting">選択を強制するかどうか。</param>
        /// <param name="isAI">AIによる選択かどうか。</param>
        public void Init(Skill skill, bool forceSelecting = false, bool isAI = false)
        {
            OnSelectionComplete = null;
            if (skill == null)
            {
                Debug.LogError("Initに渡されたスキルがnullです。");
                return;
            }
            Debug.Log($"ターゲット選択　スキル名：{skill.skillData.Name} | AI選択: {isAI}");
            OnSelectionComplete += skill.InvokeSkill;

            _currentSkill = skill;
            _selectCount = SelectCount;
            ResetSelection();
            OnUnitFilter = null;
            _forceSelect = forceSelecting;
            IsRunning = true;
            InSelecting = true;
            if (FLG.FLGCheck((uint)_currentSkill.skillData.Pattern, (uint)TargetingPattern.Self))
            {
                _selectedUnits.Add(_currentSkill.parent.Parent);
                Exit();
            }
            // 必要に応じてUIを表示（プレイヤーの場合のみ）
            if (!isAI && !FLG.FLGCheckHaving((uint)_currentSkill.skillData.Pattern, (uint)TargetingPattern.Random))
            {
                SelectionUI.instance.Show();
            }

            // フィルタを設定
            CreateFilter();

            // AIの場合は自動選択を開始
            if (isAI)
            {
                StartCoroutine(AutoSelect());
            }
        }

        /// <summary>
        /// ターゲット選択の終了。
        /// 選択されたユニットに対してスキルを適用します。
        /// </summary>
        public void Exit()
        {
            if (InSelecting && _selectedUnits.Count > 0)
            {
                OnSelectionComplete?.Invoke(new List<UnitBase>(_selectedUnits));
                ResetSelection();
                _currentSkill = null;
                IsRunning = false;
                InSelecting = false;
            }
        }

        /// <summary>
        /// ユニット選択時に適用するフィルターを作成します。
        /// スキルのターゲットパターンに基づいて、選択可能なユニットをフィルタリングします。
        /// </summary>
        private void CreateFilter()
        {
            if (_currentSkill.skillData.Pattern == TargetingPattern.None)
            {
                return;
            }
            TargetingPattern pattern = _currentSkill.skillData.Pattern;
            UnitBase caster = _currentSkill.parent.Parent;

            if (FLG.FLGCheckHaving((uint)pattern, (uint)TargetingPattern.Friend))
            {
                if (FLG.FLGCheckHaving((uint)caster.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI)))
                {
                    OnUnitFilter += Target.IsEnemy;
                }
                else
                {
                    OnUnitFilter += Target.IsFriend;
                }
            }
            if (FLG.FLGCheckHaving((uint)pattern, (uint)TargetingPattern.Enemy))
            {
                if (!FLG.FLGCheckHaving((uint)caster.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI)))
                {
                    OnUnitFilter += Target.IsFriend;
                }
                else
                {
                    OnUnitFilter += Target.IsEnemy;
                }
            }
            if (FLG.FLGCheckHaving((uint)pattern, (uint)TargetingPattern.Self))
            {
                OnUnitFilter += Target.SelfTarget;
            }
            // 他のターゲットパターンに基づくフィルタリングを追加
        }

        /// <summary>
        /// 更新処理。ユーザーの入力に基づいてターゲット選択を制御します。
        /// プレイヤー選択時のみ処理を行います。
        /// </summary>
        private void Update()
        {
            if (!IsRunning || !InSelecting) return;

            // プレイヤー選択の場合のみ入力処理を行う
            if (!SkillSelectionIsAI())
            {
                // 左クリックでユニットを選択
                if (Input.GetMouseButtonDown(0) && _currentSkill != null)
                {
                    Debug.Log("クリック検出");
                    HandleUnitSelection();
                }

                // 右クリックまたはEscキーでキャンセル
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelSelection();
                }
            }
        }

        /// <summary>
        /// ユニットの選択処理を行います。
        /// プレイヤー選択の場合のみ呼び出されます。
        /// </summary>
        private void HandleUnitSelection()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> rayResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, rayResults);

            foreach (var raycast in rayResults)
            {
                if (raycast.gameObject.TryGetComponent(out UnitBase unit) && (OnUnitFilter?.Invoke(unit, _currentSkill) ?? true))
                {
                    Debug.Log(unit.Name);
                    if (!_selectedUnits.Contains(unit) && _selectedUnits.Count < _selectCount)
                    {
                        SelectUnit(unit);
                    }
                    else
                    {
                        DeselectUnit(unit);
                    }
                }
            }
        }

        /// <summary>
        /// ユニットを選択状態にします。
        /// </summary>
        /// <param name="unit">選択するユニット。</param>
        private void SelectUnit(UnitBase unit)
        {
            Debug.Log("選択");
            _selectedUnits.Add(unit);
            ParticleManager.instance.MakeParticle(ParticleType.Highlight, AnimationOptions.Fitting, unit.gameObject);
            // 追加の視覚エフェクトやロジックをここに追加
        }

        /// <summary>
        /// ユニットの選択を解除します。
        /// </summary>
        /// <param name="unit">選択解除するユニット。</param>
        private void DeselectUnit(UnitBase unit)
        {
            if (_selectedUnits.Remove(unit))
            {
                ParticleManager.instance.DeleteParticle(unit.gameObject, ParticleType.Highlight);
                // 追加の視覚エフェクトやロジックをここに追加
            }
        }

        /// <summary>
        /// 選択完了処理。
        /// コールバックを通じて選択されたユニットにスキルを適用します。
        /// プレイヤー選択またはAI選択に応じて処理を行います。
        /// </summary>
        public void CompleteSelection()
        {
            if (!IsRunning) return;

            Exit();
        }

        /// <summary>
        /// 選択のキャンセル処理。
        /// プレイヤー選択の場合のみ実行します。
        /// </summary>
        public void CancelSelection()
        {
            if (!IsRunning) return;

            ResetSelection();
            if (_forceSelect)
            {
                return;
            }
            _currentSkill = null;

            // スキル選択UIをリセットまたは非表示にします
            if (!SkillSelectionIsAI())
            {
                SettingPlayerSkillSelection.instance.CancelSelection();
            }
        }

        /// <summary>
        /// 選択されたユニットのリストをリセットします。
        /// </summary>
        private void ResetSelection()
        {
            foreach (UnitBase unit in _selectedUnits)
            {
                ParticleManager.instance.DeleteParticle(unit.gameObject, ParticleType.Highlight);
                // 追加の視覚エフェクトやロジックをここに追加
            }
            _selectedUnits.Clear();
        }

        /// <summary>
        /// リストをリセットします。
        /// </summary>
        public void ResetList()
        {
            ResetSelection();
        }

        /// <summary>
        /// AIによる自動選択を行うコルーチン。
        /// </summary>
        /// <returns>コルーチンの列挙子。</returns>
        private IEnumerator AutoSelect()
        {
            yield return new WaitForSeconds(0.5f); // AIの思考時間をシミュレート

            if (_currentSkill == null)
            {
                Debug.LogWarning("AIに利用可能なスキルが存在しません。");
                CancelSelection();
                yield break;
            }
            _selectCount = SelectCount;
            ResetSelection();
            IsRunning = true;
            InSelecting = true;

            // ターゲットをシミュレート
            List<UnitBase> targets = ChooseAITargets();
            foreach (var target in targets)
            {
                SelectUnit(target);
            }

            // 選択完了を通知
            Exit();
        }

        /// <summary>
        /// AIが選択するターゲットを決定します。
        /// </summary>
        /// <returns>選択されたユニットのリスト。</returns>
        private List<UnitBase> ChooseAITargets()
        {
            List<UnitBase> targets = new List<UnitBase>();
            var potentialTargets = BattleSceneManager.instance.AllUnits.Where(u => OnUnitFilter?.Invoke(u, _currentSkill) ?? true).ToList();

            if (_selectCount <= 0 || potentialTargets.Count == 0)
            {
                return targets;
            }

            // 例として、最もHPが低いユニットを選択
            targets = potentialTargets.OrderBy(u => u.StatusTracker.CurrentHP.CurrentAmount).Take(_selectCount).ToList();
            return targets;
        }

        /// <summary>
        /// 現在の選択がAIによるものかどうかを判定します。
        /// </summary>
        /// <returns>AIによる選択の場合はtrue、それ以外はfalse。</returns>
        private bool SkillSelectionIsAI()
        {
            // 実装方法によりますが、例えば現在選択中のスキルの所有者がAIかどうかをチェック
            return _currentSkill != null && FLG.FLGCheckHaving((uint)_currentSkill.parent.Parent.MyUnitType, (uint)(UnitType.FriendAI | UnitType.EnemyAI));
        }

        /// <summary>
        /// オブジェクトが破棄される際に呼び出されます。
        /// イベントハンドラーの解除を行います。
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnSelectionComplete = null;
            OnUnitFilter = null;
        }
    }
}
