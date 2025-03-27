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

    int completedTasks = 0;
    bool tasksShown = false;

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

    public void ShowTasks()
    {
        if (!tasksShown)
        {
            foreach (Task task in tasks)
            {
                GameObject taskUI = Instantiate(taskUIPrefab, taskListParent);
                TaskUI taskUIComponent = taskUI.GetComponent<TaskUI>();
                taskUIComponent.Initialize(task, FindProviderForTask(task));
            }
            tasksShown = true;
            UpdateProgressBar();
        }
    }

    public void HideTasks()
    {
        tasksShown = true;
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
    }

    ITaskProvider FindProviderForTask(Task task)
    {
        Container[] containers = FindObjectsOfType<Container>();
        foreach (Container container in containers)
        {
            if (container.GetBaseTaskName() == task.taskName)
                return container;
        }

        Container2[] container2s = FindObjectsOfType<Container2>();
        foreach (Container2 container2 in container2s)
        {
            if (container2.GetTaskName() == task.taskName)
                return container2;
        }

        return null;
    }
}