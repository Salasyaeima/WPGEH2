using UnityEngine;
using TMPro;
using System.Collections;

public class TextDisplayManager : MonoBehaviour
{
    [System.Serializable]
    public class TextData
    {
        [TextArea] public string text;
        public bool triggerAngryModel;
        public float duration = 3f;
    }

    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] TargetWalk targetWalk;
    [SerializeField] TextData[] textList;
    [SerializeField] bool useAutoDisplay = true;
    

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
        textMeshPro.text = textList[currentTextIndex].text;

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
            textMeshPro.text = textList[currentTextIndex].text;
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