using UnityEngine;

public class Item : Interactable
{

    [SerializeField] Transform playerHand;
    private Rigidbody rb;

    private bool isHeld = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override string Description(){
        if(isHeld == false) {
            return "Press {E} to interact.";
        }else{
            return "Tes";
        }
    }

    void PickUp(){
        transform.SetParent(playerHand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        isHeld = true;
        PlayerInteractions.heldItem = this;
    }

    public override void Interact(){
    if (isHeld == false){
            PickUp();
        }else{
            Debug.Log("Penuh bang");
        }
    }
}
