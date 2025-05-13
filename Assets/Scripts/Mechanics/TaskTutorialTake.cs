using TMPro;
using UnityEngine;

public class TaskTutorialTake : Interactable
{
    [SerializeField] GameObject taskList;
    [SerializeField] GameObject windowQuest;
    [SerializeField] TextDialogChild taskTutorial;
    public bool isDone = false;

    public override string Description()
    {
        if (isDone == false)
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
        isDone = true;
        taskList.SetActive(false);
        windowQuest.SetActive(false);
        taskTutorial.intruksi.text = "Tekan [T] untuk melihat tugas";
        taskTutorial.playerInteractions.canInteract = false;
    }
}
