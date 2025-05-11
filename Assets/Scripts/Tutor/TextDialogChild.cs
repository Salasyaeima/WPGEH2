using UnityEngine;
using TMPro;
using System.Collections;
using Unity.AppUI.UI;

public class TextDialogChild : MonoBehaviour
{
    [SerializeField] Window_QuestPointer windowQuest;
    [SerializeField] Transform newTargetTransform;
    [SerializeField] string[] texts;
    [SerializeField] float textDuration = 2f;
    public TextMeshProUGUI intruksi;
    public bool useAutoDisplay = true;
    public TextMeshProUGUI textDisplay;

    int currentTextIndex = 0;
    bool isDisplaying = false;
    float timer = 0f;
    bool isPermanentlyStopped = false;
    bool isPaused = false;



    void Awake()
    {
        ValidateReferences();
    }

    void OnEnable()
    {
        StartCoroutine(DelayedStart());
    }

    void OnDisable()
    {
        isDisplaying = false;
        isPaused = false;
        if (textDisplay != null)
        {
            textDisplay.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isDisplaying && !isPaused)
        {
            if (useAutoDisplay)
            {
                timer += Time.unscaledDeltaTime;
                if (timer >= textDuration)
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
        if (textDisplay == null)
        {
            Debug.LogError($"{gameObject.name}: TextMeshProUGUI is not assigned!");
            enabled = false;
        }
        if (texts == null || texts.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name}: Text array is empty!");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Text array has {texts.Length} elements");
        }
    }

    public void StartDisplayingText()
    {
        if (isPermanentlyStopped)
        {
            Debug.Log($"{gameObject.name}: Permanently stopped, cannot restart displaying text.");
            return;
        }
        if (texts == null || texts.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name}: Text array is empty, cannot start!");
            return;
        }

        if (currentTextIndex >= texts.Length)
        {
            Debug.Log($"{gameObject.name}: No more texts to display!");
            return;
        }

        isDisplaying = true;
        isPaused = false;
        if (textDisplay != null)
        {
            textDisplay.gameObject.SetActive(true);
            textDisplay.text = texts[currentTextIndex];
            timer = 0f;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: textDisplay is null, cannot display text!");
            isDisplaying = false;
        }
    }

    public void StopDisplayingText()
    {
        if (!isDisplaying || textDisplay == null)
        {
            Debug.Log($"{gameObject.name}: StopDisplayingText called, but not displaying or textDisplay is null");
            return;
        }

        isDisplaying = false;
        isPermanentlyStopped = true;
        isPaused = false;
        textDisplay.gameObject.SetActive(false);
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        StartDisplayingText();
    }

    IEnumerator DelayedInstruksi()
    {
        yield return new WaitForSeconds(3f);
        intruksi.enabled = true;
        windowQuest.gameObject.SetActive(true);
    }
    public IEnumerator ResumeInstruksi()
    {
        yield return new WaitForSeconds(3f);
        ResumeDisplayingText();

    }
    public void NextText()
    {
        currentTextIndex++;

        if (currentTextIndex == 5)
        {
            PauseDisplayingText();
            StartCoroutine(DelayedInstruksi());
            return;
        }
        else if (currentTextIndex == 7)
        {
            PauseDisplayingText();
            StartCoroutine(ShowInstruksiAfterDelay());
            return;
        }

        else if (currentTextIndex == 9)
        {
            ResumeDisplayingText();
        }
        else if (currentTextIndex == 10)
        {
            PauseDisplayingText();
            intruksi.text = "Pergi Ke tempat tidur";
        }

        if (currentTextIndex < texts.Length && textDisplay != null)
        {
            textDisplay.text = texts[currentTextIndex];
            timer = 0f;
        }
        else
        {
            StopDisplayingText();
        }
    }

    IEnumerator ShowInstruksiAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        intruksi.text = "Ambil Tugas!";
        intruksi.enabled = true;
        ChangeTargetTransform();
    }


    public void ContinueDisplayingText()
    {
        if (texts == null || texts.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name}: Text array is empty, cannot continue!");
            return;
        }

        if (currentTextIndex >= texts.Length)
        {
            Debug.Log($"{gameObject.name}: No more texts to display!");
            return;
        }

        isPermanentlyStopped = false;
        isDisplaying = true;
        if (textDisplay != null)
        {
            textDisplay.enabled = true;
            textDisplay.text = texts[currentTextIndex];
            timer = 0f;
        }
        else
        {
            Debug.LogError($"{gameObject.name}: textDisplay is null, cannot continue!");
            isDisplaying = false;
        }
    }

    public void PauseDisplayingText()
    {
        if (!isDisplaying)
        {
            Debug.Log($"{gameObject.name}: PauseDisplayingText called, but not displaying");
            return;
        }

        isPaused = true;
        isDisplaying = false;
        if (textDisplay != null)
        {
            textDisplay.enabled = false;
        }
    }

    public void ResumeDisplayingText()
    {
        if (isDisplaying)
        {
            return;
        }

        if (currentTextIndex < texts.Length)
        {
            isDisplaying = true;
            isPaused = false;
            if (textDisplay != null)
            {
                textDisplay.enabled = true;
                textDisplay.text = texts[currentTextIndex];
            }
            timer = 0f;
        }
        else
        {
            Debug.Log($"{gameObject.name}: No more text to resume");
        }
    }

    void ChangeTargetTransform()
    {
        if (windowQuest != null && newTargetTransform != null)
        {
            windowQuest.Show(newTargetTransform);
        }
        else
        {
            Debug.LogWarning("Referensi newTargetTransform belum diatur.");
        }
    }
}