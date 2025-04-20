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
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] TextDisplayManager textDisplayManager;
    [SerializeField] GameObject pickupItem;
    [SerializeField] Transform handBone;
    [SerializeField] Transform lookTarget;
    [SerializeField] GameObject modelUpdate;
    [SerializeField] GameObject modelMarah;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Vector3 pickupItemPosition = new Vector3(-0.055f, 0.008f, 0.043f);
    [SerializeField] Quaternion pickupItemRotation = Quaternion.Euler(-0.053f, 138.049f, 50.604f);
    [SerializeField] Vector3 pickupItemScale = new Vector3(0.2088804f, 0.2088804f, 0.2088804f);

    public enum CharacterState { Idlee, Walking, LookingAround, Angry, PickingUp };
    CharacterState currentState = CharacterState.Idlee;

    Animator motherAnimator;
    int currentWaypoint = 0;
    int lastReachedWaypoint = -1;
    bool isMoving = false;
    bool autoMove = false;
    Coroutine pickupCoroutine;

    public event Action<int> OnWaypointReached;

    void Awake()
    {
        ValidateReferences();
        motherAnimator = GetComponent<Animator>();
        if (mother == null)
        {
            Debug.LogError("Mother transform not assigned.");
            enabled = false;
        }
    }

    void Start()
    {
        isMoving = false;
        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioMute(0, true);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToWaypoint();
        }
    }

    void ValidateReferences()
    {
        if (mother == null) Debug.LogError("Mother transform not assigned.");
        if (waypoints == null || waypoints.Length == 0) Debug.LogError("Waypoints array is empty or not assigned.");
        if (videoPlayer == null) Debug.LogWarning("VideoPlayer not assigned.");
        if (handBone == null) Debug.LogWarning("Hand bone not assigned for pickup.");
        if (modelUpdate == null || modelMarah == null) Debug.LogWarning("Model references not assigned.");
    }

    void MoveToWaypoint()
    {
        if (currentWaypoint >= waypoints.Length) return;

        Vector3 direction = (waypoints[currentWaypoint].position - mother.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        mother.rotation = Quaternion.Slerp(mother.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        mother.position = Vector3.MoveTowards(mother.position, waypoints[currentWaypoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(mother.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            isMoving = false;
            lastReachedWaypoint = currentWaypoint;

            if (lastReachedWaypoint == 7)
            {
                videoPlayer.SetDirectAudioMute(0, false);
            }
            if (lastReachedWaypoint == 6 || lastReachedWaypoint == 9 || lastReachedWaypoint == 10)
            {
                StopAutoMove();
            }

            else if (autoMove && currentWaypoint + 1 < waypoints.Length)
            {
                currentWaypoint++;
                StartMovingToWaypoint(currentWaypoint);
            }
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
        isMoving = true;
        autoMove = true;
        motherAnimator.Play("Walking");

        if (textDisplayManager != null)
        {
            textDisplayManager?.StopDisplayingText();
        }

    }

    public void StopAutoMove()
    {
        isMoving = false;
        autoMove = false;
        if (lastReachedWaypoint == 6)
        {
            SetState(CharacterState.LookingAround);
        }
        else if (lastReachedWaypoint == 9)
        {
            SetState(CharacterState.Angry);
        }
        else if (lastReachedWaypoint == 10)
        {
            SetState(CharacterState.PickingUp);
            pickupCoroutine = StartCoroutine(PickupItemWithDelay(3f));
        }

        textDisplayManager?.StartDisplayingText();

    }

    void SetState(CharacterState newState)
    {
        currentState = newState;
        string animation = currentState switch
        {
            CharacterState.Walking => "Walking",
            CharacterState.LookingAround => "LookingAround",
            CharacterState.Angry => "Angry",
            CharacterState.PickingUp => "Pick Up",
            _ => "Idlee"
        };
        motherAnimator.Play(animation);
    }

    IEnumerator PickupItemWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (handBone != null)
        {
            pickupItem.transform.SetParent(handBone);
            pickupItem.transform.localPosition = pickupItemPosition;
            pickupItem.transform.localRotation = pickupItemRotation;
            pickupItem.transform.localScale = pickupItemScale;
            videoPlayer.SetDirectAudioMute(0, true);
            yield return StartCoroutine(Idle(3f));
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
        if (lookTarget != null)
        {
            yield return StartCoroutine(RotateToTarget(lookTarget, 1f));
        }

        SetState(CharacterState.Idlee);
    }

    IEnumerator RotateToTarget(Transform target, float duration)
    {
        Quaternion referenceRotation = new Quaternion(-0.0420385301f, 0.509046674f, -0.00661452068f, -0.859686315f);
        Vector3 referenceEuler = referenceRotation.eulerAngles;
        float startXRotation = mother.eulerAngles.x;
        float targetXRotation = 10.878f;
        float startYRotation = mother.eulerAngles.y;
        float startZRotation = mother.eulerAngles.z;
        float time = 0;

        while (time < duration)
        {
            float xRotation = Mathf.LerpAngle(startXRotation, targetXRotation, time / duration);
            float yRotation = Mathf.LerpAngle(startYRotation, referenceEuler.y, time / duration);
            float zRotation = Mathf.LerpAngle(startZRotation, referenceEuler.z, time / duration);
            mother.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
            time += Time.deltaTime;
            yield return null;
        }

        mother.rotation = Quaternion.Euler(targetXRotation, referenceEuler.y, referenceEuler.z);
    }
    public bool IsMoving()
    {
        return isMoving;
    }

    public int GetCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public void ShowAngryModel()
    {
        modelUpdate.SetActive(false);
        modelMarah.SetActive(true);
        motherAnimator = modelMarah.GetComponent<Animator>();
        mother = modelMarah.transform;

        SetState(CharacterState.Idlee);
    }
}
