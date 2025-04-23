using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    [SerializeField] GameObject m_loadingScreen;
    [SerializeField] Slider progressBar;


    private void Awake()
    {
        if(Instance != null && Instance != this)
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
        m_loadingScreen.SetActive(true);
        progressBar.value = 0;
        StartCoroutine(SwitchToSceneAsyc(nameScene));
    }

    IEnumerator SwitchToSceneAsyc(string nameScene)
    {
        AsyncOperation asycLoad = SceneManager.LoadSceneAsync(nameScene);
        while(!asycLoad.isDone)
        {
            progressBar.value = asycLoad.progress;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        m_loadingScreen.SetActive(false);
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
