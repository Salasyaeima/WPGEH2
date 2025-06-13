using UnityEngine;

public class DoorCutscenee : MonoBehaviour
{
    [SerializeField] DoorTrigger doorTrigger;
    [SerializeField] string openDoorSFX = "Buka_Pintu";
    [SerializeField] string closeDoorSFX = "Tutup_Pintu";
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mother"))
        {
            AudioManager.instance.PlaySFX(openDoorSFX, 0.5f);
            StartCoroutine(doorTrigger.AnimateDoor());
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mother"))
        {
            StartCoroutine(doorTrigger.AnimateDoor());
            AudioManager.instance.PlaySFX(closeDoorSFX, 0.5f);
        }
    }
}
