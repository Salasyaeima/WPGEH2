using System.Collections;
using UnityEngine;

public class Door : Interactable
{
    public bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    bool isAnimating = false;

    [SerializeField] float openAngle = 90f;
    [SerializeField] float animationDuration = 1f;
    [SerializeField] string openDoorSFX = "Buka_Pintu";
    [SerializeField] string closeDoorSFX = "Tutup_Pintu";

    Collider door;

    void Start()
    {
        door = GetComponentInChildren<Collider>();
        closedRotation = door.transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    public override string Description()
    {
        return LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
            ? (!isOpen ? "Press {E} to open." : "Press {E} to close.")
            : (!isOpen ? "Tekan {E} untuk membuka." : "Tekan {E} untuk menutup.");
    }

    public override void Interact()
    {
        if (!isAnimating)
        {
            if (isOpen)
            {
                AudioManager.instance.PlaySFX(closeDoorSFX, 0.5f);
            }
            else
            {
                AudioManager.instance.PlaySFX(openDoorSFX, 0.5f);
            }
            StartCoroutine(AnimateDoor());
        }
    }

    public void OpenAutomatically()
    {
        if (!isOpen && !isAnimating)
        {
            StartCoroutine(AnimateDoor());
        }
    }


    public IEnumerator AnimateDoor()
    {
        isAnimating = true;
        float elapsed = 0f;
        Quaternion startRotation = door.transform.rotation;
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            door.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / animationDuration);
            yield return null;
        }

        door.transform.rotation = targetRotation;
        isOpen = !isOpen;
        isAnimating = false;
    }
}