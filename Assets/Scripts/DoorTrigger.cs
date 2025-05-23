using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
        bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    bool isAnimating = false;

    [SerializeField] float openAngle;
    [SerializeField] float animationDuration = 1f;
    [Header("The Center of Rotation")]
    [SerializeField] GameObject doorEngsel;
    // [Header("Trigger Collider Settings")]
    // [SerializeField] Vector3 colliderCenter;
    // [SerializeField] Vector3 colliderSizing;
    // [SerializeField] LayerMask detectionLayer;

    void Start()
    {
        closedRotation = doorEngsel.transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        // CheckDoorTriggerBox();
        Debug.Log(isOpen);
    }

    // void CheckDoorTriggerBox()
    // {
    //     Collider[] boxColliders = Physics.OverlapBox(transform.position + colliderCenter, colliderSizing / 2, transform.rotation, detectionLayer);
    //     List<Collider> trackedColliders = new List<Collider>();
    //     string state = null;

    //     if (boxColliders.Length > 0)
    //     {
    //         foreach (Collider collider in boxColliders)
    //         {
    //             if (!trackedColliders.Contains(collider))
    //             {
    //                 Debug.Log("Masuk");
    //                 trackedColliders.Add(collider);
    //             }
    //         }

    //         foreach (Collider collider in trackedColliders)
    //         {
    //             if (boxColliders.Contains(collider))
    //             {
    //                 if (!isOpen)
    //                 {
    //                     Debug.Log("Stay");
    //                     StartCoroutine(AnimateDoor());
    //                 }
    //                 Debug.Log("Stay");
    //             }
    //         }

    //         for (int i = 0; i < trackedColliders.Count; i++)
    //         {
    //             Debug.Log(trackedColliders[i].gameObject);
    //             if (!boxColliders.Contains(trackedColliders[i]))
    //             {

    //                 Debug.Log("Keluar");
    //                 // trackedColliders.Remove(collider);
    //             }
    //         }

    //         // for (int i = trackedColliders.Count - 1; i >= 0; i--)
    //         // {
    //         //     if (!boxColliders.Contains(trackedColliders[i]))
    //         //     {
    //         //         if (isOpen)
    //         //         {
    //         //             StartCoroutine(AnimateDoor());
    //         //         }
    //         //         Debug.Log("Keluar");
    //         //         trackedColliders.RemoveAt(i);
    //         //     }
    //         // }
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        StartCoroutine(AnimateDoor());
    }

    void OnTriggerExit(Collider other)
    {
        if (isOpen) 
            StartCoroutine(AnimateDoor());
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
        // Gizmos.DrawWireCube(transform.position + colliderCenter, colliderSizing); 
    }
}
