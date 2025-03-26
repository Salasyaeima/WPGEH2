using UnityEngine;

public class DebugCollider : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask detectionLayer;
    public GameObject objectNearby
    {get;set;}
    

    public GameObject CheckNearbyObject()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, detectionLayer);
        foreach (Collider collider in colliders)
        {
            if (collider)
            {
                
            }
        }
        return objectNearby;
    }
}
