using UnityEngine;
using TMPro;
using System.Collections;
using Unity.AppUI.UI;

public class TextDialogChild : MonoBehaviour
{
    [System.Serializable]
    public struct TextData
    {
        [TextArea] public string indonesianText;
        [TextArea] public string englishText;
    }
    public Window_QuestPointer windowQuest;
    public TextMeshProUGUI intruksi;
    public bool useAutoDisplay = true;
    public TextMeshProUGUI textDisplay;
    public PlayerInteractions playerInteractions;
    [SerializeField] GameObject mother;
    [SerializeField] Transform newTargetTransform;
    [SerializeField] Transform[] targetTransforms;
    [SerializeField] GameObject cameraPendukung;
    [SerializeField] GameObject blinkController;
    [SerializeField] HidingMechanism hidingMechanism;
    [SerializeField] MotherTutorial motherTutorial;
    [SerializeField] string hmmSFXName = "Menghela";
    [SerializeField] string ckSFXName = "Ck";
    [SerializeField] string memghelaSFXName = "Menghela";
    [SerializeField] string hoamSFXName = "Hoaaam";
    [SerializeField] string heehSFXName = "HEEHHH";
    [SerializeField] string duhSFXName = "Duh";
    [SerializeField] string pintuSFXName = "Buka_Pintu";
    [SerializeField] string hmmIbuSFXName = "HmmIbu";
    [SerializeField] TextData[] texts;
    [SerializeField] float textDuration = 2f;

    bool lastHidingState;
    int currentTextIndex = 0;
    bool isDisplaying = false;
    float timer = 0f;
    bool isPermanentlyStopped = false;
    bool isPaused = false;

    void Awake()
    {
        ValidateReferences();
        if (hidingMechanism != null)
        {
            lastHidingState = hidingMechanism.isHiding;
        }
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
        if (isDisplaying && !isPaused && !motherTutorial.isChasing)
        {
            if (useAutoDisplay)
            {
                timer += Time.unscaledDeltaTime;
                if (timer >= textDuration)
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

        if (currentTextIndex == 0)
        {
            AudioManager.instance.PlaySFX(hmmSFXName, 0.1f);
        }
       

        isDisplaying = true;
        isPaused = false;
        if (textDisplay != null)
        {
            textDisplay.gameObject.SetActive(true);
            textDisplay.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? texts[currentTextIndex].englishText
                : texts[currentTextIndex].indonesianText;
                Debug.Log($"Language: {LanguageManager.Instance.GetCurrentLanguage()}, Text: {textDisplay.text}");
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
        
        if (currentTextIndex >= texts.Length)
        {
            StopDisplayingText();
            return;
        }
         textDisplay.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? texts[currentTextIndex].englishText
                : texts[currentTextIndex].indonesianText;
                

        if (currentTextIndex != 5 && currentTextIndex != 7 && currentTextIndex != 10 && currentTextIndex != 21)
        {
            playerInteractions.canInteract = false;
        }
        

        if (currentTextIndex == 5)
        {
            intruksi.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? "Enter the room!"
            : "Masuk Kamar!";
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
        else if (currentTextIndex == 8)
        {
            blinkController.SetActive(true);
        }
        else if (currentTextIndex == 9)
        {
            ResumeDisplayingText();
        }
        else if (currentTextIndex == 10)
        {
            AudioManager.instance.PlaySFX(hoamSFXName, 0.1f);
            PauseDisplayingText();
            intruksi.enabled = true;
            playerInteractions.canInteract = true;
            playerInteractions.SetInteractionMode(PlayerInteractions.InteractionMode.BedOnly);
            intruksi.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? "Go to bed"
                : "Pergi Ke tempat tidur";

            newTargetTransform = targetTransforms[0];
            ChangeTargetTransform();
        }
        else if (currentTextIndex == 14)
        {
            AudioManager.instance.PlaySFX(pintuSFXName, 0.5f);
            mother.SetActive(true);
        }
        else if (currentTextIndex == 15)
        {
            cameraPendukung.SetActive(true);
            StartCoroutine(DisableCameraAfterSeconds(1f));
        }
        else if (currentTextIndex == 17)
        {
            intruksi.enabled = true;
            intruksi.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? "Hide in the cardboard"
                : "Sembunyi Ke Kardus";
            playerInteractions.canInteract = true;
            playerInteractions.SetInteractionMode(PlayerInteractions.InteractionMode.HidingOnly);
            newTargetTransform = targetTransforms[1];
            ChangeTargetTransform();
            windowQuest.gameObject.SetActive(true);
            PauseDisplayingText();
            StartCoroutine(CheckHidingState());
        }

        else if (currentTextIndex == 21)
        {
            PauseDisplayingText();
        }

        if (currentTextIndex < texts.Length && textDisplay != null)
        {
            timer = 0f;
        }
        else
        {
            StopDisplayingText();
        }
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
        playerInteractions.canInteract = true;
        playerInteractions.SetInteractionMode(PlayerInteractions.InteractionMode.DoorOnly);
        windowQuest.gameObject.SetActive(true);
    }
    public IEnumerator ResumeInstruksi()
    {
        yield return new WaitForSeconds(3f);
        ResumeDisplayingText();
    }

    IEnumerator DisableCameraAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraPendukung.SetActive(false);
    }

    IEnumerator ShowInstruksiAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        intruksi.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? "Pick up the task!"
            : "Ambil Tugas!";
        playerInteractions.canInteract = true;
        playerInteractions.SetInteractionMode(PlayerInteractions.InteractionMode.TaskOnly);
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
            textDisplay.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? texts[currentTextIndex].englishText
                : texts[currentTextIndex].indonesianText;
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

        if (currentTextIndex == 5)
        {
            AudioManager.instance.PlaySFX(ckSFXName, 0.05f);
        }
        else if (currentTextIndex == 7)
        {
            AudioManager.instance.PlaySFX(memghelaSFXName, 0.05f);
        }
        else if (currentTextIndex == 10)
        {
            AudioManager.instance.PlaySFX(heehSFXName, 0.03f);
        }
        else if (currentTextIndex == 18)
        {
            AudioManager.instance.PlaySFX(hmmIbuSFXName, 0.5f);
        }
        else if (currentTextIndex == 21)
        {
            AudioManager.instance.PlaySFX(duhSFXName, 0.03f);
        }

        if (currentTextIndex < texts.Length)
            {
                isDisplaying = true;
                isPaused = false;
                if (textDisplay != null)
                {
                    textDisplay.enabled = true;
                    textDisplay.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? texts[currentTextIndex].englishText
                : texts[currentTextIndex].indonesianText;
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

    IEnumerator CheckHidingState()
    {
        if (hidingMechanism == null)
        {
            yield break;
        }

        while (!hidingMechanism.isHiding)
        {
            yield return null;
        }

        if (intruksi != null)
        {
            intruksi.enabled = false;
        }

        while (currentTextIndex < 22)
        {
            if (!hidingMechanism.isHiding)
            {
                hidingMechanism.EnterHide();
                AudioManager.instance.StopAllAudio();
                motherTutorial.StopChasing();
            }
            yield return null;
        }
    }
}