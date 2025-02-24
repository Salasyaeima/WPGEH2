using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    [SerializeField] string requiredObjectTag = "Pickable";

    [SerializeField] Transform placementPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Masuk Player");
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            Debug.Log(inventory);
            if (inventory != null && inventory.heldObject != null)
            {
                Debug.Log("Masuk Obj");
                GameObject heldObj = inventory.heldObject;
                if (heldObj.CompareTag(requiredObjectTag))
                {
                    PlaceObejct(heldObj, inventory);
                }
            }
        }
    }

    void PlaceObejct(GameObject obj, PlayerInventory inventory)
    {
        obj.transform.SetParent(null);
        obj.transform.position = placementPoint.position;
        obj.transform.rotation = Quaternion.identity;

        obj.tag = "Untagged";
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        inventory.heldObject = null;
    }
}
