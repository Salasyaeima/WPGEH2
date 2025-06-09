using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TargetWalk : MonoBehaviour
{
    [System.Serializable]
    public class WaypointData
    {
        public Transform waypoint;
        public string animationToPlay = "Idle";
        public bool stopAutoMove;
        public bool muteVideo = true;
    }

    [SerializeField] Transform mother;
    [SerializeField] GameObject anak;
    [SerializeField] WaypointData[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] TextDisplayManager textDisplayManager;
    [SerializeField] GameObject pickupItem;
    [SerializeField] Transform handBone;
    [SerializeField] Transform lookTarget;
    [SerializeField] GameObject modelUpdate;
    [SerializeField] GameObject modelMarah;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] AudioSource videoPlayerAudio;
    [SerializeField] Transform HandPhone;
    [SerializeField] string footstepSFXName = "Footstep";
    [SerializeField] float footstepInterval = 0.5f;
    [SerializeField] string marahSFXName = "Marahibu";
    [SerializeField] string hdehSFXName = "HeIbu";



    public enum CharacterState { Idlee, Walking, LookingAround, Angry, PickingUp, Idleee }
    public CharacterState currentState = CharacterState.Idlee;
    public bool isMuted { get; private set; } = true;

    bool hasTriggeredHandPhoneTransform = false;
    Animator motherAnimator;
    int currentWaypoint = 0;
    int lastReachedWaypoint = -1;
    bool isMoving = false;
    bool autoMove = false;
    Coroutine pickupCoroutine;
    Coroutine footstepCoroutine;
    Vector3 targetPosition;

    static readonly int IdleeHash = Animator.StringToHash("Idlee");
    static readonly int WalkingHash = Animator.StringToHash("Walking");
    static readonly int LookingAroundHash = Animator.StringToHash("LookingAround");
    static readonly int AngryHash = Animator.StringToHash("Angry");
    static readonly int PickUpHash = Animator.StringToHash("Pick Up");
    static readonly int IdleeeHash = Animator.StringToHash("Idleee");

    void Awake()
    {
        ValidateReferences();
        motherAnimator = GetComponentInChildren<Animator>();
        if (mother == null)
        {
            Debug.LogError("Mother transform not assigned.");
            enabled = false;
        }
    }

    void OnDisable()
    {
        if (pickupCoroutine != null)
        {
            StopCoroutine(pickupCoroutine);
            pickupCoroutine = null;
        }
        StopAllCoroutines();
    }

    void Start()
    {
        isMoving = false;
        if (videoPlayer != null && videoPlayerAudio != null)
        {
            videoPlayer.Play(); 
            videoPlayer.SetDirectAudioMute(0, true);
            videoPlayerAudio.mute = true;
            videoPlayerAudio.volume = 0f;
        }
        SetCursorVisibility(false);
    }

    void FixedUpdate()
    {
        if (isMoving && currentState == CharacterState.Walking)
        {
            MoveToWaypoint();
            if (footstepCoroutine == null)
            {
                footstepCoroutine = StartCoroutine(PlayFootstepSound());
            }
        }
        else
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }
    }

    void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void ValidateReferences()
    {
        if (mother == null) Debug.LogError("Mother transform not assigned.");
        if (waypoints == null || waypoints.Length == 0) Debug.LogError("Waypoints array is empty or not assigned.");
        if (videoPlayer == null) Debug.LogWarning("VideoPlayer not assigned.");
        if (videoPlayerAudio == null) Debug.LogWarning("VideoPlayer AudioSource not assigned.");
        if (handBone == null) Debug.LogWarning("Hand bone not assigned for pickup.");
        if (modelUpdate == null || modelMarah == null) Debug.LogWarning("Model references not assigned.");
        if (HandPhone == null) Debug.LogWarning("HandPhone not assigned.");
    }

    void MoveToWaypoint()
    {
        if (currentWaypoint >= waypoints.Length) return;

        Vector3 direction = targetPosition - mother.position;
        float sqrDistance = direction.sqrMagnitude;

        if (sqrDistance < 0.01f)
        {
            isMoving = false;
            lastReachedWaypoint = currentWaypoint;
            HandleWaypointReached();
        }
        else
        {
            mother.position = Vector3.MoveTowards(mother.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
                mother.rotation = Quaternion.Slerp(mother.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void HandleWaypointReached()
    {
        if (lastReachedWaypoint == 8 && videoPlayer != null && videoPlayerAudio != null)
        {
            isMuted = false;
        }

        switch (lastReachedWaypoint)
        {
            case 6:
                SetState(CharacterState.LookingAround);
                StopAutoMove();
                break;
            case 7:
                SetState(CharacterState.Idlee);
                StopAutoMove();
                break;
            case 11:
                Vector3 lookDirection = lookTarget.position - mother.position;
                lookDirection.y = 0f;
                mother.rotation = Quaternion.LookRotation(lookDirection);
                SetState(CharacterState.Angry);
                pickupCoroutine = StartCoroutine(PickupAfterDelay(9f));
                StopAutoMove();
                break;
            case 13:
                if (currentWaypoint + 1 < waypoints.Length)
                {
                    currentWaypoint++;
                    StartMovingToWaypoint(currentWaypoint);
                }
                else
                {
                    Debug.LogWarning("No more waypoints to move to!");
                }
                break;
            default:
                if (autoMove && currentWaypoint + 1 < waypoints.Length)
                {
                    SetState(CharacterState.Walking);
                    currentWaypoint++;
                    StartMovingToWaypoint(currentWaypoint);
                }
                break;
        }
    }

    public void StartMovingToWaypoint(int waypointIndex = -1)
    {
        int targetWaypoint = waypointIndex >= 0 ? waypointIndex : currentWaypoint;
        if (targetWaypoint < 0 || targetWaypoint >= waypoints.Length)
        {
            Debug.LogError($"Waypoint index {targetWaypoint} out of range. Total waypoints: {waypoints.Length}");
            return;
        }

        currentWaypoint = targetWaypoint;
        targetPosition = waypoints[currentWaypoint].waypoint.position;
        isMoving = true;
        autoMove = true;

        SetState(CharacterState.Walking);

        if (textDisplayManager != null)
        {
            textDisplayManager.StopDisplayingText();
        }
    }

    public void StopAutoMove()
    {
        isMoving = false;
        autoMove = false;

        if (pickupCoroutine != null)
        {
            StopCoroutine(pickupCoroutine);
            pickupCoroutine = null;
        }

        if (lastReachedWaypoint == 6)
        {
            AudioManager.instance.PlaySFX(marahSFXName, 0.10f);
            SetState(CharacterState.LookingAround);
        }
        else if (lastReachedWaypoint == 7)
        {
            AudioManager.instance.PlaySFX(hdehSFXName, 0.10f);
            SetState(CharacterState.Idlee);
        }
        else if (lastReachedWaypoint == 11)
        {
            AudioManager.instance.PlaySFX(marahSFXName, 0.10f);
            Vector3 lookDirection = lookTarget.transform.position - mother.transform.position;
            lookDirection.y = 0f;
            mother.transform.rotation = Quaternion.LookRotation(lookDirection);

            SetState(CharacterState.Angry);
            pickupCoroutine = StartCoroutine(PickupAfterDelay(9f));
        }
        else if (lastReachedWaypoint == 13)
        {
            StartCoroutine(WaitAndContinue(78f));
        }

        textDisplayManager?.StartDisplayingText();
    }

    IEnumerator WaitAndContinue(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ContinueAfterStop();
    }

    public void ContinueAfterStop()
    {
        if (lastReachedWaypoint == 13 && currentWaypoint + 1 < waypoints.Length)
        {
            currentWaypoint++;
            StartMovingToWaypoint(currentWaypoint);
        }
    }

    IEnumerator PickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 lookDirection = HandPhone.transform.position - mother.transform.position;
        lookDirection.y = 0f;
        mother.transform.rotation = Quaternion.LookRotation(lookDirection);
        SetState(CharacterState.PickingUp);
        pickupCoroutine = StartCoroutine(PickupItemWithDelay(1f));
    }

    void SetState(CharacterState newState)
    {
        currentState = newState;
        int animationHash = currentState switch
        {
            CharacterState.Walking => WalkingHash,
            CharacterState.LookingAround => LookingAroundHash,
            CharacterState.Angry => AngryHash,
            CharacterState.PickingUp => PickUpHash,
            CharacterState.Idleee => IdleeeHash,
            _ => IdleeHash
        };
        motherAnimator.CrossFade(animationHash, 0.1f);
    }

    public void SetHandPhoneTransform()
    {
        if (HandPhone == null)
        {
            Debug.LogWarning("HandPhone not assigned!");
            return;
        }

        anak.SetActive(false);
        HandPhone.position = new Vector3(-32.40999984741211f, -1.3025000095367432f, 124.66999816894531f);
        HandPhone.rotation = new Quaternion(0.0915236547589302f, -0.6859381794929504f, -0.09547333419322968f, 0.7155396938323975f);
        HandPhone.localScale = new Vector3(1.0f, 0.970300018787384f, 1.1936999559402466f);
    }

    IEnumerator PickupItemWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (handBone != null)
        {
            pickupItem.transform.SetParent(handBone, true);
            pickupItem.transform.localPosition = Vector3.zero;
            pickupItem.transform.localRotation = Quaternion.identity;

            if (videoPlayer != null && videoPlayerAudio != null)
            {
                isMuted = true;
                videoPlayer.SetDirectAudioMute(0, true);
                videoPlayerAudio.mute = true;
                videoPlayerAudio.volume = 0f;
            }
            yield return StartCoroutine(Idle(1.5f));
        }
        else
        {
            Debug.LogWarning("Hand bone not assigned for pickup!");
        }

        pickupCoroutine = null;
    }

    IEnumerator Idle(float delay)
    {
        yield return new WaitForSeconds(delay);

        handBone.localPosition = new Vector3(0.00115f, 0.00044f, -0.00059f);
        handBone.localRotation = new Quaternion(0.673387945f, -0.00628960039f, -0.00370098976f, 0.739253342f);
        handBone.localScale = new Vector3(0.00208880496f, 0.00175327586f, 0.00212146551f);

        Vector3 lookDirection = lookTarget.transform.position - mother.transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Quaternion downwardRotation = Quaternion.Euler(72f, 0f, 0f);
            targetRotation = targetRotation * downwardRotation;
            mother.transform.rotation = Quaternion.Slerp(mother.transform.rotation, targetRotation, Time.deltaTime * 60f);

            Vector3 backwardDirection = -mother.transform.forward;
            float backwardDistance = 1f;
            mother.transform.position += backwardDirection * backwardDistance;
        }

        SetState(CharacterState.Idleee);
    }

    public void ShowAngryModel()
    {
        modelMarah.transform.position = modelUpdate.transform.position;
        modelMarah.transform.rotation = modelUpdate.transform.rotation;
        modelUpdate.SetActive(false);
        modelMarah.SetActive(true);
        motherAnimator = modelMarah.GetComponent<Animator>();

        SetState(CharacterState.Idleee);
    }

    public bool IsMoving() => isMoving;
    public int GetCurrentWaypoint() => currentWaypoint;
    public void LoadingScreenManager()
    {
        LoadingScreen.Instance.SwitchToScene("RoomsTutorial");
    }

    IEnumerator PlayFootstepSound()
    {
        while (true)
        {
            AudioManager.instance.PlaySFX(footstepSFXName, 0.09f);
            yield return new WaitForSeconds(footstepInterval); 
        }
    }

}