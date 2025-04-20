using UnityEngine;
using TMPro;

public class TextDisplayManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] GameObject mother;
    [SerializeField] TargetWalk targetWalk;
    [SerializeField] string[] textList;
    [SerializeField] float displayDuration = 3f;
    [SerializeField] bool useAutoDisplay = true;
    int currentTextIndex = 0;
    float timer = 0f;
    bool isDisplaying = false;

    void Start()
    {
        if (textUI != null)
        {
            textUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI is not assigned in TextDisplayManager!");
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
        textUI.text = textList[currentTextIndex];
        timer = 0f;
        Debug.Log($"Displaying text: {textList[currentTextIndex]}");
    }

    public void StopDisplayingText()
    {
        if (isDisplaying)
        {
            isDisplaying = false;
            textUI.gameObject.SetActive(false);
            if (currentTextIndex < textList.Length)
            {
                currentTextIndex++;
                Debug.Log($"Text display stopped, advancing to index: {currentTextIndex}");
            }
        }
    }

    private void NextText()
    {
        currentTextIndex++;
        if (currentTextIndex < textList.Length)
        {
            textUI.text = textList[currentTextIndex];
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
        if (currentTextIndex == 9)
        {
            targetWalk.TampilkanMarah();
        }
    }
    public bool IsDisplaying()
    {
        return isDisplaying;
    }
}