using UnityEngine;
using Cinemachine;
using Unity.Behavior;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using StarterAssets;
using Unity.AppUI.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HidingMechanism : Interactable
{
    [Header("Script Refrence")]
    [SerializeField] StarterAssetsInputs starterAssetsInputs;
    [Header("Define the KeyCode for interactions")]
    public string keyCode;
    [SerializeField]
    public bool isHiding;
    [SerializeField] Slider energi;
    [SerializeField] CanvasGroup energiTransparant;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CinemachineVirtualCamera playersCamera;
    [SerializeField]
    private BehaviorGraph behavior;
    [SerializeField]
    private List<GameObject> models;
    private CinemachineVirtualCamera thisCamera;
    [SerializeField]
    private Vector3 outLocation;
    [SerializeField]
    private float timer;
    [SerializeField]
    private float coolDown;
    public bool isCoolDown;
    [SerializeField] string enterBox = "Masuk_Box";
    [SerializeField] string exitBox = "Keluar_Box";
    bool isInTutorialScene;


    void Start()
    {
        thisCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        SetCameraPriority(playersCamera, thisCamera);
        SetActiveModels(true, false);

        isInTutorialScene = SceneManager.GetActiveScene().name == "RoomsTutorial";
    }

    void Update()
    {
        if (isHiding)
        {
            timer += Time.deltaTime;
        }
        OnButtonCooldown();

        if (!isInTutorialScene || !isHiding)
        {
            Hide();
        }
    }

    public override string Description()
    {
        if (isHiding)
        {
            if (isInTutorialScene)
            {
                return string.Empty; 
            }
            string key = keyCode.ToUpper();
            string text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? $"Press {{{key}}} to exit."
                : $"Tekan {{{key}}} untuk keluar.";
            return text;
        }
        else
        {
            string key = keyCode.ToUpper();
            string text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English
                ? $"Press {{{key}}} to hide."
                : $"Tekan {{{key}}} untuk sembunyi.";
            return text;
        }
    }

    public override void Interact()
    {
        if (!isHiding)
        {
            isHiding = true;
            starterAssetsInputs.move = Vector2.zero;
            starterAssetsInputs.sprint = false;
            energiTransparant.alpha = 0;
            NotHiddenWhenChased();
            PerformHide(isHiding);
        }

    }

    private void Hide()
    {
        if (!isCoolDown && !isInTutorialScene)
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
            AudioManager.instance.PlaySFX(enterBox);
        }
        else
        {
            ExitHide();
            AudioManager.instance.PlaySFX(exitBox);
        }
        SwitchComponents(!condition);
    }

    private void ExitHide()
    {
        SetActiveModels(true, false);
        SetCameraPriority(playersCamera, thisCamera);

        player.transform.position = new Vector3(this.transform.position.x + outLocation.x, this.transform.position.y + outLocation.y, this.transform.position.z + outLocation.z);
        player.transform.rotation = Quaternion.LookRotation(this.transform.forward);
    }

    public void EnterHide()
    {
        SetActiveModels(false, true);
        SetCameraPriority(thisCamera, playersCamera);

        player.transform.position = this.transform.position;
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
            gameObject.layer = LayerMask.NameToLayer("Player");
            behavior.BlackboardReference.SetVariableValue<GameObject>("Target", this.gameObject);
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            behavior.BlackboardReference.SetVariableValue<GameObject>("Target", player);
        }
    }

    private void OnButtonCooldown()
    {
        if (Input.GetKeyDown(keyCode) && timer >= coolDown)
        {
            isCoolDown = false;
            timer = 0f;
            return;
        }
        isCoolDown = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(this.transform.position.x + outLocation.x, this.transform.position.y + outLocation.y, this.transform.position.z + outLocation.z), 1);
    }
}