using System.Collections;
using UnityEngine;

public class MotherTutorial : MonoBehaviour
{
    [System.Serializable]
    public class WaypointData
    {
        public Transform waypoint;
    }

    [SerializeField] Transform character;
    [SerializeField] WaypointData[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 5f;

    [SerializeField] TextDialogChild textDialogChild;
    [SerializeField] Animator motherAnim;
    int currentWaypoint = 0;
    bool isMoving = false;
    bool isPaused = false;
    Vector3 targetPosition;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            StartMovingToWaypoint(0);
        }
    }

    void Update()
    {
        if (isMoving && !isPaused)
        {
            MoveToWaypoint();
        }
    }

    void StartMovingToWaypoint(int index)
    {
        if (index >= 0 && index < waypoints.Length)
        {
            currentWaypoint = index;
            targetPosition = waypoints[currentWaypoint].waypoint.position;
            isMoving = true;
            motherAnim.Play("Walking");
        }
    }

    void MoveToWaypoint()
    {
        Vector3 direction = targetPosition - character.position;
        if (direction.sqrMagnitude < 0.01f)
        {
            isMoving = false;

            if (currentWaypoint == 2)
            {
                textDialogChild.ResumeDisplayingText();
                motherAnim.Play("Idlee");
                StartCoroutine(ResumeAfterDelay(3f));
                return;
            }

            if (currentWaypoint == 4)
            {
                motherAnim.Play("LookingAround");
                textDialogChild.ResumeDisplayingText();
                StartCoroutine(ResumeAfterDelay(12f));
                return;
            }

            if (currentWaypoint + 1 < waypoints.Length)
            {
                StartMovingToWaypoint(currentWaypoint + 1);
            }
        }
        else
        {
            direction.Normalize();
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            character.rotation = Quaternion.Slerp(character.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            character.position = Vector3.MoveTowards(character.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartMovingToWaypoint(currentWaypoint + 1);
        textDialogChild.PauseDisplayingText();
    }

    public void PauseMoving()
    {
        isPaused = true;
    }

    public void ResumeMoving()
    {
        isPaused = false;
    }
}
