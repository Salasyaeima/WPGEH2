using UnityEngine;
using TMPro;
using System;

public class TextDisplayManager : MonoBehaviour
{
    [System.Serializable]
    public class TextData
    {
        [TextArea] public string text;
        public bool triggerAngryModel;
    }
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] TargetWalk targetWalk;
    [SerializeField] private TextData[] textList;
    [SerializeField] bool useAutoDisplay = true;
    [SerializeField] float displayDuration = 3f;
    int currentTextIndex = 0;
    float timer = 0f;
    bool isDisplaying = false;

    void Awake()
    {
        ValidateReferences();
    }

    void Start()
    {
        if (textUI != null)
        {
            textUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isDisplaying)
        {
            if (useAutoDisplay)
            {
                timer += Time.deltaTime;
                if (timer >= displayDuration)
                {
                    NextText();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    NextText();
                }
            }
        }
    }

    void ValidateReferences()
    {
        if (textUI == null) Debug.LogError("TextMeshProUGUI is not assigned in TextDisplayManager!");
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
        textUI.gameObject.SetActive(true);
        textUI.text = textList[currentTextIndex].text;
        timer = 0f;
    }

    public void StopDisplayingText()
    {
        if (!isDisplaying || textUI == null) return;

        isDisplaying = false;
        textUI.gameObject.SetActive(false);

        if (currentTextIndex < textList.Length)
        {
            currentTextIndex++;
        }
    }

    private void NextText()
    {
        currentTextIndex++;
        if (currentTextIndex < textList.Length && textUI != null)
        {
            textUI.text = textList[currentTextIndex].text;
            CheckAngryState();
            timer = 0f;
        }
        else
        {
            isDisplaying = false;
            textUI.gameObject.SetActive(false);
        }
    }

    void CheckAngryState()
    {
        if (currentTextIndex < textList.Length && textList[currentTextIndex].triggerAngryModel)
        {
            targetWalk?.ShowAngryModel();
        }
    }
    public bool IsDisplaying()
    {
        return isDisplaying;
    }
}