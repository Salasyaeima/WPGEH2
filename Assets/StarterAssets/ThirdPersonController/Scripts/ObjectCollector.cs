using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    [SerializeField] string requiredObjectTag = "Pickable";
    [SerializeField] Transform placementPoint;
    PlayerInventory playerInventory;
    bool isPlayerInAreaDrop = false;


    void Update()
    {
        if (isPlayerInAreaDrop && Input.GetKeyDown(KeyCode.E))
        {
            DropObeject();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInAreaDrop = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        isPlayerInAreaDrop = false;
        playerInventory = null;
    }

    private void DropObeject()
    {
        if (playerInventory != null && playerInventory.heldObject != null)
        {
            GameObject heldObj = playerInventory.heldObject;
            if (IsObjectAllowedToDrop(heldObj))
            {
                PlaceObejct(heldObj, playerInventory);
                GameManager.Instance.SwitchTurn();
            }
        }
    }

    void PlaceObejct(GameObject obj, PlayerInventory inventory)
    {
        obj.transform.SetParent(null);
        obj.transform.position = placementPoint.position;
        obj.transform.rotation = Quaternion.identity;

        obj.tag = "Untagged";

        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        inventory.heldObject = null;
    }

    bool IsObjectAllowedToDrop(GameObject obj)
    {
        string objectTag = obj.tag;
        return objectTag == GameManager.Instance.GetCurrentTurn();
    }
}
