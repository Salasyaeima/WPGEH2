using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CaughtHandler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera playerCameraJumpScare;
    Animator jumpscareAnimator;
    [SerializeField] CameraJumpScare cameraJumpScare;
    [SerializeField] Broom broom;
    [SerializeField] PlayerInteractions playerInteractions;
    [SerializeField] TMP_Text interactionsText;

    void Start()
    {
        jumpscareAnimator = GetComponent<Animator>();
    }
    public void PerformCaught()
    {
        if (broom.isHeld == true)
        {
            broom.Drop();
        }
        playerInteractions.enabled = false;
        interactionsText.gameObject.SetActive(false);
        playerCamera.enabled = false;
        playerCameraJumpScare.enabled = true;
        jumpscareAnimator.Play("Scene");
        cameraJumpScare.StartShakeAndZoom();
    }
}
