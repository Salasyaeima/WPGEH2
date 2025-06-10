using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    [SerializeField] GameObject p_loadingScreen; //parent loading screen
    [SerializeField] GameObject m_loadingScreen;
    [SerializeField] Slider progressBar;
    [SerializeField] private float timeDuration = 3f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SwitchToScene(string nameScene)
    {
        if (p_loadingScreen != null && !p_loadingScreen.activeInHierarchy)
        {
            p_loadingScreen.SetActive(true);
        }
        StartCoroutine(StartLoadingScreen(nameScene));
        AudioListener.pause = true;
    }

    IEnumerator StartLoadingScreen(string nameScene)
    {
        m_loadingScreen.SetActive(true);
        progressBar.value = 0;
        yield return null;

        yield return StartCoroutine(SwitchToSceneAsyc(nameScene));
    }

    IEnumerator SwitchToSceneAsyc(string nameScene)
    {
        AsyncOperation asycLoad = SceneManager.LoadSceneAsync(nameScene);
        Time.timeScale = 1f;
        while (!asycLoad.isDone)
        {
            progressBar.value = asycLoad.progress;
            yield return null;
        }
        yield return new WaitForSeconds(timeDuration);
        m_loadingScreen.SetActive(false);
        AudioListener.pause = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
