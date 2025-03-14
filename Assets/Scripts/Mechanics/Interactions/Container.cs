using UnityEngine;

public class Container : Interactable
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string Description(){
        if(PlayerInteractions.heldItem != null) {
            return "Press {E} to interact.";
        }else{
            return " ";
        }
    }


    public override void Interact(){
        if(PlayerInteractions.heldItem != null){
            Collecting();
        }
    }

    void Collecting(){
        Debug.Log("Task selesai");
        Destroy(PlayerInteractions.heldItem.gameObject);
        PlayerInteractions.heldItem = null;
    }
}
