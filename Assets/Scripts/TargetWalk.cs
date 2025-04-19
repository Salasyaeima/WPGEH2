using System.Collections;
using UnityEngine;

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
        motherAnimator.Play("LookingAround");
        if (textDisplayManager != null)
        {
            textDisplayManager.StartDisplayingText();
        }

        if (lastReachedWaypoint == 9)
        {
            motherAnimator.Play("Angry");
            Debug.Log("Playing Angry animation at waypoint 9");
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
        Quaternion startRotation = mother.rotation;
        Vector3 targetPos = target.position;
        targetPos.y = mother.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - mother.position);
        float time = 0;

        while (time < duration)
        {
            mother.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        mother.rotation = targetRotation;
    }
    public bool IsMoving()
    {
        return isMoving;
    }

    public int GetCurrentWaypoint()
    {
        return currentWaypoint;
    }
}