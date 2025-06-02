using UnityEngine;
using System.Collections;

public class Item : Interactable
{
    [SerializeField] Transform playerHand;
    [SerializeField] string interectionSFXName = "Ambilbarang";

    private Rigidbody rb;
    private Collider itemCollider;

    public bool isHeld = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }

    public override string Description()
    {
        if (isHeld == false)
        {
            return "Press Left Mouse Button to interact.";
        }
        else
        {
            return " ";
        }
    }

    void PickUp()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        itemCollider.enabled = false;
        isHeld = true;
        PlayerInteractions.heldItem = this;

        StartCoroutine(MoveToHand());
    }

    IEnumerator MoveToHand()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, playerHand.position, elapsed / duration);
            transform.rotation = Quaternion.Lerp(startRot, playerHand.rotation, elapsed / duration);
            yield return null;
        }

        transform.SetParent(playerHand);
    }

    public override void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        itemCollider.enabled = true;
        isHeld = false;
        PlayerInteractions.heldItem = null;
    }

    public override void Interact()
    {
        ItemData itemData = GetComponent<ItemData>();
        AudioManager.instance.PlaySFX(interectionSFXName, 0.5f);


        if (itemData != null && itemData.category == ItemData.ItemCategory.Clothes)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "RoomsTutorial")
            {
                if (!PlayerInteractions.canInteractWithClothes)
                {
                    Debug.Log("Kamu belum boleh ambil pakaian!");
                    return;
                }
            }
        }

        PickUp();
    }

    void Update()
    {

    }
}
