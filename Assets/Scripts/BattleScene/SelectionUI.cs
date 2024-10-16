using UnityEngine;
using UnityEngine.UI;

namespace Contest
{
    public class SelectionUI : SingletonBehavior<SelectionUI>
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        override protected void Awake()
        {
            base.Awake();

            // ボタンのリスナーを設定
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);

        }

        /// <summary>
        /// UI を表示する
        /// </summary>
        public void Show()
        {
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// UI を非表示にする
        /// </summary>
        public void Hide()
        {
            confirmButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// 確定ボタンが押された時の処理
        /// </summary>
        private void OnConfirm()
        {
            SkillSelectManager.instance.CompleteSelection();
            SettingPlayerSkillSelection.instance.CompleteSelection();
        }

        /// <summary>
        /// キャンセルボタンが押された時の処理
        /// </summary>
        private void OnCancel()
        {
            SkillSelectManager.instance.CancelSelection();
            SettingPlayerSkillSelection.instance.CancelSelection();
        }
    }
}
