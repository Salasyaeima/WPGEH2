using UnityEngine;
using TMPro;

public class TextDialogChild : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] string[] texts;
    [SerializeField] bool useAutoDisplay = true;
    [SerializeField] float textDuration = 2f;

    private int currentTextIndex = 0;
    private bool isDisplaying = false;
    private float timer = 0f;
    private bool isPermanentlyStopped = false;
    private bool isPaused = false;

    void Awake()
    {
        ValidateReferences();
    }

    void OnEnable()
    {
        StartDisplayingText();
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
        Canvas canvas = textDisplay?.GetComponentInParent<Canvas>();
        if (canvas == null || !canvas.isActiveAndEnabled)
        {
            Debug.LogError($"{gameObject.name}: TextMeshProUGUI is not in an active Canvas!");
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

    public void NextText()
    {
        currentTextIndex++;

        if (currentTextIndex == 5)
        {
            StopDisplayingText();
            return;
        }
        else if (currentTextIndex == 10)
        {
            StopDisplayingText();
            return;
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
            textDisplay.gameObject.SetActive(true);
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
            textDisplay.gameObject.SetActive(false);
        }
    }

    public void ResumeDisplayingText()
    {
        if (isDisplaying)
        {
            Debug.Log($"{gameObject.name}: ResumeDisplayingText called, but already displaying");
            return;
        }

        if (currentTextIndex < texts.Length)
        {
            isDisplaying = true;
            isPaused = false;
            if (textDisplay != null)
            {
                textDisplay.gameObject.SetActive(true);
                textDisplay.text = texts[currentTextIndex];
            }
            timer = 0f;
        }
        else
        {
            Debug.Log($"{gameObject.name}: No more text to resume");
        }
    }
}