using UnityEngine;
using Unity.Behavior;

public class LineOfSight : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 20f;
    //Opsional, untuk peletakan "Raycast" tepat di kepala
    [SerializeField]
    private float detectionHeight = 3f;
    [SerializeField]
    private LayerMask detectionLayer;
    [SerializeField]
    private bool showDebugVisuals;
    [SerializeField]
    private BehaviorGraph behavior;
    private Vector3 lastPosition;
    
    public GameObject DetectedTarget;

    public GameObject CheckInSight(GameObject potentialTarget)
    {
        RaycastHit hit;
        Vector3 direction = potentialTarget.transform.position - transform.position;
        Physics.Raycast(transform.position + Vector3.up * detectionHeight, direction, out hit, detectionRange, detectionLayer);

        if (hit.collider != null && hit.collider.gameObject == potentialTarget)
        {
            if (showDebugVisuals)
            {
                Debug.DrawLine(transform.position + Vector3.up * detectionHeight, potentialTarget.transform.position,Color.red);
            }

            DetectedTarget = hit.collider.gameObject;
        }
        else
        {
            CheckLastSeen();          
            DetectedTarget = null;
        }
        return DetectedTarget;
    }

    public void CheckLastSeen()
    {   
        if (DetectedTarget != null)
        {
            lastPosition = DetectedTarget.transform.position;
            behavior.BlackboardReference.SetVariableValue<Vector3>("Last Known Pos", lastPosition);
            Debug.Log(lastPosition);
        }
    }

    void OnDrawGizmos()
    {
        if (showDebugVisuals)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastPosition, 0.3f);
            Gizmos.DrawSphere(transform.position + Vector3.up * detectionHeight, 0.3f);
        }
    }
}
