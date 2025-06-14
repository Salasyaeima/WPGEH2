using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Container : Interactable, ITaskProvider
{
    public Transform spawnPoint;
    public GameObject prefab;
    public GameObject emptyContainer;
    public GameObject fullContainer;
    public List<GameObject> storedItems = new List<GameObject>();
    public int maxCapacity = 2;
    [SerializeField] string interectionSFXName = "Ambilbarang";

    TaskManager taskManager;
    private float count = 0f;
    Room room;

    public enum ContainerType
    {
        toyContainer,
        Bookshelf,
        wardrobe,
        gudang
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

            if (SceneManager.GetActiveScene().name == "Rooms" && containerType == ContainerType.wardrobe)
            {
                storedItems.Clear();
                for (int i = 0; i < maxCapacity; i++)
                {
                    storedItems.Add(null);
                }
                emptyContainer.SetActive(false);
                fullContainer.SetActive(true);
                string taskToComplete = GetBaseTaskName();
                Task task = FindTaskByName(taskToComplete);
                if (task != null && !task.isCompleted)
                {
                    taskManager.CompleteTask(task, playSound: false);
                }
            }
        }
    }

    public override string Description()
    {
        if (PlayerInteractions.heldItem != null)
        {
            return LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Press {E} to interact" : "Tekan {E} untuk berinteraksi";
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

    void PlaceClothes()
    {
        
        Transform clothesTransform = PlayerInteractions.heldItem.transform.Find("Kain");
        Renderer clothesRenderer = clothesTransform.GetComponent<Renderer>();
        Color itemColor = clothesRenderer.material.color;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, count * 0.5f);
        GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
        
        Transform spawnedPrefabTransform = spawnedPrefab.transform.Find("Kain");

        if (spawnedPrefabTransform != null)
        {
            Renderer spawnedPrefabRenderer = spawnedPrefabTransform.GetComponent<Renderer>();
            if (spawnedPrefabRenderer != null)
            {
                spawnedPrefabRenderer.material.mainTexture = clothesRenderer.material.mainTexture;
                spawnedPrefabRenderer.material.color = clothesRenderer.material.color;
            }
        }

        count++;
        Destroy(PlayerInteractions.heldItem.gameObject);
    }

    void SpawnItem()
    {
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, count);
        GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
        spawnedPrefab.transform.SetParent(spawnPoint);
        count+=2.7f;
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
                PlayCollectSound();
            }
            else if ((containerType == ContainerType.wardrobe && itemData.category == ItemData.ItemCategory.Clothes))
            {
                PlaceClothes();
                storedItems.Add(PlayerInteractions.heldItem.gameObject);
                PlayerInteractions.heldItem = null;
                PlayCollectSound();
            }
            else if ((containerType == ContainerType.gudang && itemData.category == ItemData.ItemCategory.Box))
            {
                SpawnItem();
                storedItems.Add(PlayerInteractions.heldItem.gameObject.gameObject);
                PlayerInteractions.heldItem = null;
                PlayCollectSound();
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

    void PlayCollectSound()
    {
        AudioManager.instance.PlaySFX(interectionSFXName, 0.3f);
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
        bool isIndonesian = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.Indonesian;
        switch (containerType)
        {
            case ContainerType.toyContainer:
                return isIndonesian ? "Masukkan barang ke dalam kotak mainan" : "Put items in the toy box";
            case ContainerType.wardrobe:
                return isIndonesian ? "Simpan pakaian di dalam lemari" : "Store clothes in the wardrobe";
            case ContainerType.gudang:
               return isIndonesian ? "Masukkan kardus ke rak" : "Place boxes on the shelf";
            default:
                return "";
        }
    }

    public string GetTaskName()
    {
        return $"{GetBaseTaskName()} ({storedItems.Count}/{maxCapacity})";
    }
}