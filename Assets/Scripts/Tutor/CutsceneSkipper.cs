using UnityEngine;
using TMPro; // Jangan lupa kalau pakai TextMeshPro
using System.Collections;

public class CutsceneSkipper : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "RoomsTutorial";

    [Header("Fade-in Text")]
    public TextMeshProUGUI cutsceneText;
    public float delayBeforeFade = 5f;
    public float fadeDuration = 2f;

    void Start()
    {
        if (cutsceneText != null)
        {
            UpdateCutsceneText();
            Color c = cutsceneText.color;
            c.a = 0;
            cutsceneText.color = c;

            StartCoroutine(FadeInAfterDelay());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadingScreen.Instance.SwitchToScene(nextSceneName);
        }
    }

    void UpdateCutsceneText()
    {
        if (LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.Indonesian)
        {
            cutsceneText.text = "Tekan [ESC] Untuk Skip >>";
        }
        else
        {
            cutsceneText.text = "Press [ESC] to Skip >>";
        }
    }

    IEnumerator FadeInAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            Color c = cutsceneText.color;
            c.a = alpha;
            cutsceneText.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Color finalColor = cutsceneText.color;
        finalColor.a = 1f;
        cutsceneText.color = finalColor;
    }
}

