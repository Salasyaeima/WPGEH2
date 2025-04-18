using UnityEngine;

public class TargetWalk : MonoBehaviour
{
    [SerializeField] Transform mother;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;
    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool autoMove = false;
    private Animator motherAnimator;

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
        Debug.Log($"Auto move stopped at waypoint {currentWaypoint}");
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