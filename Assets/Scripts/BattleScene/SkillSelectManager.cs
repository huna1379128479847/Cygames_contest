using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Contest
{
    public class SkillSelectManager : SingletonBehavior<SkillSelectManager>, IManager
    {
        private Skill currentSkill;
        private List<UnitBase> selectUnits = new List<UnitBase>();
        private int selectCount;
        private bool forceSelect;
        private delegate bool SelectedUnitFilter(UnitBase unit);
        private event SelectedUnitFilter filter;
        private bool isRunning;
        public bool IsRunning
        {
            get
            {
                return isRunning && BattleSceneManager.instance.IsRunning;
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
                if (currentSkill != null)
                {
                    if (FLG.FLGCheck((uint)currentSkill.Target, (uint)TargetingPateren.Solo))
                    {
                        return 1;
                    }
                    else if (FLG.FLGCheck((uint)currentSkill.Target, (uint)TargetingPateren.Duo))
                    {
                        return 2;
                    }
                    else if (FLG.FLGCheck((uint)currentSkill.Target, (uint)TargetingPateren.Trio))
                    {
                        return 3;
                    }
                    else if (FLG.FLGCheck((uint)currentSkill.Target, (uint)TargetingPateren.All))
                    {
                        if (FLG.FLGCheck((uint)currentSkill.parent.parent.MyUnitType, (uint)(UnitType.Friend | UnitType.FriendAI)))
                        {
                            return BattleSceneManager.instance.Enemy;
                        }
                        else
                        {
                            return BattleSceneManager.instance.Friend;
                        }
                    }
                }
                return -100;
            }
        }
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
            selectUnits.Clear();
            filter = null;
            IsRunning = true;
            InSelecting = true;

            // 必要に応じてUIを表示
            SelectionUI.Instance.Show();
        }

        /// <summary>
        /// ターゲット選択の終了
        /// </summary>
        public void Exit()
        {
            if (InSelecting && selectUnits != null)
            {
                currentSkill.Notyfy_Selected(selectUnits);
                currentSkill = null;
                IsRunning = false;
                InSelecting = false;
                selectUnits.Clear();
            }
            // 必要に応じてUIを非表示
            SelectionUI.Instance.Hide();
        }

        /// <summary>
        /// 選択フィルター
        /// </summary>
        public void MakingFilter()
        {
            TargetingPateren pateren = currentSkill.Target;
            UnitBase unit = currentSkill.parent.parent;
            if (FLG.FLGCheck((uint)pateren, (uint)TargetingPateren.Friend))
            {
                if (FLG.FLGCheck((uint)unit.MyUnitType, (uint)UnitType.Friend | (uint)UnitType.FriendAI))
                {
                    filter += Target.IsEnemy;
                }
                else
                {
                    filter += Target.IsFriend;
                }
            }
        }

        private void Update()
        {
            if (IsRunning && Input.GetMouseButtonDown(0) && currentSkill != null && selectUnits.Count <= selectCount)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                UnitBase unit;
                if (Physics.Raycast(ray, out hit) && hit.collider.TryGetComponent<UnitBase>(out unit))
                {
                    if (filter.Invoke(unit))
                    {
                        if (!selectUnits.Contains(unit))
                        {
                            selectUnits.Add(unit);
                            //unit.SetSelected(true); // ビジュアルフィードバック
                            ParticleManager.MakeParticle(ParticleType.HighLight, ParticleOptions.Fitting, unit.gameObject);
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
            selectUnits.Clear();
            currentSkill = null;

            // 必要に応じてUIを非表示
            SelectionUI.Instance.Hide();
        }

        /// <summary>
        /// 選択のキャンセル処理
        /// </summary>
        public void CancelSelection()
        {
            selectUnits.Clear();
            if (forceSelect) { return; }
            IsRunning = false;
            InSelecting = false;
            currentSkill = null;

            // キャンセル時のフィードバックをここに追加

            // 必要に応じてUIを非表示
            SelectionUI.Instance.Hide();
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
