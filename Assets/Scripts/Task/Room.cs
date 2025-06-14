using UnityEngine;

public class Room : MonoBehaviour
{
    [System.Serializable]
    public struct RoomNameData
    {
        public string englishName;
        public string indonesianName;
    }
    public RoomNameData roomName;
    public int totalTasks = 0;
    int completedTasks = 0;
    bool isCompleted = false;

    public string GetLocalizedRoomName()
    {
        return LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? roomName.englishName
            : roomName.indonesianName;
    }

    public void OnTaskCompleted()
    {
        if (completedTasks < totalTasks)
        {
            completedTasks++;
            if (completedTasks >= totalTasks && !isCompleted)
            {
                isCompleted = true;
                TaskManager.Instance.OnRoomCompleted();
            }
        }
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public int GetCompletedTasks()
    {
        return completedTasks;
    }

    public int GetTotalTasks()
    {
        return totalTasks;
    }

    void OnTriggerEnter(Collider other)
    {
        if (TaskManager.Instance.Player != null && other.gameObject == TaskManager.Instance.Player)
        {
            Room room = GetComponent<Room>();
            TaskManager.Instance.SetCurrentRoom(room);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (TaskManager.Instance.Player != null && other.gameObject == TaskManager.Instance.Player)
        {
            TaskManager.Instance.SetCurrentRoom(null);
        }
    }
}
