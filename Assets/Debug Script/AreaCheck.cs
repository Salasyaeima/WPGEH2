using UnityEditor.Rendering;
using UnityEngine;

public class AreaCheck : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 25;
    [SerializeField]
    private LayerMask DetectionLayer;
    [SerializeField]
    private bool showDebugVisuals;

    public GameObject DetectedTarget
    {
        get;set;
    }

    public GameObject CheckInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, DetectionLayer);
        if (colliders.Length > 0)
        {
            DetectedTarget = colliders[0].gameObject;
        }
        else
        {
            DetectedTarget = null;
        }
        return DetectedTarget;
    }

    void OnDrawGizmos()
    {
        if(!showDebugVisuals || !this.enabled) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
