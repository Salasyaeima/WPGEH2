using System.Collections;
using TMPro;
using UnityEngine;

public class TaskPanelControllerTutor : MonoBehaviour
{
    public GameObject taskPanel;
    public TaskTutorialTake taskTutorialTake;
    public bool isShow = true;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] GameObject panelProgress;
    [SerializeField] GameObject intruction2;
    [SerializeField] GameObject tutorControl;
    [SerializeField] Container wardrobeContainer;
    [SerializeField] TextDialogChild textDialogChild;
    [SerializeField] GameObject blinkController;
    [SerializeField] string paperSFXName = "paper";
    [SerializeField] TextMeshProUGUI textProgress;

    bool hasSeenTasks = false;
    CanvasGroup canvasGroup;
    bool isPanelOpen = false;
    bool hasProcessedFullContainer = false;

    void Start()
    {
        taskPanel.SetActive(false);
        panelProgress.SetActive(false);
        intruction2.SetActive(false);
        tutorControl.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && taskTutorialTake.isDone && isShow)
        {
            TogglePanel();
        }

        if (wardrobeContainer != null &&
       wardrobeContainer.containerType == Container.ContainerType.wardrobe &&
       wardrobeContainer.storedItems.Count >= wardrobeContainer.maxCapacity &&
            !hasProcessedFullContainer)
        {
            hasProcessedFullContainer = true;
            infoText.text = "";
            StartCoroutine(ContinueThenNext());
        }
    }

    void TogglePanel()
    {
        AudioManager.instance.PlaySFX(paperSFXName);
        isPanelOpen = !isPanelOpen;
        if (isPanelOpen)
        {
            taskPanel.SetActive(true);
            TaskManager.Instance.ShowTasks();
            infoText.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Press [T] again to close the task." : "Tekan [T] lagi untuk menutup tugas";
            textProgress.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "This is your assignment progress" : "Ini progres tugasmu";
            
        }
        else
        {
            taskPanel.SetActive(false);
            isShow = false;
            infoText.text = "";

            if (!hasSeenTasks)
            {
                StartCoroutine(ActivateTutorControlWithDelay(2f));
                intruction2.SetActive(true);
                panelProgress.SetActive(true);
                hasSeenTasks = true;
            }
        }
    }

    IEnumerator ActivateTutorControlWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        intruction2.SetActive(false);
        tutorControl.SetActive(true);
    }

    public void CursorNonActive()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        infoText.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Complete the task of stuffing clothes" : "Selesaikan tugas memasukkan baju";
        PlayerInteractions.canInteractWithClothes = true;
        textDialogChild.playerInteractions.canInteract = true;
        textDialogChild.playerInteractions.SetInteractionMode(PlayerInteractions.InteractionMode.clothesOnly);
    }

    IEnumerator ContinueThenNext()
    {
        textDialogChild.ResumeDisplayingText();
        yield return new WaitForSeconds(3f);
        blinkController.SetActive(true);
    }


}
