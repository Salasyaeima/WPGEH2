using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TargetWalk : MonoBehaviour
{
    [SerializeField] Transform mother;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField]
    TextDisplayManager textDisplayManager;
    [SerializeField] GameObject pickupItem;
    [SerializeField] Transform handBone;
    [SerializeField] Transform lookTarget;
    [SerializeField] GameObject modelUpdate;
    [SerializeField] GameObject modelMarah;
    [SerializeField] VideoPlayer videoPlayer;
    Coroutine pickupCoroutine;
    int currentWaypoint = 0;
    int lastReachedWaypoint = -1;
    bool isMoving = false;
    bool autoMove = false;
    bool reachedWaypoint = false;
    Animator motherAnimator;



    void Start()
    {
        motherAnimator = GetComponent<Animator>();
        isMoving = false;
        videoPlayer.SetDirectAudioMute(0, true);
        Debug.Log("TargetWalk initialized");
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        Vector3 direction = (waypoints[currentWaypoint].position - mother.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        mother.rotation = Quaternion.Slerp(mother.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        mother.position = Vector3.MoveTowards(mother.position, waypoints[currentWaypoint].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(mother.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            isMoving = false;
            lastReachedWaypoint = currentWaypoint;
            Debug.Log($"Reached waypoint {currentWaypoint}");

            if (lastReachedWaypoint == 7)
            {
                videoPlayer.SetDirectAudioMute(0, false);
            }
            if (lastReachedWaypoint == 6 || lastReachedWaypoint == 9 || lastReachedWaypoint == 10)
            {
                StopAutoMove();
            }

            if (autoMove && currentWaypoint + 1 < waypoints.Length)
            {
                currentWaypoint++;
                StartMovingToWaypoint(currentWaypoint);
            }
        }
    }

    public void StartMovingToWaypoint(int waypointIndex = -1)
    {
        int targetWaypoint = waypointIndex >= 0 ? waypointIndex : currentWaypoint;
        if (targetWaypoint < waypoints.Length)
        {
            currentWaypoint = targetWaypoint;
            isMoving = true;
            autoMove = true;
            motherAnimator.Play("Walking");
            Debug.Log($"Starting move to waypoint {currentWaypoint}");

            if (textDisplayManager != null)
            {
                textDisplayManager.StopDisplayingText();
            }
        }
        else
        {
            Debug.LogWarning($"Waypoint index {targetWaypoint} out of range. Total waypoints: {waypoints.Length}");
        }
    }

    public void StopAutoMove()
    {
        isMoving = false;
        autoMove = false;
        if (lastReachedWaypoint == 6)
        {
            motherAnimator.Play("LookingAround");
        }
        if (textDisplayManager != null)
        {
            textDisplayManager.StartDisplayingText();
        }

        if (lastReachedWaypoint == 9)
        {
            motherAnimator.Play("Angry");
        }
        else if (lastReachedWaypoint == 10)
        {
            motherAnimator.Play("Pick Up");
            pickupCoroutine = StartCoroutine(PickupItemWithDelay(3f));
        }

    }

    IEnumerator PickupItemWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (handBone != null)
        {
            pickupItem.transform.SetParent(handBone);
            pickupItem.transform.localPosition = new Vector3(-0.055f, 0.008f, 0.043f);
            pickupItem.transform.localRotation = Quaternion.Euler(-0.053f, 138.049f, 50.604f);
            pickupItem.transform.localScale = new Vector3(0.2088804f, 0.2088804f, 0.2088804f);
            Debug.Log("Pickup item attached to hand");
            videoPlayer.SetDirectAudioMute(0, true);
            StartCoroutine(Idle(3f));
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

        motherAnimator.Play("Idlee");
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

    public void TampilkanMarah()
    {
        modelUpdate.SetActive(false);
        modelMarah.SetActive(true);

        motherAnimator = modelMarah.GetComponent<Animator>();
        mother = modelMarah.transform;
        motherAnimator.Play("Idlee");
    }
}
