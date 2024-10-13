using System.Collections.Generic;
using UnityEngine;

namespace Contest
{
    /// <summary>
    /// スキル選択およびターゲット選択を管理するクラス。
    /// ユニットをターゲットにスキルを発動する際のプロセスを制御し、スキルを適用する。
    /// </summary>
    public class SkillSelectManager : SingletonBehavior<SkillSelectManager>, IManager
    {
        // 現在選択されているスキル
        private Skill currentSkill;
        // 選択されたユニットのリスト
        private List<UnitBase> selectUnits = new List<UnitBase>();
        // 選択するユニットの数
        private int selectCount;
        // 選択を強制するかどうか
        private bool forceSelect;

        // ユニット選択時のフィルタ
        public delegate bool SelectedUnitFilter(UnitBase unit);
        public event SelectedUnitFilter Filter;

        // ユニット選択後のコールバック
        public delegate void CallBackSelection(List<UnitBase> unitBases);
        public event CallBackSelection CallBack;

        // 選択が実行中かどうかを管理
        private bool isRunning;

        // 定数
        private const int InvalidSelectionCount = -100;

        // プロパティ
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if (value != isRunning)
                {
                    isRunning = value;
                }
            }
        }

        // 現在選択中かどうか
        public bool InSelecting { get; private set; }

        // 選択できるユニットの数を返すプロパティ
        public int SelectCount
        {
            get
            {
                if (currentSkill == null) return InvalidSelectionCount;

                switch (currentSkill.skillData.Pattern)
                {
                    case TargetingPattern.Solo: return 1;
                    case TargetingPattern.Duo: return 2;
                    case TargetingPattern.Trio: return 3;
                    case TargetingPattern.All:
                        return (FLG.FLGCheck((uint)currentSkill.parent.Parent.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI))) ?
                               BattleSceneManager.instance.Enemy : BattleSceneManager.instance.Friend;
                    default: return InvalidSelectionCount;
                }
            }
        }

        // 初期化処理
        protected override void Awake()
        {
            base.Awake();
            // 必要に応じて他の初期化処理を追加
        }

        /// <summary>
        /// ターゲット選択の初期化。
        /// スキルに基づいて選択プロセスを開始する。
        /// </summary>
        /// <param name="skill">実行するスキル</param>
        public void Init(Skill skill, bool forceSelecting = false)
        {
            CallBack += skill.InvokeSkill;
            currentSkill = skill;
            selectCount = SelectCount;
            ResetList();
            Filter = null;
            forceSelect = forceSelecting;
            IsRunning = true;
            InSelecting = true;
            // 必要に応じてUIを表示
            SelectionUI.instance.Show();

            // フィルタを設定
            MakingFilter();
        }

        /// <summary>
        /// ターゲット選択の終了。
        /// 選択されたユニットに対してスキルを適用する。
        /// </summary>
        public void Exit()
        {
            if (InSelecting && selectUnits != null)
            {
                // スキルを発動する際に、選択されたユニットを渡す
                CallBack.Invoke(selectUnits);
                currentSkill = null;
                IsRunning = false;
                InSelecting = false;
                ResetList();
            }
            // 必要に応じてUIを非表示
            SelectionUI.instance.Hide();
        }

        /// <summary>
        /// ユニット選択時に適用するフィルターを作成。
        /// スキルのターゲットパターンに基づいて、選択可能なユニットをフィルタリング。
        /// </summary>
        public void MakingFilter()
        {
            TargetingPattern pattern = currentSkill.skillData.Pattern;
            UnitBase unit = currentSkill.parent.Parent;

            if (FLG.FLGCheck((uint)pattern, (uint)TargetingPattern.Friend))
            {
                if (FLG.FLGCheck((uint)unit.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI)))
                {
                    Filter += Target.IsEnemy;
                }
                else
                {
                    Filter += Target.IsFriend;
                }
            }
        }

        private void Update()
        {
            if (!IsRunning) { return; }

            // 左クリックでユニットを選択
            if (Input.GetMouseButtonDown(0) && currentSkill != null && selectUnits.Count < selectCount)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                UnitBase unit;
                if (Physics.Raycast(ray, out hit) && hit.collider.TryGetComponent<UnitBase>(out unit))
                {
                    if (Filter?.Invoke(unit) ?? true)
                    {
                        if (!selectUnits.Contains(unit))
                        {
                            selectUnits.Add(unit);
                            // 選択時の視覚エフェクトを追加
                            ParticleManager.instance.MakeParticle(ParticleType.HighLight, AnimationOptions.Fitting, unit.gameObject);
                        }
                        else
                        {
                            selectUnits.Remove(unit);
                            // 選択解除時の視覚エフェクトを削除
                            ParticleManager.instance.DeleteParticle(unit.gameObject, ParticleType.HighLight);
                        }
                    }
                }
            }

            // 右クリックまたはEscキーでキャンセル
            if (IsRunning && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
            {
                CancelSelection();
            }

            // 選択が完了した場合（例: 必要な数を選択したら自動で終了）
            if (selectUnits.Count == selectCount)
            {
                Exit();
            }
        }

        /// <summary>
        /// 選択完了処理。
        /// コールバックを通じて選択されたユニットにスキルを適用。
        /// </summary>
        public void CompleteSelection()
        {
            IsRunning = false;
            InSelecting = false;
            ResetList();
            currentSkill = null;
            // 必要に応じてUIを非表示
            SelectionUI.instance.Hide();
        }

        /// <summary>
        /// 選択のキャンセル処理。
        /// </summary>
        public void CancelSelection()
        {
            ResetList();
            if (forceSelect) { return; } // 何らかの条件によって
            IsRunning = false;
            InSelecting = false;
            currentSkill = null;
            // 必要に応じてUIを非表示
            SelectionUI.instance.Hide();
        }

        /// <summary>
        /// 選択されたユニットのリストをリセット。
        /// </summary>
        public void ResetList()
        {
            foreach (UnitBase unit in selectUnits)
            {
                ParticleManager.instance.DeleteParticle(unit.gameObject);
            }
            selectUnits.Clear();
        }
    }
}
