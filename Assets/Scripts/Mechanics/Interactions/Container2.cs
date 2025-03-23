using UnityEngine;
using System.Collections.Generic;


public class Container2 : Interactable
{
    public GameObject emptyContainer;
    public GameObject fullContainer;
    TaskManager taskManager;

    bool isDone = false;

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
    }

    public override string Description()
    {
        if (isDone == false)
        {
            return "Hold {E} to interact.";
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
