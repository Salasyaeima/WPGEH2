using UnityEngine;
using UnityEngine.SceneManagement;

public class CaughtHandler : MonoBehaviour
{
    public void PerformCaught()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
