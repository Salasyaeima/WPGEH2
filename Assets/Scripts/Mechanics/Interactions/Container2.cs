using UnityEngine;
using System.Collections.Generic;

public class Container2 : Interactable, ITaskProvider
{
    public GameObject emptyContainer;
    public GameObject fullContainer;
    [SerializeField] string interectionHoldSFXName = "Cloth_sound";
    TaskManager taskManager;
    Room room;
    bool isDone = false;
    bool isHolding = false;


    public enum ContainerType
    {
        toyContainer,
        Bookshelf,
        wardrobe,
        bed
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
            taskManager.RegisterTask(GetTaskName(), this, room);
        }
    }

    public override string Description()
    {
        if (isDone == false)
        {
            return  LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Hold {E} Interact." :"Tahan {E} Berinteraksi";
        }
        else
        {
            return " ";
        }
    }

    public override void Interact()
    {
        isDone = true;

    }

    void Update()
    {
        if (isDone == true)
        {
            emptyContainer.SetActive(false);
            fullContainer.SetActive(true);

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

    void Destroy()
    {
        Destroy(emptyContainer);
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

    public string GetTaskName()
    {
        bool isIndonesian = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.Indonesian;
        switch (containerType)
        {
            case ContainerType.Bookshelf:
                return isIndonesian ? "Susun buku di meja belajar" : "Arrange books on the study table";
            case ContainerType.bed:
                return isIndonesian ? "Rapihkan tempat tidur" : "Make the bed";
            default:
                return "";
        }
    }

    public override void OnHoldStart()
    {
        if (!isHolding)
        {
            isHolding = true;
            AudioManager.instance.PlayLoopingSFX(interectionHoldSFXName);
        }
    }

    public override void OnHoldEnd()
    {
        isHolding = false;
        AudioManager.instance.StopLoopingSFX(interectionHoldSFXName);

    }
}