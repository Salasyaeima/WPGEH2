using UnityEngine;

public class TaskPanelController : MonoBehaviour
{
    public GameObject taskPanel;
    CanvasGroup canvasGroup;
    bool isPanelOpen = false;

    void Start()
    {
        taskPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
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
        }
        else
        {
            taskPanel.SetActive(false);
        }
    }
}
