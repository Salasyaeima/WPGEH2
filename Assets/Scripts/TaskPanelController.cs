using TMPro;
using UnityEngine;

public class TaskPanelController1 : MonoBehaviour
{
    public GameObject taskPanel;
    [SerializeField] string paperSFXName = "paper";

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
        AudioManager.instance.PlaySFX(paperSFXName);
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
