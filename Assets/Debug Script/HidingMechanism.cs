using UnityEngine;
using Cinemachine;
using System.Collections;

public class HidingMechanism : Interactable
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CinemachineVirtualCamera playersCamera;    
    private CinemachineBrain cameraBrain;
    private CinemachineVirtualCamera thisCamera;
    private bool isHiding;
    private Vector3 playersLastPos;
    private LineOfSight lineOfSight;

    void Start()
    {
        cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        thisCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        lineOfSight = player.GetComponent<LineOfSight>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isHiding)
        {
            PerformHide(true, playersCamera, thisCamera);
            isHiding = false;
        }
    }

    public override string Description()
    {
        return "Press {E} to interact.";
    }

    public override void Interact()
    {
        PerformHide(false, thisCamera, playersCamera);
        isHiding = true;
    }

    private void PerformHide(bool condition, CinemachineVirtualCamera camera1, CinemachineVirtualCamera camera2)
    {
        if (isHiding)
        {
            SetCameraPriority(camera1, camera2);
            player.transform.position = playersLastPos;
            playersLastPos = Vector3.zero;
            SwitchComponents(condition);
        }
        else
        {
            SwitchComponents(condition);
            playersLastPos = player.transform.position;
            player.transform.position = this.transform.position;
            SetCameraPriority(camera1, camera2);
        }
    }

    private void SwitchComponents(bool condition)
    {
        MonoBehaviour[] components = player.GetComponentsInChildren<MonoBehaviour>();
        Collider[] colliders= player.GetComponentsInChildren<Collider>();
        Renderer renderer = player.GetComponentInChildren<Renderer>();
        
        if (components != null){foreach (MonoBehaviour component in components) 
        {
            component.enabled = condition;
        }}

        if (colliders != null)
        {
            foreach (Collider collider in colliders)
            {
                collider.enabled = condition; 
            }
        }

        renderer.enabled = condition;
    }

    private void SetCameraPriority(CinemachineVirtualCamera camera1, CinemachineVirtualCamera camera2)
    {
        camera1.Priority = 20;
        camera2.Priority = 10;
    }
}
