using UnityEngine;
using Cinemachine;
using Unity.Behavior;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
public class HidingMechanism : Interactable
{
    [Header("Define the KeyCode for interactions")]
    public string keyCode;
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
    [SerializeField]
    private float timer;
    [SerializeField]
    private float coolDown;
    public bool isCoolDown;
    private bool isHiding;
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
        if (isHiding)
        {
            timer += Time.deltaTime;
        }
        OnButtonCooldown();
        Hide();
    }

    public override string Description()
    {
        if (isHiding)
        {
            return " ";
        }
        else
        {
            return "Tekan {" + keyCode.ToUpper() + "} untuk berinteraksi.";
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

    private void Hide()
    {
        if (!isCoolDown)
        {
            if (isHiding)
            {
                isHiding = false;
                NotHiddenWhenChased();
                PerformHide(isHiding);
            }
        }
    }

    private void PerformHide(bool condition)
    {
        if (isHiding)
        {
            EnterHide();
        }
        else
        {
            ExitHide();
        }
        SwitchComponents(!condition);
    }

    private void ExitHide()
    {
        SetActiveModels(true, false);
        SetCameraPriority(playersCamera, thisCamera);

        player.transform.position = new Vector3(this.transform.position.x, player.transform.position.y, this.transform.position.z - 3f);
        player.transform.rotation = Quaternion.LookRotation(this.transform.forward);
        Debug.Log("Exit Hide");
    }

    private void EnterHide()
    {
        SetActiveModels(false, true);
        SetCameraPriority(thisCamera, playersCamera);

        player.transform.position = this.transform.position;
        Debug.Log("Enter Hide");
    }

    private void SwitchComponents(bool condition)
    {
        player.SetActive(condition);
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
        }
        else
        {
            behavior.BlackboardReference.SetVariableValue<GameObject>("Target", player);
        }
    }

    private void OnButtonCooldown()
    {
        if(Input.GetKeyDown(keyCode) && timer >= coolDown)
        {
           isCoolDown = false;
           timer = 0f;
           return;
        }
        isCoolDown = true;
    }
}