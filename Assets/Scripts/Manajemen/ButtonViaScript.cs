using UnityEngine;
using UnityEngine.UI;

public class ButtonViaScript : MonoBehaviour
{
    [SerializeField] string GoToScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button tombol = GetComponent<Button>();
        LoadingScreen loadingScreen = GameObject.Find("LoadingScreenManager").GetComponent<LoadingScreen>();

        tombol.onClick.AddListener(() => loadingScreen.SwitchToScene(GoToScene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
