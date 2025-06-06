using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using StarterAssets;

public class CaughtHandler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera playerCameraJumpScare;
    Animator jumpscareAnimator;
    [SerializeField] CameraJumpScare cameraJumpScare;
    [SerializeField] Broom broom;
    [SerializeField] PlayerInteractions playerInteractions;
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] TMP_Text interactionsText;
    [SerializeField] GameObject progression;

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
        progression.SetActive(false);
        playerInteractions.enabled = false;
        firstPersonController.DisableInput();
        interactionsText.gameObject.SetActive(false);
        playerCamera.enabled = false;
        playerCameraJumpScare.enabled = true;
        jumpscareAnimator.Play("Scene");
        cameraJumpScare.StartShakeAndZoom();
        AudioManager.instance.StopAllAudio();
    }
}
