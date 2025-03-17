using UnityEngine;
using System.Collections;

public class Item : Interactable
{
    [SerializeField] Transform playerHand;
    private Rigidbody rb;
    private Collider itemCollider;

    private bool isHeld = false;
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
            return "Press {E} to interact.";
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

    public void Drop()
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
        PickUp();
    }

    void Update()
    {
        if (isHeld == true && PlayerInteractions.heldItem == this)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }

        }
    }
}
