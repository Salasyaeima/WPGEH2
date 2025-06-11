using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button restartButton;
    public Button menuButton;
    public Button playMenu;

    void Start()
    {
        // Pastikan LoadingScreen.Instance tersedia
        if (LoadingScreen.Instance != null)
        {
            if (restartButton != null && menuButton != null)
            {
                restartButton.onClick.AddListener(() => LoadingScreen.Instance.SwitchToScene("Rooms"));
                menuButton.onClick.AddListener(() => LoadingScreen.Instance.SwitchToScene("MainMenu"));
            }
            if (playMenu != null)
            {
                playMenu.onClick.AddListener(() => LoadingScreen.Instance.SwitchToScene("CutScene"));
            }
            
        }
        else
        {
            Debug.LogError("LoadingScreen instance not found! Ensure LoadingScreen is initialized in the scene.");
        }
    }
}
