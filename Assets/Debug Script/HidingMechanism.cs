using UnityEngine;
using Cinemachine;
using Unity.Behavior;
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
    [SerializeField]
    private BehaviorGraph behavior;
    [SerializeField]
    private List<GameObject> models;
    private CinemachineVirtualCamera thisCamera;
    private float timer;
    private bool coolDown;
    private bool isHiding;
    private Vector3 playersLastPos;
    private LineOfSight lineOfSight;

    void Start()
    {
        thisCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        lineOfSight = enemy.GetComponent<LineOfSight>();

        SetCameraPriority(playersCamera, thisCamera);
        SetActiveModels(true, false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        Debug.Log(playersLastPos);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHiding)
            {
                isHiding = false;
                NotHiddenWhenChased();
                PerformHide(isHiding);
            }
        }
    }

    public override string Description()
    {
        if (isHiding)
        {
            return " ";
        }else
        {
            return "Tekan {E} untuk berinteraksi.";
        }
    }

    public override void Interact()
    {
        if (!isHiding)
        {
            isHiding = true;
            NotHiddenWhenChased();
            PerformHide(isHiding);
        }
    }

    private void PerformHide(bool condition)
    {
        if (isHiding)
        {
            playersLastPos = player.transform.position;
            EnterHide();
        }else
        {
            ExitHide();
        }
        SwitchComponents(!condition);
    }

    private void ExitHide()
    {
        SetActiveModels(true, false);
        SetCameraPriority(playersCamera, thisCamera);

        player.transform.position = playersLastPos;
        Debug.Log("Exit Hide");
    }

    private void EnterHide()
    {
        // playersLastPos = player.transform.position;
        SetActiveModels(false, true);
        SetCameraPriority(thisCamera, playersCamera);

        player.transform.position = this.transform.position;
        Debug.Log("Enter Hide");
    }

    // IEnumerator EnableRendererAfterBlend()
    // {
    //     yield return new WaitForSeconds(cameraBrain.m_DefaultBlend.m_Time);
    //     Renderer renderer = player.GetComponentInChildren<Renderer>();
    //     if (renderer != null)
    //     {
    //         renderer.enabled = true;
    //     }
    // }

    private void SwitchComponents(bool condition)
    {
        player.SetActive(condition);
        // MonoBehaviour[] components = player.GetComponentsInChildren<MonoBehaviour>();
        // Collider[] colliders = player.GetComponentsInChildren<Collider>();
        // Renderer renderer = player.GetComponentInChildren<Renderer>();

        // if (components != null)
        // {
        //     foreach (MonoBehaviour component in components)
        //     {
        //         component.enabled = condition;
        //     }
        // }

        // if (colliders != null)
        // {
        //     foreach (Collider collider in colliders)
        //     {
        //         collider.enabled = condition;
        //     }
        // }

        // if (renderer != null)
        // {
        //     renderer.enabled = enableRenderer;
        // }
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
        if (player.CompareTag("isChased") && isHiding)
        {
            behavior.BlackboardReference.SetVariableValue<GameObject>("Target", this.gameObject);
        }else
        {
            behavior.BlackboardReference.SetVariableValue<GameObject>("Target", player);
        }
    }
}