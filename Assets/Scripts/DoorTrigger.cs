using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    bool isAnimating = false;

    [SerializeField] float openAngle = 90f;
    [SerializeField] float animationDuration = 1f;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, 90);
    }

    IEnumerator AnimateDoor()
    {
        isAnimating = true;
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / animationDuration);
            yield return null;
        }

        transform.rotation = targetRotation;
        isOpen = !isOpen;
        isAnimating = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mother") && !isOpen && !isAnimating)
        {
            StartCoroutine(AnimateDoor());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mother") && isOpen && !isAnimating)
        {
            StartCoroutine(AnimateDoor());
        }
    }
}
