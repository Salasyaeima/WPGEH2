using UnityEngine;

public class DoorCutscenee : MonoBehaviour
{
    [SerializeField] DoorTrigger doorTrigger;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mother"))
        {
            StartCoroutine(doorTrigger.AnimateDoor());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Mother"))
        {
            StartCoroutine(doorTrigger.AnimateDoor());
        }
    }
}
