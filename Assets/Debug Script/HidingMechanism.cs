using UnityEngine;

using Unity.Behavior;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
public class HidingMechanism : Interactable
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private CinemachineVirtualCamera playersCamera;
    private CinemachineBrain cameraBrain;
    private CinemachineVirtualCamera thisCamera;
    [SerializeField]
    private List<GameObject> models;
    private bool isHiding;
    private Vector3 playersLastPos;
    private LineOfSight lineOfSight;

    void Start()
    {
        cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        thisCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        SetActiveModels(true, false);
        lineOfSight = enemy.GetComponent<LineOfSight>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isHiding)
        {
            ExitHide();
        }
        Debug.Log(isHiding);
        // NotHiddenWhenChased();
    }

    public override string Description()
    {
        return "Tekan {E} untuk berinteraksi.";
    }

    public override void Interact()
    {
        SetActiveModels(false, true);
        PerformHide(false, thisCamera, playersCamera);
        isHiding = true;
    }

    private void PerformHide(bool condition, CinemachineVirtualCamera camera1, CinemachineVirtualCamera camera2)
    {
        if (isHiding)
        {
            SetCameraPriority(camera1, camera2);
        }
        else
        {
            SwitchComponents(condition, false);
            playersLastPos = player.transform.position;
            player.transform.position = this.transform.position;
            SetCameraPriority(camera1, camera2);
        }
    }

    private void ExitHide()
    {
        SetActiveModels(true, false);
        SetCameraPriority(playersCamera, thisCamera);

        player.transform.position = playersLastPos;
        SwitchComponents(true, false);

        isHiding = false;
        StartCoroutine(EnableRendererAfterBlend());
    }

    IEnumerator EnableRendererAfterBlend()
    {
        yield return new WaitForSeconds(cameraBrain.m_DefaultBlend.m_Time);
        Renderer renderer = player.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }
    }

    private void SwitchComponents(bool condition, bool enableRenderer)
    {
        MonoBehaviour[] components = player.GetComponentsInChildren<MonoBehaviour>();
        Collider[] colliders = player.GetComponentsInChildren<Collider>();
        Renderer renderer = player.GetComponentInChildren<Renderer>();

        if (components != null)
        {
            foreach (MonoBehaviour component in components)
            {
                component.enabled = condition;
            }
        }

        if (colliders != null)
        {
            foreach (Collider collider in colliders)
            {
                collider.enabled = condition;
            }
        }

        if (renderer != null)
        {
            renderer.enabled = enableRenderer;
        }
    }

    private void SetCameraPriority(CinemachineVirtualCamera camera1, CinemachineVirtualCamera camera2)
    {
        camera1.Priority = 20;
        camera2.Priority = 10;
    }

    private void SetActiveModels(bool bool1, bool bool2)
    {
        models[0].SetActive(bool1);
        models[1].SetActive(bool2);
    }

    private void NotHiddenWhenChased()
    {
        if (lineOfSight.DetectedTarget != null)
        {
            if (lineOfSight.DetectedTarget.CompareTag(lineOfSight.tagAfter) && isHiding)
            {
                Debug.Log("Test");
            }    
        }
        
    }
}