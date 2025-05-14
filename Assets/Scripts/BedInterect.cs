using UnityEngine;

public class BedInterect : Interactable
{
    [SerializeField] GameObject cameraSleep;
    [SerializeField] SleepyBlinkEffect sleepyBlinkEffect;
    bool isBed = false;
    public override string Description()
    {
        if (!isBed)
        {
            return "Press {E} to sleep.";
        }
        else
        {
            return " ";
        }
    }

    public override void Interact()
    {
        if (cameraSleep != null)
        {
            cameraSleep.SetActive(true);
        }
        if (sleepyBlinkEffect != null)
        {
            sleepyBlinkEffect.StartFadeToBlack();
            sleepyBlinkEffect.textDialogChild.intruksi.enabled = false;
            sleepyBlinkEffect.textDialogChild.windowQuest.gameObject.SetActive(false);
        }
        isBed = true;
    }
}
