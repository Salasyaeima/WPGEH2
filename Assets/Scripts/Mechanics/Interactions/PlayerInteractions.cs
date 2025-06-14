using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


//Press F for Interact
public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private float holdTimeDuration = 5f;
    public float interactionDistance;
    public TMPro.TextMeshProUGUI interactionText;
    public GameObject interactionHoldGo;
    public UnityEngine.UI.Image holdProgress;
    public static Interactable heldItem = null;
    public static bool canInteractWithClothes = false;
    public InteractionMode currentInteractionMode = InteractionMode.Default;
    public bool canInteract = false;
    Interactable interactable = null;
    bool successfullHit = false;
    int interactableMask;
    Camera cam;
    bool isInteractionEnabled = true;
    bool isHittingDirt = false;

    public enum InteractionMode
    {
        None,
        DoorOnly,
        BedOnly,
        HidingOnly,
        TaskOnly,
        clothesOnly,
        Default
    }
    void Start()
    {
        cam = Camera.main;
        interactableMask = ~LayerMask.GetMask("Player", "ObstacleLayer") | LayerMask.GetMask("Dirt");
        ResetInterectionState();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        isInteractionEnabled = true;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableMask))
        {
            interactable = hit.collider.GetComponentInParent<Interactable>();

            if (heldItem is Broom broom)
            {
                if (hit.collider.CompareTag("Dirt"))
                {
                    isHittingDirt = true;
                    interactionText.text = LanguageManager.Instance.GetCurrentLanguage() == LanguageManager.Language.English ? "Hold {Left Click} to Sweep" : "Tahan {Klik Kiri} Menyapu";
                }
                else
                {
                    isHittingDirt = false;
                    interactionText.text = broom.Description();
                }
                interactionText.gameObject.SetActive(true);
                interactionHoldGo.SetActive(false);
                successfullHit = false;

                if (hit.collider.CompareTag("Dirt") && Input.GetMouseButton(0))
                {
                    BroomInHand broomInHand = FindObjectOfType<BroomInHand>();
                    if (broomInHand != null)
                    {
                        broomInHand.HandleDirt(hit.collider);
                    }
                    else
                    {
                        Debug.LogError("BroomInHand tidak ditemukan!");
                    }
                }
                return;
            }

            if (interactable != null)
            {
                if (SceneManager.GetActiveScene().name == "RoomsTutorial")
                {
                    if (canInteract)
                    {
                        switch (currentInteractionMode)
                        {
                            case InteractionMode.DoorOnly:
                                isInteractionEnabled = (interactable is Interactable && interactable.GetComponent<Door>());
                                break;
                            case InteractionMode.TaskOnly:
                                isInteractionEnabled = (interactable is Interactable && interactable.GetComponent<TaskTutorialTake>());
                                break;
                            case InteractionMode.clothesOnly:
                                isInteractionEnabled = (interactable is Item item &&
                                        item.GetComponent<ItemData>()?.category == ItemData.ItemCategory.Clothes && canInteractWithClothes) ||
                                    (interactable is Interactable &&
                                        interactable.GetComponent<Container>()?.containerType == Container.ContainerType.wardrobe && canInteractWithClothes);
                                break;
                            case InteractionMode.BedOnly:
                                isInteractionEnabled = (interactable is Interactable && interactable.GetComponent<BedInterect>());
                                break;
                            case InteractionMode.HidingOnly:
                                isInteractionEnabled = (interactable is Interactable && interactable.GetComponent<HidingMechanism>());
                                break;
                            case InteractionMode.None:
                                isInteractionEnabled = false;
                                break;
                            case InteractionMode.Default:
                            default:
                                isInteractionEnabled =
                                    (interactable is TaskTutorialTake) ||
                                    (interactable is Item item1 &&
                                        item1.GetComponent<ItemData>()?.category == ItemData.ItemCategory.Clothes && canInteractWithClothes) ||
                                    (interactable is Interactable &&
                                        interactable.GetComponent<Container>()?.containerType == Container.ContainerType.wardrobe && canInteractWithClothes) ||
                                    (interactable is Interactable && interactable.GetComponent<Door>()) ||
                                    (interactable is Interactable && interactable.GetComponent<BedInterect>() && canInteractWithClothes) ||
                                    (interactable is Interactable && interactable.GetComponent<HidingMechanism>());
                                break;
                        }
                    }
                    else
                    {
                        isInteractionEnabled = false;
                    }
                }


                if (isInteractionEnabled)
                {
                    HandleInteraction(interactable);
                    interactionText.text = interactable.Description();
                    interactionText.gameObject.SetActive(true);
                    interactionHoldGo.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
                    successfullHit = true;
                }
                else
                {
                    successfullHit = false;
                }
            }
            else
            {
                successfullHit = false;
                isInteractionEnabled = false;
            }
        }
        else
        {
            successfullHit = false;
            isInteractionEnabled = false;

            if (heldItem is Broom broom)
            {
                isHittingDirt = false;
                interactionText.text = broom.Description();
                interactionText.gameObject.SetActive(true);
                interactionHoldGo.SetActive(false);
            }
        }


        if (heldItem != null)
        {
            if (heldItem.interactionType == Interactable.InteractionType.Item && Input.GetMouseButtonUp(0))
            {
                StartCoroutine(delayDrop());
            }
            else if (heldItem.interactionType == Interactable.InteractionType.Click && Input.GetKeyDown(KeyCode.E) && !successfullHit)
            {
                StartCoroutine(delayDrop());
            }
        }

        if (!successfullHit)
        {
            interactionText.gameObject.SetActive(false);
            interactionHoldGo.SetActive(false);
        }

        if (heldItem is Broom)
        {
            interactionText.gameObject.SetActive(true);
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
                    interactable.OnHoldStart();
                    interactable.increaseHoldTime();
                    if (interactable.HoldTime() > holdTimeDuration)
                    {
                        interactable.Interact();
                        interactable.resetHoldTime();
                        interactable.OnHoldEnd();
                    }
                    else if (successfullHit == false && interactable.interactionType == Interactable.InteractionType.Hold)
                    {
                        interactable.resetHoldTime();
                        interactable.OnHoldEnd();
                    }
                }
                else
                {
                    interactable.resetHoldTime();
                    interactable.OnHoldEnd();
                }
                holdProgress.fillAmount = interactable.HoldTime() / holdTimeDuration;
                break;
            case Interactable.InteractionType.Item:
                if (interactable is Item item)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
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

    IEnumerator delayDrop()
    {
        yield return new WaitForSeconds(0.2f);
        if (heldItem != null)
        {
            heldItem.Drop();
            heldItem = null;
        }
    }

    public void SetInteractionMode(InteractionMode mode)
    {
        currentInteractionMode = mode;
    }

    void ResetInterectionState()
    {
        heldItem = null;
        canInteractWithClothes = false;
        currentInteractionMode = InteractionMode.Default;
    }
}

