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
    [Header("The Center of Rotation")]
    [SerializeField] GameObject doorEngsel;
    [Header("Trigger Collider Settings")]
    [SerializeField] Vector3 colliderCenter;
    [SerializeField] Vector3 colliderSizing;
    [SerializeField] LayerMask detectionLayer;

    private Collider currentOverlap;
    private Collider previousOverlap;
 
    void Start()
    {
        closedRotation = doorEngsel.transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        CheckDoorTriggerBox();
    }

    void CheckDoorTriggerBox()
    {
        Collider[] boxColliders = Physics.OverlapBox(transform.position + colliderCenter, colliderSizing/2, transform.rotation, detectionLayer);
        currentOverlap = null;
        if (boxColliders.Length > 0)
        {
            currentOverlap = boxColliders[0];
            if (previousOverlap != currentOverlap)
            {
                if (!isAnimating)
                {
                    StartCoroutine(AnimateDoor());
                }
            }
            previousOverlap = currentOverlap;
            return;
        }
        previousOverlap = null;
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
        Gizmos.DrawWireCube(transform.position + colliderCenter, colliderSizing); 
    }
}
