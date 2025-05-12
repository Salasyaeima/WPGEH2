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
    Interactable interactable = null;
    bool successfullHit = false;
    int interactableMask;
    Camera cam;
    bool isInteractionEnabled = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        interactableMask = ~LayerMask.GetMask("Player");
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

            if (interactable != null)
            {
                if (SceneManager.GetActiveScene().name == "RoomsTutorial")
                {
                    isInteractionEnabled = interactable is TaskTutorialTake ||
                           (interactable is Item item &&
                            item.GetComponent<ItemData>()?.category == ItemData.ItemCategory.Clothes && canInteractWithClothes) ||
                           (interactable is Interactable &&
                            interactable.GetComponent<Container>()?.containerType == Container.ContainerType.wardrobe && canInteractWithClothes) || (interactable is Interactable &&
                            interactable.GetComponent<Door>() || (interactable is Interactable && interactable.GetComponent<BedInterect>()));
                }

                if (isInteractionEnabled)
                {
                    HandleInteraction(interactable);
                    interactionText.text = interactable.Description();
                    interactionText.gameObject.SetActive(true);
                    interactionHoldGo.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
                    successfullHit = true;
                }
            }
            else
            {
                successfullHit = false;
            }
        }
        else
        {
            successfullHit = false;
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

        if (heldItem is Broom broom)
        {
            Animator broomAnimator = broom.GetBroomAnimator();

            bool isSweeping = Input.GetMouseButton(0);

            if (broomAnimator != null)
            {
                broomAnimator.SetBool("IsSweeping", isSweeping);
            }

            if (isSweeping)
            {
                Transform sweepPoint = broom.GetSweepPoint();
                float sweepRadius = broom.GetSweepRadius();
                LayerMask dirtLayer = broom.GetDirtLayer();

                if (sweepPoint != null)
                {
                    Collider[] hits = Physics.OverlapSphere(sweepPoint.position, sweepRadius, dirtLayer);
                    foreach (Collider dirtCollider in hits)
                    {
                        if (dirtCollider.CompareTag("Dirt"))
                        {
                            Destroy(dirtCollider.gameObject, 2f);
                            Debug.Log("Cleaned dirt: " + dirtCollider.name);
                        }
                    }
                }
            }

            if (!successfullHit || !isInteractionEnabled)
            {
                interactionText.gameObject.SetActive(false);
                interactionHoldGo.SetActive(false);
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
                    interactable.increaseHoldTime();
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
}

