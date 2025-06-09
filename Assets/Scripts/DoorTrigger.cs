using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    bool isAnimating;

    [SerializeField] float openAngle;
    [SerializeField] float animationDuration = 1f;
    [Header("The Center of Rotation")]
    [SerializeField] GameObject doorEngsel;
    [SerializeField] string openDoorSFX = "Buka_Pintu";
    [SerializeField] string closeDoorSFX = "Tutup_Pintu";


    void Start()
    {
        closedRotation = doorEngsel.transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            AudioManager.instance.PlaySFX(openDoorSFX, 0.10f);
            StartCoroutine(AnimateDoor());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isOpen)
        {
            AudioManager.instance.PlaySFX(closeDoorSFX, 0.10f);
            StartCoroutine(AnimateDoor());

        }
    }

    IEnumerator AnimateDoor()
    {
        isAnimating = true;
        float elapsed = 0f;
        Quaternion startRotation = doorEngsel.transform.rotation;
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            doorEngsel.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / animationDuration);
            yield return null;
        }

        doorEngsel.transform.rotation = targetRotation;
        isOpen = !isOpen;
        isAnimating = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
    }
}
