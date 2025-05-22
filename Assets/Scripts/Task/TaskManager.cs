using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public List<Task> tasks = new List<Task>();
    private Dictionary<Room, List<Task>> roomTasks = new Dictionary<Room, List<Task>>();
    public GameObject taskUIPrefab;
    public Transform taskListParent;
    public Image progressBar;
    [SerializeField] TextMeshProUGUI taskText;
    [SerializeField] TextMeshProUGUI totalTask;
    [SerializeField] TextMeshProUGUI tasksPerRoom;
    [SerializeField] TextMeshProUGUI roomText;
    [SerializeField] TextMeshProUGUI detailProgression;
    [SerializeField] GameObject player;
    public GameObject panelResult;

    Room[] rooms;
    int completedRooms = 0;
    int completedTasks = 0;
    bool tasksShown = false;
    Room currentRoom;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        rooms = Object.FindObjectsByType<Room>(FindObjectsSortMode.None);
        UpdateTaskInfo();
        UpdateTasksPerRoom();
    }

    public GameObject Player
    {
        get { return player; }
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
        UpdateTasksPerRoom();
    }

    public void ShowTasks()
    {
        if (!tasksShown)
        {
            foreach (Transform child in taskListParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var roomTask in roomTasks)
            {
                Room room = roomTask.Key;
                List<Task> tasksInRoom = roomTask.Value;

                if (tasksInRoom.Count > 0)
                {
                    GameObject roomHeader = Instantiate(taskUIPrefab, taskListParent);
                    roomText = roomHeader.GetComponentInChildren<TextMeshProUGUI>();
                    if (roomText != null)
                    {
                        roomText.text = room.roomName;
                        roomText.fontStyle = FontStyles.Bold | FontStyles.Underline;
                        roomText.fontSize = 40;
                        TaskUI taskUI = roomHeader.GetComponent<TaskUI>();
                        if (taskUI != null) Destroy(taskUI);
                    }
                    else
                    {
                        Debug.LogWarning($"taskUIPrefab untuk {room.roomName} tidak memiliki TextMeshProUGUI!");
                    }

                    foreach (Task task in tasksInRoom)
                    {
                        GameObject taskUI = Instantiate(taskUIPrefab, taskListParent);
                        TaskUI taskUIComponent = taskUI.GetComponent<TaskUI>();
                        taskUIComponent.Initialize(task, FindProviderForTask(task));
                    }
                }
                else
                {
                    Debug.Log($"Tidak ada tugas untuk ruangan: {room.roomName}");
                }
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
            UpdateTasksPerRoom();

            if (completedTasks == tasks.Count && completedRooms >= rooms.Length)
            {
                panelResult.SetActive(true);
                Timer.Instance.CompleteGame();
            }
        }
    }

    void UpdateTaskInfo()
    {
        if (taskText != null && tasks != null)
            taskText.text = $"{completedTasks}/{tasks.Count} Task";

        if (totalTask != null && tasks != null)
            totalTask.text = $"Total Tugas: {tasks.Count}";

        if (roomText != null && rooms != null)
            roomText.text = $"{completedRooms}/{rooms.Length} Ruangan";
    }


    void UpdateTasksPerRoom()
    {
        if (tasksPerRoom == null)
        {
            Debug.LogWarning("tasksPerRoom tidak diatur di Inspector!");
            return;
        }

        if (currentRoom == null)
        {
            tasksPerRoom.text = "Tugas di Ruangan Ini: -/-";
        }
        else
        {
            int tasksInRoom = 0;
            int completedTasksInRoom = 0;

            foreach (Task task in tasks)
            {
                if (task.room == currentRoom)
                {
                    tasksInRoom++;
                    if (task.isCompleted)
                        completedTasksInRoom++;
                }
            }
            tasksPerRoom.text = $"Tugas di Ruangan Ini: {completedTasksInRoom}/{tasksInRoom}";
        }
    }

    void UpdateProgressBar()
    {
        if (tasks.Count > 0)
        {
            float progress = (float)completedTasks / tasks.Count;
            progressBar.fillAmount = progress;

            int percentage = Mathf.RoundToInt(progress * 100f);
            detailProgression.text = $"{percentage}% Tugas Selesai";
        }
    }

    public void RegisterTask(string taskName, ITaskProvider provider, Room room = null)
    {
        Task newTask = new Task { taskName = taskName, isCompleted = false, room = room };
        tasks.Add(newTask);

        if (room != null)
        {
            if (!roomTasks.ContainsKey(room))
            {
                roomTasks[room] = new List<Task>();
            }
            roomTasks[room].Add(newTask);
        }

        UpdateTaskInfo();
    }

    ITaskProvider FindProviderForTask(Task task)
    {
        Container[] containers = Object.FindObjectsByType<Container>(FindObjectsSortMode.None);
        foreach (Container container in containers)
        {
            if (container.GetBaseTaskName() == task.taskName)
                return container;
        }
        Container2[] container2s = Object.FindObjectsByType<Container2>(FindObjectsSortMode.None);
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