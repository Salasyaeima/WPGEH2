using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] Transform playerHand;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            Debug.Log("PlayerInventory found: " + (playerInventory != null));
            if (playerInventory.heldObject == null)
            {
                GameObject objectToPickup = FindNearestObjectInArea();
                Debug.Log("Object to pickup: " + (objectToPickup != null));
                if (objectToPickup != null)
                {
                    playerInventory.heldObject = objectToPickup;
                    AttachToHand(playerInventory.heldObject, playerHand);
                    Debug.Log("Object picked up: " + objectToPickup.name);
                }
            }
        }
    }

    GameObject FindNearestObjectInArea()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Pickable"))
            {
                return col.gameObject;
            }
        }

        return null;
    }

    void AttachToHand(GameObject obj, Transform hand)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        obj.transform.SetParent(hand);
        obj.transform.localPosition = Vector3.zero;
    }


}
