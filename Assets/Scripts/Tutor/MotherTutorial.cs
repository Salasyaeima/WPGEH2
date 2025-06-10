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
    [SerializeField] Transform child;
    [SerializeField] WaypointData[] waypoints;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float chaseSpeed = 3.5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float detectionRange = 10f;
    [SerializeField] float detectionAngle = 60f;
    [SerializeField] TextDialogChild textDialogChild;
    [SerializeField] LayerMask childLayer;
    [SerializeField] Animator motherAnim;
    [SerializeField] HidingMechanism hidingMechanism;
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera playerCameraJumpScare;
    [SerializeField] Animator jumpscareAnimator;
    [SerializeField] CameraJumpScare cameraJumpScare;
    [SerializeField] SleepyBlinkEffect sleepyBlinkEffect;
    int currentWaypoint = 0;
    bool isMoving = false;
    bool isPaused = false;
    public bool isChasing = false;
    bool hasDetectedPlayer = false;
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
        if (!isPaused)
        {
            if (DetectChild() && !hidingMechanism.isHiding)
            {
                hasDetectedPlayer = true;
                isChasing = true;
                isMoving = false;
                targetPosition = child.position;
                motherAnim.Play("RunningI");
                textDialogChild.PauseDisplayingText();
            }
            else if (isChasing && (!DetectChild() || hidingMechanism.isHiding) && !hasDetectedPlayer)
            {
                isChasing = false;
                hasDetectedPlayer = false;
                StartMovingToWaypoint(currentWaypoint);
                textDialogChild.ResumeDisplayingText();
            }

            if (hasDetectedPlayer)
            {
                isChasing = true;
                ChaseChild();
            }

            if (isChasing)
            {
                ChaseChild();
            }
            else if (isMoving)
            {
                MoveToWaypoint();
            }
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

    bool DetectChild()
    {
        if (hidingMechanism.isHiding)
        {
            return false;
        }

        Vector3 directionToChild = (child.position - character.position).normalized;
        float distanceToChild = Vector3.Distance(character.position, child.position);
        float angleToChild = Vector3.Angle(character.forward, directionToChild);

        if (distanceToChild <= detectionRange && angleToChild <= detectionAngle / 2f)
        {
            if (Physics.Raycast(character.position, directionToChild, out RaycastHit hit, detectionRange, childLayer))
            {
                if (hit.transform == child)
                {
                    return true;
                }
            }
        }
        return false;
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

    void ChaseChild()
    {
        targetPosition = child.position;
        Vector3 direction = (targetPosition - character.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        character.rotation = Quaternion.Slerp(character.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        character.position = Vector3.MoveTowards(character.position, targetPosition, chaseSpeed * Time.deltaTime);
    }

    IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isChasing)
        {
            StartMovingToWaypoint(currentWaypoint + 1);
            textDialogChild.PauseDisplayingText();
        }
    }

    public void PauseMoving()
    {
        isPaused = true;
    }

    public void ResumeMoving()
    {
        isPaused = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textDialogChild.StopDisplayingText();
            textDialogChild.windowQuest.gameObject.SetActive(false);
            textDialogChild.intruksi.enabled = false;
            isPaused = true;
            cameraJumpScare.gameObject.SetActive(true);
            jumpscareAnimator.Play("Scene");
            playerCamera.enabled = false;
            playerCameraJumpScare.enabled = true;
            cameraJumpScare.StartShakeAndZoom();
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(3f);
        LoadingScreen.Instance.SwitchToScene("RoomsTutorial");
    }

    public void StopChasing()
    {
        isChasing = false;
        hasDetectedPlayer = false;
        StartMovingToWaypoint(currentWaypoint);
    }   
}
