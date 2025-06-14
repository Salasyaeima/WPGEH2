using System.Collections;
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
    [SerializeField] string interectionHoldSFXName = "Cloth_sound";

    bool isBed = false;
    public override string Description()
    {
        if (!isBed)
        {
            return  LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Press {E} to sleep" : "Tekan {E} untuk tidur";
        }
        else
        {
            return " ";
        }
    }

    public override void Interact()
    {
        AudioManager.instance.PlaySFX(interectionHoldSFXName);
        StartCoroutine(StopSoundAfterSeconds(3f));
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

    IEnumerator StopSoundAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AudioManager.instance.StopAllAudio();
    }

}
