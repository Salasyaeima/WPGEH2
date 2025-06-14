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
    [SerializeField] TextMeshProUGUI taskTextPause;
    [SerializeField] TextMeshProUGUI totalRoamPause;
    [SerializeField] TextMeshProUGUI tasksPerRoom;
    [SerializeField] TextMeshProUGUI roomText;
    [SerializeField] TextMeshProUGUI roomTextComplated;
    [SerializeField] TextMeshProUGUI detailProgression;
    [SerializeField] GameObject player;
    [SerializeField] string TaskResultSFX = "Instrument";
    public GameObject panelWin;
    public GameObject panelLose;
    [SerializeField] Button winResultButton;
    [SerializeField] Button loseResultButton;

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
        completedTasks = 0;
    }

    void Start()
    {
        rooms = Object.FindObjectsByType<Room>(FindObjectsSortMode.None);
        UpdateProgressBar();
        UpdateTaskInfo();
        UpdateTasksPerRoom();

        if (winResultButton != null)
        {
            winResultButton.onClick.RemoveAllListeners();
            winResultButton.onClick.AddListener(() =>
            {
                OnWinResultButtonClicked();
                winResultButton.interactable = false;
            });
        }
        else
        {
            Debug.LogWarning("WinResultButton tidak diatur di Inspector!");
        }

        if (loseResultButton != null)
        {
            loseResultButton.onClick.RemoveAllListeners();
            loseResultButton.onClick.AddListener(() =>
            {
                OnLoseResultButtonClicked();
                loseResultButton.interactable = false;
            });
        }
        else
        {
            Debug.LogWarning("LoseResultButton tidak diatur di Inspector!");
        }
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
                        roomText.text = room.GetLocalizedRoomName();
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


    public void CompleteTask(Task task, bool playSound = true)
    {
        if (!task.isCompleted)
        {
            task.isCompleted = true;
            completedTasks++;
            if (playSound) 
            {
                AudioManager.instance.PlaySFX(TaskResultSFX, 0.5f);
            }

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

            // if (completedTasks == tasks.Count && completedRooms >= rooms.Length)
            // {
            //     panelWin.SetActive(true); 
            //     Time.timeScale = 0f;
            //     Cursor.visible = true;
            //     Cursor.lockState = CursorLockMode.None;
            // }
        }
    }

    void UpdateTaskInfo()
    {
        if (taskText != null && tasks != null)
            taskText.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"{completedTasks}/{tasks.Count} Tasks"
            : $"{completedTasks}/{tasks.Count} Tugas";

        if (totalTask != null && tasks != null)
             totalTask.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"Total Tasks: {tasks.Count}"
            : $"Total Tugas: {tasks.Count}";

        if (taskTextPause != null && tasks != null)
            taskTextPause.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"{completedTasks}/{tasks.Count} Tasks"
            : $"{completedTasks}/{tasks.Count} Tugas";

        if (roomTextComplated != null && rooms != null)
           roomTextComplated.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"{completedRooms}/{rooms.Length} Rooms"
            : $"{completedRooms}/{rooms.Length} Ruangan";

        if (totalRoamPause != null && rooms != null)
             totalRoamPause.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"{completedRooms}/{rooms.Length} Rooms"
            : $"{completedRooms}/{rooms.Length} Ruangan";
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
            tasksPerRoom.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? "Tasks remaining in this room: -/-"
            : "Tugas yang tersisa di Ruangan Ini: -/-";
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
            tasksPerRoom.text = $"Tugas yang tersisa di Ruangan Ini: {completedTasksInRoom}/{tasksInRoom}";
            tasksPerRoom.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"Tasks remaining in this room: {completedTasksInRoom}/{tasksInRoom}"
            : $"Tugas yang tersisa di Ruangan Ini: {completedTasksInRoom}/{tasksInRoom}";
        }
    }

    void UpdateProgressBar()
    {
        if (tasks.Count > 0)
        {
            float progress = (float)completedTasks / tasks.Count;
            progressBar.fillAmount = progress;

            int percentage = Mathf.RoundToInt(progress * 100f);
            detailProgression.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? $"{percentage}% Tasks Completed"
            : $"{percentage}% Tugas Selesai";
        }
        else
        {
            progressBar.fillAmount = 0f;
            detailProgression.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "0% Tasks Completed" :  "0% Tugas Selesai";
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
        UpdateProgressBar();
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

       if (completedTasks == tasks.Count && completedRooms >= rooms.Length)
        {
            panelWin.SetActive(true); 

            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        // else
        // {
        //     panelLose.SetActive(true);
        //     Time.timeScale = 0f;
        //     Cursor.visible = true;
        //     Cursor.lockState = CursorLockMode.None;
        // }
    }

     public void OnWinResultButtonClicked()
    {
        LoadingScreen.Instance.SwitchToScene("EndingCutScene"); 
    }

    public void OnLoseResultButtonClicked()
    {
        LoadingScreen.Instance.SwitchToScene("Rooms"); 
    }

}