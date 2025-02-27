using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] Transform playerHand;
    bool isPlayerInAreaPick = false;
    PlayerInventory playerInventory;

    void Update()
    {
        if (isPlayerInAreaPick && Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInAreaPick = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerInAreaPick = false;
        playerInventory = null;
    }

    private void TryPickup()
    {
        if (playerInventory == null) return;
        if (playerInventory.heldObject == null)
        {
            GameObject objectToPickup = FindNearestObjectInArea();
            Debug.Log(objectToPickup);
            if (objectToPickup != null)
            {
                if (IsObjectAllowedToPick(objectToPickup))
                {
                    playerInventory.heldObject = objectToPickup;
                    AttachToHand(playerInventory.heldObject, playerHand);
                }
            }
        }
    }

    GameObject FindNearestObjectInArea()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale, Quaternion.identity);
        GameObject nearestObj = null;
        float minDistance = Mathf.Infinity;

        string currentTurn = GameManager.Instance.GetCurrentTurn();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(currentTurn))
            {
                float distance = Vector3.Distance(playerInventory.transform.position, col.transform.position);
                Debug.Log($"Objek: {col.name}, Jarak: {distance}");
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObj = col.gameObject;
                }
            }
        }

        return nearestObj;
    }

    void AttachToHand(GameObject obj, Transform hand)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        obj.transform.SetParent(hand);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    bool IsObjectAllowedToPick(GameObject obj)
    {
        string objectTag = obj.tag;
        return objectTag == GameManager.Instance.GetCurrentTurn();
    }
}
