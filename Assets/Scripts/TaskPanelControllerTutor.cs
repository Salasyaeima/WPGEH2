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

    CanvasGroup canvasGroup;
    bool isPanelOpen = false;

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
            panelProgress.SetActive(true);
            intruction2.SetActive(true);

            StartCoroutine(ActivateTutorControlWithDelay(2f));
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
        infoText.text = "Selesaikan tugas membersihkan mainan";
        PlayerInteractions.canInteractWithClothes = true;
    }

}
