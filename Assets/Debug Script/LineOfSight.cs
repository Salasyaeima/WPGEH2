using UnityEngine;
using System.Collections;
using Unity.Behavior;

public class LineOfSight : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 20f;
    [SerializeField]
    private float detectionAngle = 45f;
    //Opsional, untuk peletakan "Raycast" tepat di kepala
    [SerializeField]
    public float detectionHeight = 3f;
    [SerializeField]
    private LayerMask detectionLayer;
    [SerializeField]
    private bool showDebugVisuals;
    [SerializeField]
    [Tooltip("Tag before, triggered")]
    public string tagBefore;
    [SerializeField]
    [Tooltip("Tag after, triggered")]
    public string tagAfter;
    [SerializeField]
    private BehaviorGraph behavior;
    private Vector3 lastPosition;
    public GameObject DetectedTarget;

    public GameObject CheckInSight(GameObject potentialTarget)
    {
        if (potentialTarget)
        {
            if (CheckTargetInAngle(potentialTarget))
            {
                RaycastHit hit;
                Vector3 direction = potentialTarget.transform.position - transform.position;
                Physics.Raycast(transform.position + Vector3.up * detectionHeight, direction, out hit, detectionRange, detectionLayer);
                if (hit.collider != null && hit.collider.gameObject == potentialTarget)
                {
                    if (showDebugVisuals)
                    {
                        Debug.DrawLine(transform.position + Vector3.up * detectionHeight, potentialTarget.transform.position, Color.red);
                    }
                    //change tag when chased or not chased
                    hit.collider.gameObject.tag = tagAfter;

                    DetectedTarget = hit.collider.gameObject;
                }
                else
                {
                    if (DetectedTarget != null)
                    {
                        CheckLastSeen();
                    }
                    DetectedTarget = null;
                }
            }
            else
            {
                potentialTarget.tag = tagBefore;
            }
        }
        return DetectedTarget;
    }

    public void CheckLastSeen()
    {
        lastPosition = DetectedTarget.transform.position;
        behavior.BlackboardReference.SetVariableValue<Vector3>("Last Known Pos", lastPosition);
    }

    private bool CheckTargetInAngle(GameObject target)
    {
        Vector3 side1 = target.transform.position - this.transform.position;
        Vector3 side2 = this.transform.forward;

        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if ((angle < detectionAngle && angle > 0) || (angle > -detectionAngle && angle < 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        if (showDebugVisuals)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastPosition, 0.3f);
            // Gizmos.DrawLine(transform.position, Quaternion.Euler - transform.position);
            Gizmos.DrawSphere(transform.position + Vector3.up * detectionHeight, 1f);
        }
    }
}
