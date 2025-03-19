using UnityEngine;
using System.Collections.Generic;


public class Container : Interactable
{
    public Transform spawnPointWardrobe;
    public GameObject baju;
    public GameObject emptyContainer;
    public GameObject fullContainer;
    public List<GameObject> storedItems = new List<GameObject>();
    public int maxCapacity = 1;
    TaskManager taskManager;
    private int count = 0;

    public enum ContainerType
    {
        toyContainer,
        Bookshelf,
        wardrobe
    }

    public ContainerType containerType;


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

    void SpawnItem()
    {
        Vector3 spawnPosition = spawnPointWardrobe.position + new Vector3(0, 0, count * 0.5f);
        Instantiate(baju, spawnPosition, Quaternion.identity);
        count++;
    }

    void Collecting()
    {
        if (storedItems.Count < maxCapacity)
        {
            ItemData itemData = PlayerInteractions.heldItem.GetComponent<ItemData>();
            if (itemData != null &&
            ((containerType == ContainerType.toyContainer && itemData.category == ItemData.ItemCategory.Toy)
            || (containerType == ContainerType.wardrobe && itemData.category == ItemData.ItemCategory.Clothes)
            || (containerType == ContainerType.Bookshelf && itemData.category == ItemData.ItemCategory.Book)))
            {
                SpawnItem();
                storedItems.Add(PlayerInteractions.heldItem.gameObject);
                Destroy(PlayerInteractions.heldItem.gameObject);   
                PlayerInteractions.heldItem = null;
            }

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
                string taskToComplete = GetTaskName();
                Task task = FindTaskByName(taskToComplete);
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

    string GetTaskName()
    {
        switch (containerType)
        {
            case ContainerType.toyContainer:
                return "Masukkan Item Ke Container Toy";
            case ContainerType.wardrobe:
                return "Masukkan Item Ke Container Clothes";
            case ContainerType.Bookshelf:
                return "Masukkan Item Ke Container Book";
            default:
                return "";
        }
    }

}
