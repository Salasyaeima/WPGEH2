using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button restartButton;
    public Button menuButton;

    void Start()
    {
        // Pastikan LoadingScreen.Instance tersedia
        if (LoadingScreen.Instance != null)
        {
            restartButton.onClick.AddListener(() => LoadingScreen.Instance.SwitchToScene("Rooms"));
            menuButton.onClick.AddListener(() => LoadingScreen.Instance.SwitchToScene("MainMenu"));
        }
        else
        {
            Debug.LogError("LoadingScreen instance not found! Ensure LoadingScreen is initialized in the scene.");
        }
    }
}
