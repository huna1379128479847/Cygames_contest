using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Contest
{
    /// <summary>
    /// プレイヤーのスキル選択を管理するクラス。
    /// </summary>
    public class SettingPlayerSkillSelection : SingletonBehavior<SettingPlayerSkillSelection>
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform frameParent;

        private Skill selectedSkill;
        private readonly List<SkillObject> skillObjects = new List<SkillObject>();

        public delegate void SkillSelectedHandler(Skill skill);
        public event SkillSelectedHandler OnSkillSelected;

        private SkillObject currentSkillObject;

        [SerializeField]public bool IsSelecting { get; private set; }

        /// <summary>
        /// スキル選択を開始します。
        /// </summary>
        /// <param name="skills">選択可能なスキルのリスト。</param>
        public void StartSkillSelection(List<Skill> skills)
        {
            if (IsSelecting)
            {
                Debug.LogWarning("既にスキル選択中です。");
                return;
            }
            Debug.Log("スキル選択開始");
            IsSelecting = true;
            selectedSkill = null;
            ClearExistingSkills();
            CreateSkillObjects(skills);
        }

        /// <summary>
        /// スキル選択をキャンセルします。
        /// </summary>
        public void CancelSelection()
        {
            if (currentSkillObject != null)
            {
                currentSkillObject.DeselectSkill();
                currentSkillObject = null;
            }
            selectedSkill = null;
            IsSelecting = SkillSelectManager.instance.InSelecting;
        }

        /// <summary>
        /// スキル選択を完了し、選択されたスキルを通知します。
        /// </summary>
        public void CompleteSelection()
        {
            if (selectedSkill != null && IsSelecting)
            {
                OnSkillSelected?.Invoke(selectedSkill);
                Debug.Log("スキル選択成功！ skillName:" + selectedSkill.skillData.Name);
                IsSelecting = false;
                selectedSkill = null;
            }
            else if (selectedSkill == null && IsSelecting)
            {
                Debug.LogWarning("選択されたスキルがありません。");
            }
        }

        private void CreateSkillObjects(List<Skill> skills)
        {
            if (skills == null || skills.Count == 0)
            {
                Debug.LogWarning("スキルリストが空です。");
                IsSelecting = false;
                return;
            }

            foreach (Skill skill in skills)
            {
                if (skill == null)
                {
                    Debug.LogWarning("nullのスキルがリストに含まれています。");
                    continue;
                }

                GameObject cardInstance = Instantiate(cardPrefab, frameParent);
                if (cardInstance.TryGetComponent(out SkillObject skillObject))
                {
                    skillObject.SetSkill(skill);
                    skillObjects.Add(skillObject);
                }
                else
                {
                    Debug.LogError("SkillObjectコンポーネントがアタッチされていません。");
                    Destroy(cardInstance);
                }
            }
        }

        private void HandleSkillClicked(SkillObject skillObject)
        {
            if (currentSkillObject != null && currentSkillObject != skillObject)
            {
                currentSkillObject.DeselectSkill();
            }

            if (!skillObject.IsSelected)
            {
                skillObject.SelectSkill();
                selectedSkill = skillObject.GetSkill();
                currentSkillObject = skillObject;
            }
            else
            {
                skillObject.DeselectSkill();
                selectedSkill = null;
                currentSkillObject = null;
            }
        }

        private void ClearExistingSkills()
        {
            foreach (var skillObject in skillObjects)
            {
                Destroy(skillObject.gameObject);
            }
            skillObjects.Clear();
            currentSkillObject = null;
            selectedSkill = null;
        }

        private void Update()
        {
            if (!IsSelecting) return;

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> rayResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, rayResults);

            foreach (var rayResult in rayResults)
            {
                if (rayResult.gameObject.TryGetComponent(out SkillObject skill))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        HandleSkillClicked(skill);
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            OnSkillSelected = null;
        }
    }
}
