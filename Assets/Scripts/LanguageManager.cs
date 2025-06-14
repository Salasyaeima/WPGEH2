using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language { Indonesian, English }
    private Language currentLanguage = Language.Indonesian;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguage();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Language GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
        PlayerPrefs.SetInt("Language", (int)language);
        PlayerPrefs.Save();
    }

    private void LoadLanguage()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            currentLanguage = (Language)PlayerPrefs.GetInt("Language");
        }
    }
}