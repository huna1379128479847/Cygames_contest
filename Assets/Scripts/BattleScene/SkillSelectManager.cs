using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Contest
{
    public class SkillSelectManager : SingletonBehavior<SkillSelectManager>, IManager
    {
        //定義部
        private Skill currentSkill;
        private List<UnitBase> selectUnits = new List<UnitBase>();
        private int selectCount;
        private bool forceSelect;
        public delegate bool SelectedUnitFilter(UnitBase unit);
        public event SelectedUnitFilter Filter;
        private bool isRunning;
        //定数
        private const int InvalidSelectionCount = -100;
        //プロパティ
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
        public bool InSelecting { get; private set; }

        public int SelectCount
        {
            get
            {
                if (currentSkill == null) return -100;

                switch (currentSkill.skillData.Pattern)
                {
                    case TargetingPattern.Solo: return 1;
                    case TargetingPattern.Duo: return 2;
                    case TargetingPattern.Trio: return 3;
                    case TargetingPattern.All:
                        return (FLG.FLGCheck((uint)currentSkill.parent.parent.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI))) ?
                               BattleSceneManager.instance.Enemy : BattleSceneManager.instance.Friend;
                    default: return InvalidSelectionCount;
                }
            }
        }
        //処理部
        // Awake メソッドで初期化
        protected override void Awake()
        {
            base.Awake();
            // 必要に応じて他の初期化処理を追加
        }

        /// <summary>
        /// ターゲット選択の実行
        /// </summary>
        /// <param name="skill">実行するスキル</param>
        public void Init(Skill skill)
        {
            currentSkill = skill;
            selectCount = SelectCount;
            ResetList();
            Filter = null;
            IsRunning = true;
            InSelecting = true;
            // 必要に応じてUIを表示
            SelectionUI.instance.Show();
        }

        /// <summary>
        /// ターゲット選択の終了
        /// </summary>
        public void Exit()
        {
            if (InSelecting && selectUnits != null)
            {
                currentSkill.InvokeSkill(selectUnits);
                currentSkill = null;
                IsRunning = false;
                InSelecting = false;
                ResetList();
            }
            // 必要に応じてUIを非表示
            SelectionUI.instance.Hide();
        }

        /// <summary>
        /// 選択フィルター
        /// </summary>
        public void MakingFilter()
        {
            TargetingPattern pattern = currentSkill.skillData.Pattern;
            UnitBase unit = currentSkill.parent.parent;
            {
                if (FLG.FLGCheck((uint)pattern, (uint)TargetingPattern.Friend))
                {
                    if (FLG.FLGCheck((uint)unit.MyUnitType, (uint)UnitType.Friend | (uint)UnitType.FriendAI))
                    {
                        Filter += Target.IsEnemy;
                    }
                    else
                    {
                        Filter += Target.IsFriend;
                    }
                }
            }
        }

        private void Update()
        {
            if (!IsRunning) { return; }
            if (Input.GetMouseButtonDown(0) && currentSkill != null && selectUnits.Count < selectCount)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                UnitBase unit;
                if (Physics.Raycast(ray, out hit) && hit.collider.TryGetComponent<UnitBase>(out unit))
                {
                    if (Filter?.Invoke(unit) == true)
                    {
                        if (!selectUnits.Contains(unit))
                        {
                            selectUnits.Add(unit);
                            //unit.SetSelected(true); // ビジュアルフィードバック
                            ParticleManager.MakeParticle(ParticleType.HighLight, AnimationOptions.Fitting, unit.gameObject);
                        }
                        else
                        {
                            selectUnits.Remove(unit);
                            //unit.SetSelected(false); // ビジュアルフィードバック
                            ParticleManager.DeleteParticle(unit.gameObject);
                        }
                    }
                }
            }

            // キャンセル操作（右クリックまたはエスケープキー）
            if (IsRunning && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
            {
                CancelSelection();
            }
        }

        /// <summary>
        /// 選択完了の処理
        /// </summary>
        public void CompleteSelection()
        {
            IsRunning = false;
            InSelecting = false;

            // コールバックを通じて選択されたユニットをスキルに返す

            // 必要に応じてクリーンアップ
            ResetList();
            currentSkill = null;

            // 必要に応じてUIを非表示
            SelectionUI.instance.Hide();
        }

        /// <summary>
        /// 選択のキャンセル処理
        /// </summary>
        public void CancelSelection()
        {
            ResetList();
            if (forceSelect) { return; }
            IsRunning = false;
            InSelecting = false;
            currentSkill = null;

            // キャンセル時のフィードバックをここに追加

            // 必要に応じてUIを非表示
            SelectionUI.instance.Hide();
        }

        public void ResetList()
        {
            foreach (UnitBase unit in selectUnits)
            {
                ParticleManager.DeleteParticle(unit.gameObject);
            }
            selectUnits.Clear();
        }
    }
}