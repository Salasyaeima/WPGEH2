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
                    taskUI.UpdateCheckIconPosition();
                    break;
                }
            }
            UpdateProgressBar();

            Debug.Log($"Task '{task.taskName}' completed!");
        }
    }

    void UpdateProgressBar()
    {
        if (tasks.Count > 0)
        {
            float progress = (float)completedTasks / tasks.Count;
            progressBar.fillAmount = progress;
        }
    }

    public void RegisterTask(string taskName, ITaskProvider provider)
    {
        foreach (Task existingTask in tasks)
        {
            if (existingTask.taskName == taskName)
            {
                Debug.LogWarning($"Task '{taskName}' sudah ada, skip registrasi.");
                return;
            }
        }

        Task newTask = new Task { taskName = taskName, isCompleted = false };
        tasks.Add(newTask);
        GameObject taskUI = Instantiate(taskUIPrefab, taskListParent);
        TaskUI taskUIComponent = taskUI.GetComponent<TaskUI>();
        taskUIComponent.Initialize(newTask, provider);
        Debug.Log($"Task baru '{taskName}' dari {provider.GetType().Name} ({(provider as MonoBehaviour)?.gameObject.name}) ditambah.");
    }

    void Update()
    {
        foreach (TaskUI ui in taskListParent.GetComponentsInChildren<TaskUI>())
        {
            if (string.IsNullOrEmpty(ui.taskText.text))
            {
                Debug.LogError($"TaskUI kosong ditemukan di {ui.gameObject.name}!");
            }
        }
    }
}