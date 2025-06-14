using UnityEngine;
using UnityEngine.UI;

public class LanguageButtonHandler : MonoBehaviour
{
    [SerializeField] Button indonesianButton; 
    [SerializeField] Button englishButton;
    [SerializeField] GameObject option;
    [SerializeField] GameObject menu;

    void Start()
    {
        indonesianButton.onClick.AddListener(SetIndonesian);
        englishButton.onClick.AddListener(SetEnglish);
    }

    void SetIndonesian()
    {
        LanguageManager.Instance.SetLanguage(LanguageManager.Language.Indonesian);
        UpdateUI(); 
    }

    void SetEnglish()
    {
        LanguageManager.Instance.SetLanguage(LanguageManager.Language.English);
        UpdateUI(); 
    }

    void UpdateUI()
    {
        option.SetActive(false);
        menu.SetActive(true);
    }
}