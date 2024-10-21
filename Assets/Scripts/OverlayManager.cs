using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayManager : MonoBehaviour
{
    public string overlaySceneName = "OverlayScene";

    // ボタンが押されたときに呼ばれるメソッド
    public void ShowOverlayScene()
    {
        SceneManager.LoadScene(overlaySceneName, LoadSceneMode.Additive);
    }

    // オーバーレイシーンを非表示にするメソッド
    public void HideOverlayScene()
    {
        SceneManager.UnloadSceneAsync(overlaySceneName);
    }
}
