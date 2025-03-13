using UnityEngine;


//Press F for Interact
public class PlayerInteractions : MonoBehaviour
{

    public float interactionDistance;
    public TMPro.TextMeshProUGUI interactionText;


    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        interactionText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f, 0f));

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionDistance)){
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            bool successfullHit = false;

            if(interactable != null){
                HandleInteraction(interactable);
                interactionText.text = interactable.Description();
                interactionText.gameObject.SetActive(true);
                successfullHit = true;
            }else{
                interactionText.gameObject.SetActive(false);
            }
        }
    }

    void HandleInteraction(Interactable interactable){
        KeyCode key = KeyCode.F;

        switch(interactable.interactionType){
            case Interactable.InteractionType.Click:
                if(Input.GetKeyDown(key)){
                    interactable.Interact();
                }
                break;
        }
    }
}
