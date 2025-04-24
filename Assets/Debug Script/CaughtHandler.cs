using UnityEngine;
using UnityEngine.SceneManagement;

public class CaughtHandler : MonoBehaviour
{
    public void PerformCaught()
    {
        LoadingScreen.Instance.SwitchToScene("Rooms");
    }
}
