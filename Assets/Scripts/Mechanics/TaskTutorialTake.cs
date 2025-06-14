using TMPro;
using UnityEngine;

public class TaskTutorialTake : Interactable
{
    [SerializeField] GameObject taskList;
    [SerializeField] GameObject windowQuest;
    [SerializeField] TextDialogChild taskTutorial;
    [SerializeField] string interectionSFXName = "Ambilbarang";

    public bool isDone = false;

    public override string Description()
    {
        if (isDone == false)
        {
            return  LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Press {E} to take" :  "Tekan {E} untuk mengambil";
        }
        else
        {
            return " ";
        }
    }

    public override void Interact()
    {
        AudioManager.instance.PlaySFX(interectionSFXName, 0.3f);
        isDone = true;
        taskList.SetActive(false);
        windowQuest.SetActive(false);
        taskTutorial.intruksi.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Press [T] to view tasks": "Tekan [T] untuk melihat tugas";
        taskTutorial.playerInteractions.canInteract = false;
    }
}
