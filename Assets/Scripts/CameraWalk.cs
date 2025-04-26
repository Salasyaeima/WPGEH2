using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform cameraPlayer;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float rotationTriggerDistance = 0.100f;

    bool isMoving = false;
    int currentWaypoint = 0;
    float initialDistanceToWaypoint = 0f;

    void Start()
    {
        if (cameraPlayer == null) Debug.LogError("Camera transform not assigned.");
        StartMovingToWaypoint(0);
    }

    void Update()
    {
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        if (currentWaypoint >= waypoints.Length) return;

        Vector3 targetPosition = waypoints[currentWaypoint].position;
        float distanceToWaypoint = Vector3.Distance(cameraPlayer.position, targetPosition);

        cameraPlayer.position = Vector3.MoveTowards(cameraPlayer.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction;
        if (currentWaypoint + 1 < waypoints.Length && distanceToWaypoint < initialDistanceToWaypoint * rotationTriggerDistance)
        {
            direction = (waypoints[currentWaypoint + 1].position - cameraPlayer.position).normalized;
        }
        else
        {
            direction = (targetPosition - cameraPlayer.position).normalized;
        }

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            cameraPlayer.rotation = Quaternion.Slerp(cameraPlayer.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        if (distanceToWaypoint < 0.1f)
        {
            isMoving = false;

            if (currentWaypoint + 1 < waypoints.Length)
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
        initialDistanceToWaypoint = Vector3.Distance(cameraPlayer.position, waypoints[currentWaypoint].position); // Simpan jarak awal
        isMoving = true;
    }
}