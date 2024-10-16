using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Contest
{
    /// <summary>
    /// UIオブジェクトにアタッチするクラス。スキルの表示と選択を管理します。
    /// </summary>
    public class SkillObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private Image skillImage;
        [SerializeField] private Vector2 moveAmount;

        private Vector2 initialPosition;
        private Skill skill;

        /// <summary>
        /// スキルが選択されているかどうかを示します。
        /// </summary>
        public bool IsSelected { get; private set; } = false;

        /// <summary>
        /// 初期位置を取得します。初回のみ現在の位置を保存します。
        /// </summary>
        public Vector2 InitialPosition
        {
            get
            {
                if (initialPosition == Vector2.zero)
                {
                    RectTransform rectTransform = GetComponent<RectTransform>();
                    initialPosition = rectTransform.anchoredPosition;
                }
                return initialPosition;
            }
        }

        /// <summary>
        /// スキルをセットし、UIを更新します。
        /// </summary>
        /// <param name="skill">設定するスキル。</param>
        public void SetSkill(Skill skill)
        {
            this.skill = skill ?? throw new ArgumentNullException(nameof(skill), "Skill cannot be null.");

            if (skill.skillData != null)
            {
                skillNameText.SetText(skill.skillData.Name);
            }
            else
            {
                Debug.LogError("SkillData is missing in the provided skill.");
            }

            UpdateSkillAvailabilityUI();
        }

        /// <summary>
        /// 使用可能なスキルを返します。使用不可の場合はnullを返します。
        /// </summary>
        /// <returns>使用可能なスキル、またはnull。</returns>
        public Skill GetSkill()
        {
            return CanUseSkill() ? skill : null;
        }

        /// <summary>
        /// スキルが使用可能かどうかを判定します。
        /// </summary>
        /// <returns>使用可能ならtrue、それ以外はfalse。</returns>
        public bool CanUseSkill()
        {
            return skill != null && skill.CanUse;
        }

        /// <summary>
        /// スキルの使用可否に応じてUIの色とインタラクティブ性を設定します。
        /// </summary>
        private void UpdateSkillAvailabilityUI()
        {
            if (!CanUseSkill())
            {
                SetSkillUIState(Color.gray, false);
            }
            else
            {
                SetSkillUIState(Color.white, true);
            }
        }

        /// <summary>
        /// スキルが選択された際にUIを更新します。
        /// </summary>
        public void SelectSkill()
        {
            if (!CanUseSkill())
                return;

            IsSelected = true;
            MoveToSelectedPosition(true);
            SetSkillUIState(Color.green, true);
        }

        /// <summary>
        /// スキルの選択を解除し、UIを更新します。
        /// </summary>
        public void DeselectSkill()
        {
            IsSelected = false;
            MoveToSelectedPosition(false);
            UpdateSkillAvailabilityUI();
        }

        /// <summary>
        /// UIの色とインタラクティブ性を設定します。
        /// </summary>
        /// <param name="color">設定する色。</param>
        /// <param name="interactable">インタラクティブにするかどうか。</param>
        private void SetSkillUIState(Color color, bool interactable)
        {
            skillImage.color = color;
            skillImage.raycastTarget = interactable;
        }

        /// <summary>
        /// スキルが選択されたとき、UIオブジェクトを移動します。
        /// </summary>
        /// <param name="isSelected">選択状態かどうか。</param>
        public void MoveToSelectedPosition(bool isSelected)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = isSelected ? InitialPosition + moveAmount : InitialPosition;
        }

        /// <summary>
        /// スキルがクリックされた際に呼び出されるメソッド。外部から呼び出すことを想定。
        /// </summary>
        public void OnSkillClicked()
        {
            if (IsSelected)
            {
                DeselectSkill();
            }
            else
            {
                SelectSkill();
            }
        }

        /// <summary>
        /// スキルの情報をリセットし、UIを初期状態に戻します。
        /// </summary>
        public void ResetSkill()
        {
            skill = null;
            skillNameText.SetText(string.Empty);
            SetSkillUIState(Color.white, false);
            DeselectSkill();
        }
    }
}
