using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    [SerializeField] string DirtTag = "Dirt";
    [SerializeField] Broom broom;
    TaskManager taskManager;
    bool isDone = false;
    Collider areaCollider;

    void Start()
    {
        taskManager = TaskManager.Instance;
        areaCollider = GetComponent<Collider>();
        if (areaCollider == null)
        {
            Debug.LogError("FloorChecker perlu Collider!");
        }
    }

    void Update()
    {
        if (isDone) return;

        Collider[] colliders = Physics.OverlapBox(areaCollider.bounds.center, areaCollider.bounds.extents, Quaternion.identity);
        bool hasDirt = false;
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(DirtTag))
            {
                hasDirt = true;
                break;
            }
        }

        if (!hasDirt)
        {
            Debug.Log("Semua kotoran di ruangan ini sudah bersih!");
            isDone = true;
            Task task = FindTaskByName(broom.GetTaskName());
            if (task != null && !task.isCompleted)
            {
                taskManager.CompleteTask(task);
            }
            else
            {
                Debug.LogWarning($"Task '{broom.GetTaskName()}' tidak ditemukan atau sudah selesai!");
            }
        }
    }

    Task FindTaskByName(string name)
    {
        foreach (Task task in taskManager.tasks)
        {
            if (task.taskName == name)
                return task;
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size);
    }
}