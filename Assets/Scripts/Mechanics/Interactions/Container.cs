using UnityEngine;
using System.Collections.Generic;

public class Container : Interactable, ITaskProvider
{
    public Transform spawnPoint;
    public GameObject baju;
    public GameObject emptyContainer;
    public GameObject fullContainer;
    public List<GameObject> storedItems = new List<GameObject>();
    public int maxCapacity = 2;
    TaskManager taskManager;
    private int count = 0;
    Room room;

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
        room = GetComponentInParent<Room>();
        if (room == null)
        {
            Debug.LogWarning($"{name} tidak menemukan Room di parent!");
        }

        if (taskManager != null)
        {
            taskManager.RegisterTask(GetBaseTaskName(), this, room); // Pake base name
        }
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
        Transform clothesTransform = PlayerInteractions.heldItem.transform.Find("Kain");
        Renderer clothesRenderer = clothesTransform.GetComponent<Renderer>();
        Color itemColor = clothesRenderer.material.color;

        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, count * 0.5f);
        GameObject spawnedClothes = Instantiate(baju, spawnPosition, Quaternion.identity);
        Transform spawnedClothesTransform = spawnedClothes.transform.Find("Kain");

        if (spawnedClothesTransform != null)
        {
            Renderer spawnedClothesRenderer = spawnedClothesTransform.GetComponent<Renderer>();
            if (spawnedClothesRenderer != null)
            {
                spawnedClothesRenderer.material.mainTexture = clothesRenderer.material.mainTexture;
                spawnedClothesRenderer.material.color = clothesRenderer.material.color;
            }
        }

        count++;
        Destroy(PlayerInteractions.heldItem.gameObject);
    }

    void Collecting()
    {
        ItemData itemData = PlayerInteractions.heldItem.GetComponent<ItemData>();
        if (storedItems.Count < maxCapacity)
        {
            if ((containerType == ContainerType.toyContainer && itemData.category == ItemData.ItemCategory.Toy))
            {
                MoveItem();
                storedItems.Add(PlayerInteractions.heldItem.gameObject);
                PlayerInteractions.heldItem = null;
            }
            else if ((containerType == ContainerType.wardrobe && itemData.category == ItemData.ItemCategory.Clothes))
            {
                SpawnItem();
                storedItems.Add(PlayerInteractions.heldItem.gameObject);
                PlayerInteractions.heldItem = null;
            }
            else
            {
                Debug.Log("Mending rakit pc!!!");
            }
        }
    }

    void MoveItem()
    {
        Rigidbody rb = PlayerInteractions.heldItem.GetComponent<Rigidbody>();
        Collider itemCollider = PlayerInteractions.heldItem.GetComponent<Collider>();

        PlayerInteractions.heldItem.transform.SetParent(null);
        PlayerInteractions.heldItem.transform.SetParent(spawnPoint);
        itemCollider.enabled = true;
        PlayerInteractions.heldItem.transform.position = spawnPoint.position;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void Update()
    {
        if (storedItems.Count == maxCapacity)
        {
            emptyContainer.SetActive(false);
            fullContainer.SetActive(true);

            if (taskManager != null)
            {
                string taskToComplete = GetBaseTaskName();
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

    public string GetBaseTaskName()
    {
        switch (containerType)
        {
            case ContainerType.toyContainer:
                return "Masukkan barang ke dalam kotak mainan";
            case ContainerType.wardrobe:
                return "Simpan pakaian di dalam lemari";
            default:
                return "";
        }
    }

    public string GetTaskName()
    {
        return $"{GetBaseTaskName()} ({storedItems.Count}/{maxCapacity})";
    }
}