using UnityEngine;
using System.Collections.Generic;


public class Container : Interactable
{
    public Transform containerSlot;
    public GameObject emptyContainer;
    public GameObject fullContainer;
    public List<GameObject> storedItems = new List<GameObject>();
    public int maxCapacity = 1;
    TaskManager taskManager;

    void Start()
    {
        taskManager = TaskManager.Instance;
    }

    public override string Description()
    {
        if (PlayerInteractions.heldItem != null)
        {
            return "Press {E} to interact.";
        }
        else
        {
            return " ";
        }
    }


    public override void Interact()
    {
        if (PlayerInteractions.heldItem != null)
        {
            Collecting();
        }
    }

    void Collecting()
    {
        if (storedItems.Count < maxCapacity)
        {
            PlayerInteractions.heldItem.transform.SetParent(containerSlot);
            PlayerInteractions.heldItem.transform.localPosition = new Vector3(0, storedItems.Count * 0.2f, 0); // Menyusun item ke atas
            storedItems.Add(PlayerInteractions.heldItem.gameObject);
            PlayerInteractions.heldItem = null;
        }
    }

    void Update()
    {
        if (storedItems.Count == maxCapacity)
        {
            emptyContainer.SetActive(false);
            Destroy(emptyContainer);
            fullContainer.SetActive(true);
            Debug.Log("Penuhh");

            if (taskManager != null)
            {
                Task task = FindTaskByName("Masukkan Item Ke Container1");
                if (task != null && !task.isCompleted)
                {
                    taskManager.CompleteTask(task);
                }
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
}
