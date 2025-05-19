using System.Collections;
using UnityEngine;

public class DoorCutscene : MonoBehaviour
{
    bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    bool isAnimating = false;
    [SerializeField] float openAngle;
    [SerializeField] float animationDuration = 1f;
    [SerializeField] GameObject doorEngsel;

    void Start()
    {
        closedRotation = doorEngsel.transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mother") && !isAnimating && !isOpen)
        {
            StartCoroutine(AnimateDoor(true));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mother") && !isAnimating && isOpen)
        {
            StartCoroutine(AnimateDoor(false));
        }
    }

    IEnumerator AnimateDoor(bool open)
    {
        isAnimating = true;
        float elapsed = 0f;
        Quaternion startRotation = doorEngsel.transform.rotation;
        Quaternion targetRotation = open ? openRotation : closedRotation;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            doorEngsel.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / animationDuration);
            yield return null;
        }

        doorEngsel.transform.rotation = targetRotation;
        isOpen = open;
        isAnimating = false;
    }
}
