using UnityEngine;
using UnityEngine.UI;

namespace Contest
{
    public class SelectionUI : SingletonBehavior<SelectionUI>
    {
        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        override protected void Awake()
        {
            base.Awake();

            // ボタンのリスナーを設定
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);

            Hide(); // 初期状態では非表示
        }

        /// <summary>
        /// UI を表示する
        /// </summary>
        public void Show()
        {
            selectionPanel.SetActive(true);
        }

        /// <summary>
        /// UI を非表示にする
        /// </summary>
        public void Hide()
        {
            selectionPanel.SetActive(false);
        }

        /// <summary>
        /// 確定ボタンが押された時の処理
        /// </summary>
        private void OnConfirm()
        {
            SkillSelectManager.instance.CompleteSelection();
            Hide();
        }

        /// <summary>
        /// キャンセルボタンが押された時の処理
        /// </summary>
        private void OnCancel()
        {
            SkillSelectManager.instance.CancelSelection();
            Hide();
        }
    }
}
