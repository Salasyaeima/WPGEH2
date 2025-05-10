using System.Collections;
using TMPro;
using UnityEngine;

public class TaskPanelControllerTutor : MonoBehaviour
{
    public GameObject taskPanel;
    public TaskTutorialTake taskTutorialTake;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] GameObject panelProgress;
    [SerializeField] GameObject intruction2;
    [SerializeField] GameObject tutorControl;
    [SerializeField] Container wardrobeContainer;
    [SerializeField] TextDialogChild textDialogChild;
    [SerializeField] GameObject blinkController;
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
        if (Input.GetKeyDown(KeyCode.T) && taskTutorialTake.isDone)
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
        isPanelOpen = !isPanelOpen;
        if (isPanelOpen)
        {
            taskPanel.SetActive(true);
            TaskManager.Instance.ShowTasks();
            infoText.text = "Tekan [T] lagi untuk menutup tugas";
        }
        else
        {
            taskPanel.SetActive(false);
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
        infoText.text = "Selesaikan tugas memasukkan baju";
        PlayerInteractions.canInteractWithClothes = true;
    }

    IEnumerator ContinueThenNext()
    {
        textDialogChild.ResumeDisplayingText();
        yield return new WaitForSeconds(3f);
        blinkController.SetActive(true);
    }


}
