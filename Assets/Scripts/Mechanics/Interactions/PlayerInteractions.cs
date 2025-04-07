using UnityEngine;


//Press F for Interact
public class PlayerInteractions : MonoBehaviour
{

    public float interactionDistance;
    public TMPro.TextMeshProUGUI interactionText;
    public GameObject interactionHoldGo;
    public UnityEngine.UI.Image holdProgress;
    public static Item heldItem = null;


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
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        RaycastHit hit;
        bool successfullHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                HandleInteraction(interactable);
                interactionText.text = interactable.Description();
                interactionHoldGo.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
                successfullHit = true;
            }
        }
        interactionText.gameObject.SetActive(successfullHit);
        interactionHoldGo.SetActive(successfullHit);
    }

    void HandleInteraction(Interactable interactable)
    {
        KeyCode key = KeyCode.E;

        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                if (Input.GetKeyDown(key))
                {
                    if (interactable is Item item)
                    {
                        if (heldItem == null)
                        {
                            item.Interact();
                            heldItem = item;
                        }
                        else
                        {
                            heldItem.Drop();
                            heldItem = null;
                            item.Interact();
                        }
                    }
                    else if (interactable is HidingMechanism hidingBox)
                    {
                        hidingBox.Interact();
                    }
                    else
                    {
                        interactable.Interact();
                    }
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(key))
                {
                    interactable.increaseHoldTime();
                    if (interactable.HoldTime() > 5f)
                    {
                        interactable.Interact();
                        interactable.resetHoldTime();
                    }
                }
                else
                {
                    interactable.resetHoldTime();
                }
                holdProgress.fillAmount = interactable.HoldTime() / 5f;
                break;
        }
    }
}
