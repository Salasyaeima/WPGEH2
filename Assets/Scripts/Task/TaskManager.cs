using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public List<Task> tasks = new List<Task>();
    public GameObject taskUIPrefab;
    public Transform taskListParent;
    public Image progressBar;
    [SerializeField] TextMeshProUGUI taskText;
    [SerializeField] TextMeshProUGUI roomText;
    [SerializeField] GameObject panelResult;

    Room[] rooms;
    int completedRooms = 0;
    int completedTasks = 0;
    bool tasksShown = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        rooms = FindObjectsOfType<Room>();
        UpdateTaskInfo();
        Debug.Log($"Total Ruangan: {rooms.Length}");
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

            if (task.room != null)
            {
                task.room.OnTaskCompleted();
            }
            UpdateTaskInfo();

            if (completedTasks == tasks.Count && completedRooms >= rooms.Length)
            {
                panelResult.SetActive(true);
                Timer.Instance.CompleteGame();
            }
        }
    }

    void UpdateTaskInfo()
    {
        if (taskText != null)
            taskText.text = $"{completedTasks}/{tasks.Count} Task";
        if (roomText != null)
            roomText.text = $"{completedRooms}/{rooms.Length} Ruangan";
    }

    void UpdateProgressBar()
    {
        if (tasks.Count > 0)
        {
            float progress = (float)completedTasks / tasks.Count;
            progressBar.fillAmount = progress;
        }
    }

    public void RegisterTask(string taskName, ITaskProvider provider, Room room = null)
    {
        foreach (Task existingTask in tasks)
        {
            if (existingTask.taskName == taskName)
            {
                Debug.LogWarning($"Task '{taskName}' sudah ada, skip registrasi.");
                return;
            }
        }
        Task newTask = new Task { taskName = taskName, isCompleted = false, room = room };
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

    public void OnRoomCompleted()
    {
        completedRooms = 0;
        foreach (Room room in rooms)
        {
            if (room.IsCompleted())
                completedRooms++;
        }
        UpdateTaskInfo();
        Debug.Log($"Ruangan selesai: {completedRooms}/{rooms.Length}");

        if (completedTasks == tasks.Count && completedRooms >= rooms.Length)
        {
            panelResult.SetActive(true);
            Timer.Instance.CompleteGame();
        }
    }
}