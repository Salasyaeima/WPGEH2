using System.Collections;
using UnityEngine;

public class Door : Interactable
{
    bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    bool isAnimating = false;

    [SerializeField] float openAngle = 90f;
    [SerializeField] float animationDuration = 1f;
    Collider door;

    void Start()
    {
        door = GetComponentInChildren<Collider>();
        closedRotation = door.transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    public override string Description()
    {
        if (!isOpen)
        {
            return "Press {E} to open the door.";
        }
        else
        {
            return "Press {E} to close the door.";
        }
    }

    public override void Interact()
    {
        if (!isAnimating)
        {
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


    IEnumerator AnimateDoor()
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