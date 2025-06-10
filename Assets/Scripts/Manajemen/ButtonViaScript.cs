using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonViaScript : MonoBehaviour
{
    [SerializeField] string GoToScene;
    Button tombol;
    LoadingScreen loadingScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start dipanggil!");
        StartCoroutine(insertListener());
    }

    IEnumerator insertListener()
    {
        yield return new WaitForSeconds(1.5f);
        tombol = GetComponent<Button>();
        if (tombol == null)
        {
            Debug.LogError("Tombol tidak ditemukan di GameObject ini!");
            yield break;
        }

        GameObject loadingScreenObj = GameObject.Find("LoadingScreenManager");
        if (loadingScreenObj == null)
        {
            Debug.LogError("LoadingScreenManager tidak ditemukan!");
            yield break;
        }

        loadingScreen = loadingScreenObj.GetComponent<LoadingScreen>();
        if (loadingScreen == null)
        {
            Debug.LogError("Komponen LoadingScreen tidak ditemukan di LoadingScreenManager!");
            yield break;
        }

        tombol.onClick.AddListener(() => loadingScreen.SwitchToScene(GoToScene));
        Debug.Log("Listener berhasil ditambahkan!");
    }
}
