using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    List<AudioSource> audioSources = new List<AudioSource>();
    Dictionary<string, AudioSource> loopingSources = new Dictionary<string, AudioSource>();
    public List<AudioClip> sfxClips;
    [SerializeField] AudioClip mainMenuBGM;
    [SerializeField] AudioClip mainGameBGM;
    AudioSource bgmSource;
    [SerializeField] float bgmVolume = 0.5f;
    [SerializeField] float fadeDuration = 1.0f;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            audioSources.Add(source);
        }

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        OnSceneLoaded(currentScene, LoadSceneMode.Single);
    }


    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip targetBGM = null;
        Debug.Log(scene.name);
        if (scene.name == "MainMenu")
        {
            targetBGM = mainMenuBGM;
        }
        else if (scene.name == "RoomsTutorial" || scene.name == "Rooms")
        {
            targetBGM = mainGameBGM;
        }

        if (targetBGM != null && bgmSource.clip != targetBGM)
        {
            StartCoroutine(FadeAndSwitchBGM(targetBGM));
        }
        else if (targetBGM != null && !bgmSource.isPlaying)
        {
            bgmSource.clip = targetBGM;
            bgmSource.Play();
        }
        else if (targetBGM == null)
        {
            Debug.LogWarning($"BGM untuk scene {scene.name} tidak diatur di AudioManager!");
        }
    }

    IEnumerator FadeAndSwitchBGM(AudioClip newClip)
    {
        if (bgmSource.isPlaying)
        {
            float startVolume = bgmSource.volume;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
                yield return null;
            }
            bgmSource.Stop();
        }

        bgmSource.clip = newClip;
        bgmSource.Play();
        float elapsedFadeIn = 0f;
        while (elapsedFadeIn < fadeDuration)
        {
            elapsedFadeIn += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, bgmVolume, elapsedFadeIn / fadeDuration);
            yield return null;
        }
    }



    public void PlaySFX(string sfxName)
    {
        AudioClip clip = sfxClips.Find(s => s.name == sfxName);
        if (clip == null)
        {
            Debug.LogWarning("SFX " + sfxName + " tidak ditemukan!");
            return;
        }

        AudioSource availableSource = audioSources.Find(s => !s.isPlaying);
        if (availableSource != null)
        {
            availableSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Tidak ada AudioSource yang tersedia!");
        }
    }

    public void PlayLoopingSFX(string sfxName)
    {
        if (loopingSources.ContainsKey(sfxName))
        {
            if (!loopingSources[sfxName].isPlaying)
            {
                loopingSources[sfxName].Play();
            }
            return;
        }

        AudioClip clip = sfxClips.Find(s => s.name == sfxName);
        if (clip == null)
        {
            Debug.LogWarning("SFX " + sfxName + " tidak ditemukan!");
            return;
        }

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.playOnAwake = false;
        source.loop = true;
        source.Play();
        loopingSources[sfxName] = source;
    }

    public void PlayRandomSFX(string[] sfxNames)
    {
        if (sfxNames.Length == 0) return;

        string sfxName = sfxNames[Random.Range(0, sfxNames.Length)];
        PlaySFX(sfxName);
    }

    public void StopLoopingSFX(string sfxName)
    {
        if (loopingSources.ContainsKey(sfxName))
        {
            loopingSources[sfxName].Stop();
        }
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgmSource.clip != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    public void SetBGM(AudioClip newBgmClip)
    {
        if (newBgmClip != null)
        {
            StartCoroutine(FadeAndSwitchBGM(newBgmClip));
        }
        else
        {
            Debug.LogWarning("BGM Clip baru tidak valid!");
        }
    }
}