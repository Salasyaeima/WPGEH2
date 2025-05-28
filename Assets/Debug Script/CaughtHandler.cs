using UnityEngine;
using UnityEngine.SceneManagement;

public class CaughtHandler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera playerCameraJumpScare;
    Animator jumpscareAnimator;
    [SerializeField] CameraJumpScare cameraJumpScare;
    [SerializeField] Broom broom;

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
        playerCamera.enabled = false;
        playerCameraJumpScare.enabled = true;
        jumpscareAnimator.Play("Scene");
        cameraJumpScare.StartShakeAndZoom();
        // AudioManager.instance.StopAllAudio();
    }
}
