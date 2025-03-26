using UnityEngine;
using System.Collections.Generic;


public class Container : Interactable
{
    public Transform spawnPoint;
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
        Transform kainTransform = PlayerInteractions.heldItem.transform.Find("Kain");
        Renderer kainRenderer = kainTransform.GetComponent<Renderer>();
        Color itemColor = kainRenderer.material.color;

        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, count * 0.5f);
        GameObject spawnedBaju = Instantiate(baju, spawnPosition, Quaternion.identity);
        Transform spawnedKainTransform = spawnedBaju.transform.Find("Kain");

         if (spawnedKainTransform != null)
        {
            Renderer spawnedKainRenderer = spawnedKainTransform.GetComponent<Renderer>();
            if (spawnedKainRenderer != null)
            {
                spawnedKainRenderer.material.mainTexture = kainRenderer.material.mainTexture;
                spawnedKainRenderer.material.color = kainRenderer.material.color;
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
            else if((containerType == ContainerType.wardrobe && itemData.category == ItemData.ItemCategory.Clothes))
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
