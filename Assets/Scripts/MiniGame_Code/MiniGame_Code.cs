using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MiniGame_Code : MonoBehaviour
{
    public TMP_InputField inputText; // TMP_InputField への参照
    public string SceneName;
    public TextMeshProUGUI SampleText;
    private string AnswerText;
    public GameObject SuccessIndicator;
    public GameObject FailureIndication;
    public Transform TimeBar;
    public float duration = 5.0f;
    private Vector3 initialScale;
    public float countdownTime = 5f; // カウントダウンの時間（秒）
    private float currentTime;
    public TextMeshProUGUI timerText; // TextMeshProUGUIコンポーネントへの参照
    private bool isCountingDown = true; // カウントダウン中かどうかのフラグ

    void Start()
    {
        // 初期設定
        SuccessIndicator.SetActive(false);
        FailureIndication.SetActive(false);
        currentTime = countdownTime; // 現在の時間を初期化
        UpdateTimerText(); // テキストを更新

        // TMP_InputFieldが設定されているか確認
        if (inputText != null)
        {
            inputText.onValueChanged.AddListener(OnInputTextChanged);
        }
        else
        {
            Debug.LogWarning("TMP_InputField が空白");
        }

        initialScale = TimeBar.localScale;
        StartCoroutine(ScaleXToZero());

        currentTime = countdownTime; // 現在の時間を初期化
    }

    void Update()
    {
        // カウントダウンがアクティブで、時間が0以上の場合に進める
        if (isCountingDown && currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 経過時間を引く
            UpdateTimerText(); // テキストを更新
        }

        // Enterキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isCountingDown = false; // カウントダウンをストップ
            Debug.Log("Enterが押された。 " + "入力された文字 " + AnswerText);

            if (SampleText.text == AnswerText)
            {
                SuccessIndicator.SetActive(true);
                // 成功時の処理
                StartCoroutine(WaitAndUnloadScene());
            }
            else
            {
                FailureIndication.SetActive(true);
                // 失敗時の処理
                StartCoroutine(WaitAndUnloadScene());
            }
        }

        // 時間が0になった場合、カウントダウンを止める
        if (currentTime <= 0 && isCountingDown)
        {
            currentTime = 0; // 0に設定
            UpdateTimerText(); // テキストを更新
            isCountingDown = false; // カウントダウンをストップ

                FailureIndication.SetActive(true);
                // 失敗時の処理
                StartCoroutine(WaitAndUnloadScene());
        }
    }

    void OnInputTextChanged(string text)
    {
        AnswerText = text; // 入力された文字を常時代入
    }

    private IEnumerator WaitAndUnloadScene() // シーンの待機とアンロード
    {
        // 3秒間待機
        yield return new WaitForSeconds(3);

        // 待機後にシーンをアンロード
        SceneManager.UnloadSceneAsync(SceneName);
        Debug.Log("シーンをアンロードしました。");
    }

    IEnumerator ScaleXToZero()
    {
        // 経過時間をdurationで初期化
        float elapsed = duration;

        // スケールが0になるまでの間、徐々に縮小
        while (elapsed > 0)
        {
            // 経過時間を減算
            elapsed -= Time.deltaTime;
            // 徐々にXスケールを0にする
            float newXScale = Mathf.Lerp(0, initialScale.x, elapsed / duration);
            TimeBar.localScale = new Vector3(newXScale, initialScale.y, initialScale.z);
            // 次のフレームまで待機
            yield return null;
        }

        // 最後に正確にXスケールを0に設定
        TimeBar.localScale = new Vector3(0, initialScale.y, initialScale.z);
    }

    void UpdateTimerText()
    {
        // 残り時間を秒単位で表示
        timerText.text = currentTime.ToString("F2"); // 小数点を切り上げ
        // timerText.text = Mathf.Ceil(currentTime).ToString(); // 小数点を切り上げ

    }
}
