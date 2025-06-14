using UnityEngine;
using TMPro;
using System.Collections;

public class TextDisplayManager : MonoBehaviour
{
    [System.Serializable]
    public class TextData
    {
        [TextArea] public string indonesianText;
        [TextArea] public string englishText;
        public bool triggerAngryModel;
        public float duration = 3f;
    }

    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] TargetWalk targetWalk;
    [SerializeField] TextData[] textList;
    [SerializeField] bool useAutoDisplay = true;
    [SerializeField] string yaaSFXName = "Iyahh";
    [SerializeField] string EehhSFXName = "Eehh";
    [SerializeField] string marahSFXName = "Marahibu";


    

    int currentTextIndex = 0;
    bool isDisplaying = false;
    Coroutine displayCoroutine;

    void Awake()
    {
        ValidateReferences();
    }

    void Start()
    {
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isDisplaying && !useAutoDisplay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextText();
            }
        }
    }

    private void ValidateReferences()
    {
        if (textMeshPro == null) Debug.LogError("TextMeshProUGUI is not assigned in TextDisplayManager!");
        if (targetWalk == null) Debug.LogError("TargetWalk is not assigned in TextDisplayManager!");
        if (textList == null || textList.Length == 0) Debug.LogWarning("Text list is empty in TextDisplayManager!");
    }

    public void StartDisplayingText()
    {
        if (textList.Length == 0)
        {
            Debug.LogWarning("Text list is empty in TextDisplayManager!");
            return;
        }

        if (currentTextIndex >= textList.Length)
        {
            Debug.Log("No more texts to display in TextDisplayManager!");
            return;
        }

        isDisplaying = true;
        textMeshPro.gameObject.SetActive(true);
        textMeshPro.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? textList[currentTextIndex].englishText
            : textList[currentTextIndex].indonesianText;

        if (useAutoDisplay && displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(AutoDisplayText());
        }
    }

    public void StopDisplayingText()
    {
        if (!isDisplaying || textMeshPro == null) return;

        isDisplaying = false;
        textMeshPro.gameObject.SetActive(false);

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }

        if (currentTextIndex < textList.Length)
        {
            currentTextIndex++;
        }
    }

    private void NextText()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }

        currentTextIndex++;
        if (currentTextIndex < textList.Length && textMeshPro != null)
        {
            textMeshPro.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? textList[currentTextIndex].englishText
                : textList[currentTextIndex].indonesianText;
            CheckAngryState();
            if (useAutoDisplay)
            {
                displayCoroutine = StartCoroutine(AutoDisplayText());
            }

        }
        else
        {
            isDisplaying = false;
            textMeshPro.gameObject.SetActive(false);
        }

        if (currentTextIndex == 5)
        {
            // AudioManager.instance.PlaySFX(yaaSFXName, 0.05f);
        }
        else if (currentTextIndex == 8)
        {
            AudioManager.instance.PlaySFX(EehhSFXName, 0.03f);
        }
        else if (currentTextIndex == 11)
        {
            AudioManager.instance.PlaySFX(marahSFXName, 0.10f);
        }
    }

    private IEnumerator AutoDisplayText()
    {
        yield return new WaitForSeconds(textList[currentTextIndex].duration);
        NextText();
    }

    private void CheckAngryState()
    {
        if (currentTextIndex < textList.Length && textList[currentTextIndex].triggerAngryModel)
        {
            if (targetWalk != null)
            {
                targetWalk.ShowAngryModel();
            }
            else
            {
                Debug.LogWarning("TargetWalk is null, cannot trigger angry model!");
            }
        }
    }


    public void ResetTextIndex()
    {
        currentTextIndex = 0;
        isDisplaying = false;
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayCoroutine = null;
        }
    }

    public bool IsDisplaying() => isDisplaying;
}