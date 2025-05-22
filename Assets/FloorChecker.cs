using System.Collections;
using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    [SerializeField] BoxCollider floorChecker;
    [SerializeField] LayerMask dirtLayer;
    [SerializeField] Broom broom;
    TaskManager taskManager;
    bool isDone = false;

    void Start()
    {
        taskManager = TaskManager.Instance;
        StartCoroutine(CheckFloorRoutine());
    }

    public void CheckFloor()
    {
        if (isDone || floorChecker == null) return;

        Collider[] dirtColliders = Physics.OverlapBox(
            floorChecker.bounds.center,
            floorChecker.bounds.extents,
            floorChecker.transform.rotation,
            dirtLayer
        );

        if (dirtColliders.Length == 0)
        {

            isDone = true;
            Task task = FindTaskByName(broom.GetTaskName());
            if (task != null && !task.isCompleted)
            {
                taskManager.CompleteTask(task);
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

    IEnumerator CheckFloorRoutine()
    {
        while (!isDone)
        {
            CheckFloor();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
