using UnityEngine;
using TMPro;
using System.Collections;

public class TextEndingCutScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField, TextArea(3, 10)] string[] textSequence;
    [SerializeField] float displayDuration = 3f;
    Coroutine displayCoroutine;
    bool isDisplaying = false;
    int currentTextIndex = 0;
    bool skipToNextOnResume = false;


    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
        }

        if (textComponent != null && textSequence.Length > 0)
        {
            textComponent.text = "";
        }
    }

    public void StartDisplay()
    {
        if (!isDisplaying && textSequence.Length > 0)
        {
            currentTextIndex = 0;
            isDisplaying = true;
            displayCoroutine = StartCoroutine(DisplayTextSequence());
        }
    }

    public void StopDisplay()
    {
        if (isDisplaying)
        {
            isDisplaying = false;
            skipToNextOnResume = true;
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            if (textComponent != null)
            {
                textComponent.text = "";
            }
        }
    }

    public void ResumeDisplay()
    {
        if (!isDisplaying && currentTextIndex < textSequence.Length)
        {
            isDisplaying = true;
            displayCoroutine = StartCoroutine(DisplayTextSequence());
        }
    }

    private IEnumerator DisplayTextSequence()
    {
        while (currentTextIndex < textSequence.Length)
        {
            if (skipToNextOnResume)
            {
                currentTextIndex++;
                skipToNextOnResume = false;
                if (currentTextIndex >= textSequence.Length) break;
            }

            textComponent.text = textSequence[currentTextIndex];
            yield return new WaitForSeconds(displayDuration);
            currentTextIndex++;
        }

        isDisplaying = false;
        textComponent.text = ""; // Kosongkan teks setelah selesai
    }

    public void SetTextSequence(string[] newTextSequence)
    {
        textSequence = newTextSequence;
        currentTextIndex = 0;
        if (textComponent != null)
        {
            textComponent.text = "";
        }
    }

    public void SetDisplayProgress(float progress)
    {
        if (progress < 0f) progress = 0f;
        if (progress > 1f) progress = 1f;

        int targetIndex = Mathf.FloorToInt((textSequence.Length - 1) * progress);
        if (targetIndex != currentTextIndex)
        {
            currentTextIndex = targetIndex;
            if (currentTextIndex < textSequence.Length && textComponent != null)
            {
                textComponent.text = textSequence[currentTextIndex];
            }
        }

        if (isDisplaying && currentTextIndex >= textSequence.Length - 1)
        {
            StopDisplay();
        }
    }

    public bool IsDisplaying()
    {
        return isDisplaying;
    }
}