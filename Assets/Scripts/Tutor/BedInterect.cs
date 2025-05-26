using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class BedInterect : Interactable
{
    [SerializeField] StarterAssetsInputs starterAssetsInputs;
    [SerializeField] Slider energi;
    [SerializeField] CanvasGroup energiTransparant;
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
        starterAssetsInputs.move = Vector2.zero;
        starterAssetsInputs.sprint = false;
        energiTransparant.alpha = 0;
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
