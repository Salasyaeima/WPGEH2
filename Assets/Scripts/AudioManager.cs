using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioSource> audioSources = new List<AudioSource>();
    public Dictionary<string, AudioSource> loopingSources = new Dictionary<string, AudioSource>();
    public List<AudioClip> sfxClips;
    [SerializeField] AudioClip mainMenuBGM;
    [SerializeField] AudioClip mainGameBGM;
    AudioSource bgmSource;
    [SerializeField] float bgmVolume = 0.5f;
    [SerializeField] float cutsceneBGMVolume = 0.3f; 
    [SerializeField] float fadeDuration = 1.0f;
    bool isRightStep = false;

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
        float targetVolume = bgmVolume;

        if (scene.name == "MainMenu")
        {
            targetBGM = mainMenuBGM;
            targetVolume = bgmVolume; 
        }
        else if (scene.name == "RoomsTutorial" || scene.name == "Rooms")
        {
            targetBGM = mainGameBGM;
            bgmVolume = 1f;
            targetVolume = bgmVolume; 
        }
        else if (scene.name == "CutScene") 
        {
            targetBGM = mainMenuBGM; 
            targetVolume = cutsceneBGMVolume; 
        }

        if (targetBGM != null && (bgmSource.clip != targetBGM || bgmSource.volume != targetVolume))
        {
            StartCoroutine(FadeAndSwitchBGM(targetBGM, targetVolume));
        }
        else if (targetBGM != null && !bgmSource.isPlaying)
        {
            bgmSource.clip = targetBGM;
            bgmSource.volume = targetVolume;
            bgmSource.Play();
        }
        else if (targetBGM == null)
        {
            Debug.LogWarning($"BGM untuk scene {scene.name} tidak diatur di AudioManager!");
        }
    }

    IEnumerator FadeAndSwitchBGM(AudioClip newClip, float targetVolume)
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
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, elapsedFadeIn / fadeDuration);
            yield return null;
        }
    }

    public void PlaySFX(string sfxName, float volume = 1f)
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
            availableSource.volume = volume;
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
            StartCoroutine(FadeOutLoopingSFX(sfxName));
        }
    }

    IEnumerator FadeOutLoopingSFX(string sfxName)
    {
        AudioSource source = loopingSources[sfxName];
        float startVolume = source.volume;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }
        source.Stop();
        source.volume = 1f;
    }

    public void PlayBGM()
    {
        if (bgmSource != null && bgmSource.clip != null && !bgmSource.isPlaying)
        {
            bgmSource.volume = bgmVolume; 
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null)
        {
            StartCoroutine(FadeOutBGM());
        }
    }

    IEnumerator FadeOutBGM()
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
        bgmSource.volume = bgmVolume;
    }

    public void SetBGM(AudioClip newBgmClip, float volume = -1f)
    {
        if (newBgmClip != null)
        {
            float targetVolume = volume >= 0f ? Mathf.Clamp01(volume) : bgmVolume;
            StartCoroutine(FadeAndSwitchBGM(newBgmClip, targetVolume));
        }
        else
        {
            Debug.LogWarning("BGM Clip baru tidak valid!");
        }
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
    }

    public void StopAllAudio()
    {
        StartCoroutine(FadeOutAllAudio());
    }

    IEnumerator FadeOutAllAudio()
    {
        float startBGMVolume = bgmSource.volume;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            if (bgmSource != null && bgmSource.isPlaying)
            {
                bgmSource.volume = Mathf.Lerp(startBGMVolume, 0f, t);
            }

            foreach (var source in audioSources)
            {
                if (source.isPlaying)
                {
                    source.volume = Mathf.Lerp(source.volume, 0f, t);
                }
            }

            foreach (var kvp in loopingSources)
            {
                if (kvp.Value.isPlaying)
                {
                    kvp.Value.volume = Mathf.Lerp(kvp.Value.volume, 0f, t);
                }
            }

            yield return null;
        }

        if (bgmSource != null)
        {
            bgmSource.Stop();
            bgmSource.volume = bgmVolume;
        }

        foreach (var source in audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
                source.volume = 1f;
            }
        }

        foreach (var kvp in loopingSources)
        {
            if (kvp.Value.isPlaying)
            {
                kvp.Value.Stop();
                kvp.Value.volume = 1f;
            }
        }
    }

    public void PlayWalkingSFX(string leftClipName, string rightClipName)
    {
        AudioClip clip = sfxClips.Find(s => s.name == (isRightStep ? rightClipName : leftClipName));
        if (clip == null)
        {
            Debug.LogWarning($"SFX {(isRightStep ? rightClipName : leftClipName)} tidak ditemukan!");
            return;
        }

        AudioSource walkingSource;
        if (!loopingSources.ContainsKey("Walking"))
        {
            walkingSource = gameObject.AddComponent<AudioSource>();
            walkingSource.playOnAwake = false;
            walkingSource.loop = false;
            loopingSources["Walking"] = walkingSource;
        }
        else
        {
            walkingSource = loopingSources["Walking"];
        }

        walkingSource.panStereo = isRightStep ? 0.5f : -0.5f;
        walkingSource.PlayOneShot(clip);
        isRightStep = !isRightStep;
    }

    public void StopWalkingSFX()
    {
        if (loopingSources.ContainsKey("Walking"))
        {
            StartCoroutine(FadeOutWalkingSFX());
        }
    }

    IEnumerator FadeOutWalkingSFX()
    {
        AudioSource source = loopingSources["Walking"];
        float startVolume = source.volume;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }
        source.Stop();
        source.volume = 1f;
        loopingSources.Remove("Walking");
    }
}