using UnityEngine;
using UnityEngine.SceneManagement;

public class CaughtHandler : MonoBehaviour
{
    public void PerformCaught()
    {
        Cursor.lockState = CursorLockMode.None;
        TaskManager.Instance.panelResult.SetActive(true);
        Timer.Instance.CompleteGame();
    }
}
