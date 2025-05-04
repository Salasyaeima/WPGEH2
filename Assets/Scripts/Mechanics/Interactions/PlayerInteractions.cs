using UnityEngine;


//Press F for Interact
public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private float holdTimeDuration = 5f;
    public float interactionDistance;
    public TMPro.TextMeshProUGUI interactionText;
    public GameObject interactionHoldGo;
    public UnityEngine.UI.Image holdProgress;
    public static Item heldItem = null;
    Interactable interactable = null;
    bool successfullHit = false;
    int interactableMask;
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        interactableMask = ~LayerMask.GetMask("Player");
        interactionText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        interactionText.gameObject.SetActive(successfullHit);
        interactionHoldGo.SetActive(successfullHit);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableMask))
        {
            interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                HandleInteraction(interactable);
                interactionText.text = interactable.Description();
                interactionHoldGo.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
                successfullHit = true;
            }
        }
        else
        {
            successfullHit = false;
        }

        if (heldItem != null && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse dilepas!");
            heldItem.Drop();
            heldItem = null;
        }

    }

    void HandleInteraction(Interactable interactable)
    {
        KeyCode key = KeyCode.E;

        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                if (Input.GetKeyDown(key))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(key) && !heldItem)
                {
                    interactable.increaseHoldTime();
                    Debug.Log("Waktu: "+ interactable.HoldTime());
                    if (interactable.HoldTime() > holdTimeDuration)
                    {
                        interactable.Interact();
                        interactable.resetHoldTime();
                    }
                    else if (successfullHit == false && interactable.interactionType == Interactable.InteractionType.Hold)
                    {
                        interactable.resetHoldTime();
                    }
                }
                else
                {
                    interactable.resetHoldTime();
                }
                holdProgress.fillAmount = interactable.HoldTime() / holdTimeDuration;
                break;
            case Interactable.InteractionType.Item:
                if (interactable is Item item)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Mouse ditekan");
                        if (heldItem == null)
                        {
                            item.Interact();
                            heldItem = item;
                        }
                    }
                }
                break;
        }
    }
}
