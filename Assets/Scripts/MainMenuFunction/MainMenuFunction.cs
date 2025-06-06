using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;

public class MainMenuFunction : MonoBehaviour
{
    [SerializeField] public Slider bgmSlider;
    [SerializeField] public Slider sfxSlider;
    public AudioMixer mainMixer;
    

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetBGMVolume(float volume)
    {
        if (volume > 0.001f)
        {
            mainMixer.SetFloat("BGMVol", Mathf.Log10(volume) * 20);
        }
        else
        {
            mainMixer.SetFloat("BGMVol", -80f);
        }
        PlayerPrefs.SetFloat("BGMVolValue", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume > 0.001f)
        {
            mainMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        }
        else
        {
            mainMixer.SetFloat("SFXVol", -80f);
        }
        PlayerPrefs.SetFloat("SFXVolValue", volume);
    }

    private void LoadVolumeSettings()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolValue", 0.75f);
        bgmSlider.value = bgmVolume;
        SetBGMVolume(bgmVolume);

        float sfxVolume = PlayerPrefs.GetFloat("SFXVolValue", 0.75f);
        sfxSlider.value = sfxVolume;
        SetSFXVolume(sfxVolume);
    }
}
