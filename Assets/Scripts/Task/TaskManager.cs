using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public List<Task> tasks = new List<Task>();
    public GameObject taskUIPrefab;
    public Transform taskListParent;
    public Image progressBar;

    private int completedTasks = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InitializeTasks();
    }

    void InitializeTasks()
    {
        foreach (Task task in tasks)
        {
            GameObject taskUI = Instantiate(taskUIPrefab, taskListParent);
            TaskUI taskUIComponent = taskUI.GetComponent<TaskUI>();
            taskUIComponent.Initialize(task);
        }

        UpdateProgressBar();
    }

    public void CompleteTask(Task task)
    {
        if (!task.isCompleted)
        {
            task.isCompleted = true;
            completedTasks++;

            foreach (Transform child in taskListParent)
            {
                TaskUI taskUI = child.GetComponent<TaskUI>();
                if (taskUI != null && taskUI.task == task)
                {
                    taskUI.StrikeThroughText();
                    break;
                }
            }
            UpdateProgressBar();

            Debug.Log($"Task '{task.taskName}' completed!");
        }
    }

    void UpdateProgressBar()
    {
        float progress = (float)completedTasks / tasks.Count;
        progressBar.fillAmount = progress;
    }
}