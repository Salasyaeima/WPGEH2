using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomName;
    public int totalTasks = 0;
    int completedTasks = 0;
    bool isCompleted = false;

    public void OnTaskCompleted()
    {
        if (completedTasks < totalTasks)
        {
            completedTasks++;
            Debug.Log($"Task selesai di {roomName}: {completedTasks}/{totalTasks}");
            if (completedTasks >= totalTasks && !isCompleted)
            {
                isCompleted = true;
                TaskManager.Instance.OnRoomCompleted();
                Debug.Log($"{roomName} selesai!");
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
