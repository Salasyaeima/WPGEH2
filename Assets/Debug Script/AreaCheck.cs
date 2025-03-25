using UnityEngine;

public class AreaCheck : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 25;
    [SerializeField]
    public LayerMask DetectionLayer;
    [SerializeField]
    private bool showDebugVisuals;
    [SerializeField]
    [Tooltip("Enable, to trigger click event by pressing \"e\"")]
    private bool activateClickEvent;

    public GameObject DetectedTarget
    {
        get;set;
    }

    private void Update() {
        if (activateClickEvent)
        {
            CheckInRadius();
        }
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

    public bool CheckClickEvent()
    {
        if (DetectedTarget != null && activateClickEvent)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if(!showDebugVisuals || !this.enabled) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
