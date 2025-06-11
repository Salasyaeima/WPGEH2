using UnityEngine;
using UnityEngine.UI;

public class ButtonViaScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void addListenerButton(string GoToScene)
    {
        Button tombol = GetComponent<Button>();
        LoadingScreen loadingScreen = GameObject.Find("LoadingScreenManager").GetComponent<LoadingScreen>();

        tombol.onClick.AddListener(() => loadingScreen.SwitchToScene(GoToScene));
    }
}
