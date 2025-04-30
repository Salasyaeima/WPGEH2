using UnityEngine;
using UnityEngine.SceneManagement;

public class CaughtHandler : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera playerCameraJumpScare;
    Animator jumpscareAnimator;
    [SerializeField] CameraJumpScare cameraJumpScare;

    void Start()
    {
        jumpscareAnimator = GetComponent<Animator>();
    }
    public void PerformCaught()
    {
        playerCamera.enabled = false;
        playerCameraJumpScare.enabled = true;
        jumpscareAnimator.Play("Scene");
        cameraJumpScare.StartShakeAndZoom();
    }
}
